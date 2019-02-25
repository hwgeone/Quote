using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Extend
{
    public static class ListEx
    {
        public static bool IsHasDepulicateItem<TSource, Tkey>(this List<TSource> list, Func<TSource, Tkey> propertySelector)
        {
            var duplicates = list.GroupBy(propertySelector).Where(a => a.Count() > 1);

            foreach (var agent in duplicates)
            {
                return true;
            }

            return false;
        }

    }
}
