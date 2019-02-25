using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public class SingelSheetExcelScaffold<THead> :ExcelScaffold, IScaffold, IExceptType
    {
        private IWorkbook workbook;
        private Dictionary<string, int> headNames;
        protected ISheet sheet;
        private IRow headLine;
        protected int currentRowIndex = 0;

        public SingelSheetExcelScaffold()
            : this("DefaultSheet")
        {

        }

        public virtual void SetExceptTypes(List<Type> types)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> HeadNames { get { return headNames; } }

        protected IRow GetRow(int rowIndex)
        {
            if (sheet == null)
                return null;
            return sheet.GetRow(rowIndex);
        }

        protected ICell GetCell(int rowIndex,int colInex)
        {
            if (sheet == null)
                return null;
            IRow row = GetRow(rowIndex);
            if (row == null)
                return null;
            return row.GetCell(colInex);
        }

        public SingelSheetExcelScaffold(string sheetname)
        {
            headNames = new Dictionary<string, int>();
            workbook = new HSSFWorkbook();
            if (!IsSheetExist())
            {
                sheet = workbook.CreateSheet(sheetname);
            }

            headLine = sheet.CreateRow(0);
            ExcelHeadNotContainedTypeFactory.AddExceptType(typeof(Guid));
            InitHead();
        }

        protected void InitHead()
        {
            Type type = typeof(THead);
            PropertyInfo[] PropertyList = type.GetProperties();

            int columnIndex = 0;
            foreach (PropertyInfo item in PropertyList)
            {
                if (ExcelHeadNotContainedTypeFactory.GetExceptedTypes().Contains(item.PropertyType))
                    continue;
                headNames.Add(item.Name, columnIndex);
                headLine.CreateCell(columnIndex).SetCellValue(item.Name);
                columnIndex++;
            }

            if(columnIndex == 0)
                throw new Exception(new StringBuilder().AppendFormat("Class： {0} 没有任何有效属性。",type.Name).ToString());
            currentRowIndex++;
        }

        public void SetCellContent(int rowIndex, int columnIndex, string value)
        {
            if (!IsRowExist(rowIndex))
            {
                IRow row = sheet.CreateRow(rowIndex);
                if (!IsCellExist(columnIndex, row))
                {
                    row.CreateCell(columnIndex).SetCellValue(value);
                }
                else
                {
                    row.GetCell(columnIndex).SetCellValue(value);
                }
            }
            else
            {
                IRow row = sheet.GetRow(rowIndex);
                if (!IsCellExist(columnIndex, row))
                {
                    row.CreateCell(columnIndex).SetCellValue(value);
                }
                else
                {
                    row.GetCell(columnIndex).SetCellValue(value);
                }
            }
        }

        protected bool IsSheetExist()
        {
            if (sheet == null)
            {
                return false;
            }
            return true;
        }

        protected bool IsSheetExist(int sheetIndex)
        {
            if (sheet == null)
            {
                return false;
            }
            return true;
        }

        protected bool IsRowExist(int rowIndex)
        {
            IRow row = sheet.GetRow(rowIndex);
            if (row == null)
            {
                return false;
            }
            return true;
        }

        protected bool IsCellExist(int colIndex, IRow row)
        {
            ICell cell = row.GetCell(colIndex);
            if (cell == null)
            {
                return false;
            }
            return true;
        }

        public virtual void SetCellContentByColumnName(int rowIndex, string columnName, string value)
        {
            int curcolumnIndex;
            if (headNames.TryGetValue(columnName, out curcolumnIndex))
            {
                SetCellContent(rowIndex, curcolumnIndex, value);
            }
        }

        /// <summary>
        /// 不应该暴露workbook，这边完全为了省事
        /// </summary>
        /// <returns></returns>
        public IWorkbook GetWorkBook()
        {
            return workbook;
        }
    }
}