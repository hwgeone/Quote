using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    public class ExcelContentNotValidException:ApplicationException
    {
        private const string ExceptionMessage = "导入的Excel数据为空，或内容有问题。";
        public ExcelContentNotValidException():base(ExceptionMessage)
        {
 
        }
    }
}
