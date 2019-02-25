using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public interface IExceptType
    {
        void SetExceptTypes(List<Type> types);
    }
}