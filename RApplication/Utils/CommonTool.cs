using Infrastructure.ExcelHelper;
using RApplication.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Utils
{
    public static class CommonTool
    {
        public static DataTable FileStreamToDataTable(Stream stream, string fileName)
        {
            ExcelHelper excelHelper = new ExcelHelper(stream, fileName);

            DataTable dt = excelHelper.ExcelToDataTable(null, true);

            Check(dt);

            return dt;
        }

        private static void Check(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                if (dt == null || dt.Rows.Count <= 0)
                {
                    throw new ExcelContentNotValidException();
                }
            }
        }
    }
}
