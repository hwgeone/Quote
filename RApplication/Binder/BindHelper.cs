using RDomain.Entity;
using RDomain.Enum;
using RDomain.Flat;
using RDomain.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Binder
{
    public static class BindHelper
    {
        public static Project GetProject(IEnumerable<QuoteRevenue> flatObjs)
        {
            QuoteRevenue quoteRevenue = GetMainQuoteRevenue(flatObjs);
            Project project = new Project 
            {
                AccountName = quoteRevenue.AccountName,
                OpportunityOwner = quoteRevenue.OpportunityOwner,
                Territory = quoteRevenue.Territory,
                QuoteNumber = quoteRevenue.QuoteNumber.Trim(),
                StudySite = quoteRevenue.StudySite,
                ProjectLine = quoteRevenue.ProjectLine,
                TotalBooking = quoteRevenue.TotalBooking,
                USDCurrency = Currency.USD
            };

            project.Format();
            return project;
        }

        public static Income[] GetIncomes(OracleCost[] flatObjs, Guid projectId)
        {
            int length = flatObjs.Length;
            if (flatObjs == null || length == 0)
                return null;
            Income[] Incomes = new Income[length];
            for (int i = 0; i < length; i++)
            {
                Incomes[i] = GetIncome(flatObjs[i], projectId);
            }

            return Incomes;
        }

        public static Income GetIncome(OracleCost flatObj, Guid projectId)
        {
            if (flatObj == null)
                throw new NullReferenceException("flatObj");
            Income income = new Income
            {
                ProjectId = projectId,
                OracleCurrency = flatObj.OracleCurrency,
                CostAmount = flatObj.CostAmount,
                Comment = flatObj.Comment
            };
            income.Format();
            return income;
        }

        public static Order[] GetOrders(IEnumerable<QuoteRevenue> flatObjs, Guid projectId, string quoteNumber)
        {
           QuoteRevenue[] subs = GetSubQuoteRevenues(flatObjs).ToArray();
           if (subs == null || subs.Length == 0)
               return null;
           Order[] orders = new Order[subs.Length];
           for (int i = 0; i < subs.Length;i++ )
           {
               orders[i] = GetOrder(subs[i],projectId,quoteNumber);
           }
           return orders;
        }

        public static Order GetOrder(QuoteRevenue flatObj, Guid projectId, string quoteNumber)
        {
            if (flatObj == null)
                throw new NullReferenceException("flatObj");
            Order order = new Order
            {
                QuoteNumber = quoteNumber,
                ProjectId = projectId,
                Type = flatObj.Type,
                AmountCurrency = flatObj.AmountCurrency,
                Amount = flatObj.Amount,
                BookingCloseDate = flatObj.BookingCloseDate
            };
            order.Check();
            order.Format();
            return order;
        }

        private static IEnumerable<QuoteRevenue> GetSubQuoteRevenues(IEnumerable<QuoteRevenue> flatObjs)
        {
            return flatObjs.Where(o => o.Type.ToLower() == OrderType.Amendment);
        }

        private static QuoteRevenue GetMainQuoteRevenue(IEnumerable<QuoteRevenue> quoteRevenues)
        {
            QuoteRevenue qR = quoteRevenues.Where(q => q.Type.ToLower() == OrderType.NewProject).FirstOrDefault();
            float totalBooking = 0;
            totalBooking = quoteRevenues.Sum(q => q.Amount);
            qR.TotalBooking = totalBooking;

            if (qR == null)
                throw new NullReferenceException();
            return qR;
        }
    }
}
