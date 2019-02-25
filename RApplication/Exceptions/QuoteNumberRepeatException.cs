using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    public class QuoteNumberRepeatException:ApplicationException
    {
          private const string ExceptionMessage = "导入的数据中发现重复的QuoteNumber。";
          public QuoteNumberRepeatException(string quoteNumber = "")
              : base(ExceptionMessage)
        {
 
        }
    }
}
