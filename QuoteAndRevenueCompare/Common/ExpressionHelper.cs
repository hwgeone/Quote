using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace QuoteAndRevenueCompare.Common
{
    public class ExpressionHelper
    {
        /// <summary>
        /// a=>a.x
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression<Func<T, object>> GetMeberAccessLambda<T>(string property)
        {
            var param = Expression.Parameter(typeof(T), "p");
            Expression body = param;
            foreach (var member in property.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            var convert = Expression.Convert(body, typeof(object));
            return (Expression<Func<T, object>>)Expression.Lambda(convert, param);
        }

        /// <summary>
        /// a => a.x == "value"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetMeberEqualValueLambda<T>(string property,Type propertyType,object value)
        {
            var param = Expression.Parameter(typeof(T), "p");
            Expression body = param;
            foreach (var member in property.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            var convert = Expression.Convert(body, propertyType);


            Expression constvalue = Expression.Constant(value);
            var equal = Expression.Equal(convert, constvalue);

            return (Expression<Func<T, bool>>)Expression.Lambda(equal, param);
        }
    }
}