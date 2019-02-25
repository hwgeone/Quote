using RDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Entity
{
    public class ExchangeRate:Entity,IFormat
    {
        public string Currency { get; set; }
        public float Rate { get; set; }
        /// <summary>
        /// yyyy/MM
        /// </summary>
        public string YearMonthDate { get; set; }

        public void Format()
        {
            FormatObjectFactory<ExchangeRate> factory = new FormatObjectFactory<ExchangeRate>(this);
            factory.AddOrUpdateFormatRule(new RuleKey(typeof(string),"Date"),typeof(YearMonthFormtRule));
            factory.FormatObject();
        }
    }
}
