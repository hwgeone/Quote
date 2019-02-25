using RDomain.Rules.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ConvertorHelper;

namespace RDomain.Rules
{
    public class FloatRule:IRule
    {
        private const int decimals = 2;
        private float _value = 0;

        public FloatRule(object value)
        {
            _value = value.ToFloat();
        }

        public object Format()
        {
            return (float)Math.Round(_value,decimals);
        }
    }
}
