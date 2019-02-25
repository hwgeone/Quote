using Infrastructure.ConvertorHelper;
using RDomain.Rules.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Rules
{
    public class DateFormatRule : IRule
    {
        private string _dateformat = "yyyy/MM/dd";
        private string dateStr;
        private DateTime date;

        public DateFormatRule(object _date_str,string dateformat = null)
        {
            dateStr = _date_str.ToStringValue();
            if (!string.IsNullOrEmpty(dateformat))
            {
                _dateformat = dateformat;
            }
            Check(dateStr);
        }

        private void Check(string _date_str)
        {
            if (string.IsNullOrEmpty(_date_str))
                return;
            try
            {
                date = _date_str.ToDateTime();
            }
            catch (ArgumentNullException argex)
            {
                throw argex;
            }
            catch (FormatException fex)
            {
                throw fex;
            }
        }

        public virtual Object Format()
        {
            if (date == null)
                return string.Empty;
            return date.ToString(_dateformat);
        }
    }
}
