using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Flat
{
    public class OracleCost
    {
        public string QuoteNumber { get; set; }
        public string OracleCurrency { get; set; }
        public float CostAmount { get; set; }
        public string Status { get; set; }
        public string ProjectClosedDate { get; set; }
        public string Comment { get; set; }
    }
}
