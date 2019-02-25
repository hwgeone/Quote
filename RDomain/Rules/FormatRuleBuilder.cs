using RDomain.Rules.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Rules
{
    public class FormatRuleBuilder
    {
        public FormatRuleBuilder()
        {
        }

        public IRule GetRule(Type ruleType, object value)
        {
            List<object> args = new List<object>();
            args.Add(value);
            if (ruleType.Name == "FloatRule" || ruleType.Name == "YearMonthFormtRule")
            { }
            else if (ruleType.Name == "DateFormatRule")
            {
                args.Add(null);
            }
            else
                throw new Exception("Format rule not defined. ");

             return Activator.CreateInstance(ruleType, args.ToArray()) as IRule;
        }
    }
}
