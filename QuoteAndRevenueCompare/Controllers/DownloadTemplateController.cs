using QuoteAndRevenueCompare.Common;
using RApplication;
using RDomain.Entity;
using RDomain.Flat;
using RDomain.Struct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuoteAndRevenueCompare.Controllers
{
    public class DownloadTemplateController:Controller
    {
        public ReportApp app { get; set; }

        public FileResult DownloadExchangeRateTemplate()
        {
            SingelSheetExcelScaffold<FlatExchangeRate> scaffod = new SingelSheetExcelScaffold<FlatExchangeRate>("ExchageRate");
            string yearmonth = DateTime.Now.ToString("yyyy/MM");
            int rowIdex = 1;
            foreach (var cur in Currency.GetCurrencys(true))
            {
                scaffod.SetCellContentByColumnName(rowIdex, "Currency",cur);
                scaffod.SetCellContentByColumnName(rowIdex, "YearMonthDate", yearmonth);
                rowIdex++;
            }
            var memoryStream = new MemoryStream();
            scaffod.GetWorkBook().Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string s = MimeMapping.GetMimeMapping(".xls");
            return File(memoryStream, s,"ExchangeRate.xls");
        }

        public FileResult DownloadSalesForceTemplate()
        {
            SingelSheetExcelScaffold<SalesForce> scaffod = new SingelSheetExcelScaffold<SalesForce>("SalesForce");
        
            var memoryStream = new MemoryStream();
            scaffod.GetWorkBook().Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string s = MimeMapping.GetMimeMapping(".xls");
            return File(memoryStream, s,"SalesForce.xls");
        }

        public FileResult DownloadOracleKickOffTemplate()
        {
            SingelSheetExcelScaffold<OracleKickOff> scaffod = new SingelSheetExcelScaffold<OracleKickOff>("OracleKickOff");

            var memoryStream = new MemoryStream();
            scaffod.GetWorkBook().Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string s = MimeMapping.GetMimeMapping(".xls");
            return File(memoryStream, s,"OracleKickOff.xls");
        }

        public FileResult DownloadOracleCost()
        {
            SingelSheetExcelScaffold<OracleCost> scaffod = new SingelSheetExcelScaffold<OracleCost>("OracleCost");

            var memoryStream = new MemoryStream();
            scaffod.GetWorkBook().Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string s = MimeMapping.GetMimeMapping(".xls");
            return File(memoryStream, s,"OracleCost.xls");
        }

        public FileResult DownloadReport()
        {
            List<Project> projects = app.GetProjects().ToList();
            Guid[] ids = projects.Select(p => p.Id).ToArray();
            List<Order> orders = app.GetOrders(ids).ToList();
            List<Income> incomes = app.GetIncomes(ids).ToList();

            SingleSheetExcelGroupedReportingScaffold<Project, Order,Income> scaffod
                = new SingleSheetExcelGroupedReportingScaffold<Project, Order, Income>(projects, orders,incomes, "QuoteNumber");
            Total total = new Total();
            total.TotalBooking = projects.Sum(a => a.TotalBooking);
            total.TotalCost = projects.Sum(a=>a.FinalCost);
            total.Difference = total.TotalCost - total.TotalBooking;
            total.Ratio = total.TotalCost / total.TotalBooking;

            scaffod.SetTotal<Total>(total);
            var memoryStream = new MemoryStream();
            scaffod.GetWorkBook().Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string s = MimeMapping.GetMimeMapping(".xls");
            return File(memoryStream, s, "Report.xls");
        }
    }
}