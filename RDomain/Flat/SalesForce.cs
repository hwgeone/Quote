using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Flat
{
    public class SalesForce
    {
        public string AccountName { get; set; }
        public string OpportunityOwner { get; set; }
        public string Territory { get; set; }
        public string Type { get; set; }
        public string QuoteNumber { get; set; }
        public string StudySite { get; set; }
        public string ProjectLine { get; set; }
        public string AmountCurrency { get; set; }
        public float Amount { get; set; }
        public string BookingCloseDate { get; set; }
    }
}
