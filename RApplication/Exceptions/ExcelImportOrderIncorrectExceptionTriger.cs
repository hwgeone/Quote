using RDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    public class ExcelImportOrderIncorrectExceptionTriger
    {
        private const string ProjectNotExistExceptionMessage = "QuoteNumber为{0}的数据行不存在数据库中，请确保excel的导入顺序是正确的。";
        private const string KickOffDateIsEmpty = "QuoteNumber为{0}的数据的KickOffDate为空，请确保excel的导入顺序是正确的。";
        public ExcelImportOrderIncorrectExceptionTriger(Project project)
        {
           if(project == null)
               throw new Exception(string.Format(ProjectNotExistExceptionMessage,project.QuoteNumber));
            else if(string.IsNullOrEmpty(project.KickOffDate))
               throw new Exception(string.Format(KickOffDateIsEmpty,project.QuoteNumber));
        }
    }
}
