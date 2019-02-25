using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public interface IScaffold
    {
        void SetCellContent(int rowIndex, int columnIndex, string value);
        void SetCellContentByColumnName(int rowIndex, string columnName, string value);
        IWorkbook GetWorkBook();
    }
}