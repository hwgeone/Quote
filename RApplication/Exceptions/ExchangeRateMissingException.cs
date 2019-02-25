using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    public class ExchangeRateMissingException : ApplicationException
    {
        private const string ExceptionMessage = "数据库中未查找到YearMonth为{0}，Currency为{1}的记录。信息缺失。请导入相关数据。";
        public ExchangeRateMissingException(string yearmonth, string currency)
            : base(string.Format(ExceptionMessage, yearmonth, currency))
        {

        }
    }
}
