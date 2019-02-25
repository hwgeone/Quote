using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Rules
{
    public class YearMonthFormtRule:DateFormatRule
    {
        private const string yearmonth = "yyyy/MM";
        public YearMonthFormtRule(Object value)
            : base(value,yearmonth)
        { }
    }
}
