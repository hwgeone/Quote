using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    public class ExcelContentNotMatchBusinessRuleException:ApplicationException
    {
        private const string ExceptionMessage = "你导入的Excel中QuoteNumber为{0}的数据行已存数据库中，但却没有任何新的amendment类型的数据";
        public ExcelContentNotMatchBusinessRuleException(string quoteNumber):base(string.Format(ExceptionMessage,quoteNumber))
        {
 
        }
    }
}
