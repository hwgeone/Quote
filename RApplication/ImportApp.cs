using Infrastructure.ConvertorHelper;
using Infrastructure.ExcelHelper;
using RApplication.Binder;
using RDomain.Entity;
using RDomain.Enum;
using RDomain.Flat;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using RApplication.Extend;
using RApplication.Exceptions;
using RApplication.Utils;

namespace RApplication
{
    public class ImportApp
    {
        private IProjectRepository _projectRepository;
        private IOrderRepository _orderRepository;
        private IIncomeRepository _incomeRepository;
        private IExchangeRateRepository _exchangeRateRepository;
        public ImportApp(IProjectRepository projectRepository, IOrderRepository orderRepository,
            IIncomeRepository incomeRepository, IExchangeRateRepository exchangeRateRepository)
        {
            _projectRepository = projectRepository;
            _orderRepository = orderRepository;
            _incomeRepository = incomeRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task ImportForSalesForce(Stream stream, string fileName)
        {
            DataTable dt = CommonTool.FileStreamToDataTable(stream, fileName);
            List<QuoteRevenue> list = ConvertHelper<QuoteRevenue>.ConvertToList(dt);
            await StartFirstStage(list);
        }

        public async Task ImportForOracleKickOff(Stream stream, string fileName)
        {
            DataTable dt = CommonTool.FileStreamToDataTable(stream, fileName);
            List<OracleKickOff> list = ConvertHelper<OracleKickOff>.ConvertToList(dt);
            await StartSecondStage(list);
        }

        public async Task ImportForOracleCost(Stream stream, string fileName)
        {
            DataTable dt = CommonTool.FileStreamToDataTable(stream, fileName);
            List<OracleCost> list = ConvertHelper<OracleCost>.ConvertToList(dt);
            await StartThirdStage(list);
        }

        public async Task ImportForExchangeRate(Stream stream, string fileName)
        {
            DataTable dt = CommonTool.FileStreamToDataTable(stream, fileName);
            List<ExchangeRate> list = ConvertHelper<ExchangeRate>.ConvertToList(dt);
            list = list.Where(r => r.Rate > 0).ToList();
            await StartFourthStage(list);
        }

        private async Task StartFourthStage(List<ExchangeRate> list)
        {
            await Task.Factory.StartNew(async()=>await FourthBatch(list));
        }

        private Task FourthBatch(List<ExchangeRate> list)
        {
            foreach (var item in list)
            {
                item.Format();
            }
            using (TransactionScope scope = new TransactionScope())
            {
                _exchangeRateRepository.BatchAdd(list.ToArray());

                _exchangeRateRepository.Save();
            }

            return Task.CompletedTask;
        }

        private async Task StartThirdStage(List<OracleCost> oracleCosts)
        {
            await Task.Factory.StartNew(async () => await ThirdBatch(oracleCosts));
        }

        private async Task StartSecondStage(List<OracleKickOff> oracleKickOffs)
        {
            await Task.Factory.StartNew(async () => await SecondBatch(oracleKickOffs));
        }

        private async Task StartFirstStage(List<QuoteRevenue> list)
        {
            await Task.Factory.StartNew(async () => await FirstBatch(list));
        }

        private Task ThirdBatch(List<OracleCost> list)
        {
            var groupedOracleCostList = list
  .GroupBy(q => q.QuoteNumber, (quoteNumber, quoteRevenues) => new
  {
      key = quoteNumber,
      value = quoteRevenues
  });
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var group in groupedOracleCostList)
                {
                    string quoteNumber = group.key.Trim();
                    Project dbProject = _projectRepository.GetBy(p => p.QuoteNumber == quoteNumber).FirstOrDefault();
                    new ExcelImportOrderIncorrectExceptionTriger(dbProject);
                    Income[] incomes = BindHelper.GetIncomes(group.value.ToArray(), dbProject.Id);
                 
                    //根据kickoff的年月和币种得到汇率
                    string yearmonth = dbProject.GetYearAndMonthString();
                    List<ExchangeRate> rates = _exchangeRateRepository.GetBy(r => r.YearMonthDate == yearmonth).ToList();
                    if (rates.Count == 0)
                    {
                        throw new Exception("请先上传当月的Exchange rate");
                    }
                    else
                    {
                        float finalCost =0, different = 0;
                        foreach (var income in incomes)
                        {
                            ExchangeRate rate = rates.FirstOrDefault(r=>r.Currency == income.OracleCurrency);
                            if (rate == null)
                                throw new ExchangeRateMissingException(yearmonth,income.OracleCurrency);
                            finalCost += income.CostAmount / rate.Rate;
                            income.ExchageRate = rate.Rate;
                            income.Format();
                        }

                        OracleCost one = group.value.First();
                        different = finalCost - dbProject.TotalBooking;
                        dbProject.FinalCost = finalCost;
                        dbProject.different = different;
                        dbProject.Status = one.Status;
                        dbProject.ProjectClosedDate = one.ProjectClosedDate;
                        dbProject.Format();
                       _incomeRepository.BatchAdd(incomes);
                       _projectRepository.Update(dbProject);
                    }
                }
                _projectRepository.Save();
                scope.Complete();
            }
            return Task.CompletedTask;
        }

        private Task SecondBatch(List<OracleKickOff> list)
        {
            if (list.IsHasDepulicateItem<OracleKickOff, string>(p => p.QuoteNumber))
            {
                throw new QuoteNumberRepeatException();
            }
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var item in list)
                {
                    item.Format();
                    _projectRepository.Update(p => p.QuoteNumber == item.QuoteNumber.Trim(), p => new Project { KickOffDate = item.KickOffDate });
                }
                scope.Complete();
            }
            return Task.CompletedTask;
        }

        private Task FirstBatch(List<QuoteRevenue> list)
        {
            var groupedQuoteRevenueList = list
    .GroupBy(q => q.QuoteNumber, (quoteNumber, quoteRevenues) => new
    {
        key = quoteNumber,
        value = quoteRevenues
    });
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var group in groupedQuoteRevenueList)
                {
                    string quoteNumber = group.key.Trim();
                    Project dbProject = _projectRepository.GetBy(p => p.QuoteNumber == quoteNumber).FirstOrDefault();

                    if (dbProject == null)
                    {
                        Project project = BindHelper.GetProject(group.value);
                        Order[] orders = BindHelper.GetOrders(group.value, project.Id,project.QuoteNumber);

                        _projectRepository.Add(project);
                        if (orders == null || orders.Length == 0)
                        { }
                        else
                        {
                            _orderRepository.BatchAdd(orders);
                        }
                        _orderRepository.Save();
                        scope.Complete();
                    }
                    else
                    {
                        float totalBooking = 0;
                        Order[] orders = BindHelper.GetOrders(group.value, dbProject.Id,dbProject.QuoteNumber);
                        if (orders == null || orders.Length == 0)
                        {
                            throw new ExcelContentNotMatchBusinessRuleException(quoteNumber);
                        }
                        totalBooking = dbProject.TotalBooking + orders.Sum(o => o.Amount);

                        _projectRepository.Update(p => p.QuoteNumber == quoteNumber, q => new Project { TotalBooking = totalBooking });
                        _orderRepository.BatchAdd(orders);
                    }
                }
                _orderRepository.Save();
                scope.Complete();
            }
            return Task.CompletedTask;
        }
    }
}
