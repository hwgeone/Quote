using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public static class ExcelHeadNotContainedTypeFactory
    {
        private static List<Type> exceptedTypes = new List<Type>();

        public static void AddExceptType(Type type)
        {
            exceptedTypes.Add(type);
        }

        public static void RemoveExceptType(Type type)
        {
            exceptedTypes.Remove(type);
        }

        public static List<Type> GetExceptedTypes()
        {
            return exceptedTypes;
        }
    }
}