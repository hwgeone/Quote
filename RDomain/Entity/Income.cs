using RDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Entity
{
    public class Income:Entity,IFormat
    {
        public Guid ProjectId { get; set; }
        public string QuoteNumber
        {
            get;
            set;
        }
        /// <summary>
        /// oracle
        /// </summary>
        public string OracleCurrency { get; set; }
        /// <summary>
        /// oracle
        /// </summary>
        public float CostAmount { get; set; }

        public string StartDate { get; set; }

        public float ExchageRate { get; set; }

        public string Comment { get; set; }

        public void Format()
        {
            new FormatObjectFactory<Income>(this).FormatObject();
        }
    }
}
