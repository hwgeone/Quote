using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace QuoteAndRevenueCompare.Utils
{
    public class DataTableHtmlGenerator<T> where T : class
    {
        private StringBuilder sb;
        private Type type;
        private PropertyInfo[] propertyList; 
        private List<T> _values;

        public DataTableHtmlGenerator(List<T> values)
        {
            sb = new StringBuilder();
            type = typeof(T);
            propertyList = type.GetProperties();
            _values = values;
            Scaffold();
        }

        public string GetHtml()
        {
            return sb.ToString();
        }

        private void Scaffold()
        {
            sb.AppendFormat("<table class='ui celled table'>{0} {1}</table>",GetHead(),GetBody(_values));
        }

        private string GetHead()
        {
            string columns = "";
            StringBuilder headBuilder = new StringBuilder();
            foreach (PropertyInfo item in propertyList)
            {
                if (item.PropertyType == typeof(Guid))
                {
                    continue;
                }
                columns += "<th>"+item.Name+"</th>";
            }
            headBuilder.AppendFormat("<thead><tr>{0}</tr></thead>", columns);

            return headBuilder.ToString();
        }

        private string GetBody(List<T> values)
        {
            StringBuilder bodyBuilder = new StringBuilder();
            string rows = "";
            foreach (var item in values)
            {
                StringBuilder rowBuilder = new StringBuilder();
                string columns = "";
                foreach (var column in propertyList)
                {
                    if (column.PropertyType == typeof(Guid))
                    {
                        continue;
                    }
                    object temp = column.GetValue(item, null);
                    if (temp != null)
                    {
                        string colvalue = column.GetValue(item, null).ToString();
                        columns += "<td data-label='" + column.Name + "'>" + colvalue + "</td>";
                    }
                }
                rowBuilder.AppendFormat(" <tr>{0}</tr>",columns);
                rows += rowBuilder.ToString();
            }

            bodyBuilder.AppendFormat("<tbody>{0}</tbody>", rows);

            return bodyBuilder.ToString();
        }
    }
}