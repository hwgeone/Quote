using RDomain.Rules.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ConvertorHelper;

namespace RDomain.Rules
{
    public class RuleKey : IEquatable<RuleKey>
    {
        private string _specialCode;
        private Type _type;

        public RuleKey(Type type, string specialCode = null)
        {
            _type = type;
            _specialCode = specialCode == null?"null":specialCode;
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RuleKey);
        }

        public bool Equals(RuleKey obj)
        {
            return this._specialCode == obj._specialCode && this._type == obj._type;
        }
    }

    public class FormatObjectFactory<TSource> where TSource : class
    {
        private TSource _source;
        private Dictionary<RuleKey, Type> _typeFormatRuleSource;
        public FormatObjectFactory(TSource source)
        {
            _source = source;
            _typeFormatRuleSource = new Dictionary<RuleKey, Type>();
           
                _typeFormatRuleSource.Add(new RuleKey(typeof(float)), typeof(FloatRule));
                _typeFormatRuleSource.Add(new RuleKey(typeof(string), "Date"), typeof(DateFormatRule));
        }

        private string GetCode(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("propertyName");
            if (propertyName.Contains("Date"))
                return "Date";
            else
                return null;
        }

        public void AddOrUpdateFormatRule(RuleKey key, Type value)
        {
            _typeFormatRuleSource[key] = value;
        }

        public void FormatObject()
        {
            Type t = _source.GetType();

            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                if (!item.CanWrite) continue;//该属性不可写，直接跳出  
                string name = item.Name;
                object value = item.GetValue(_source, null);
                string code = GetCode(item.Name);
                RuleKey key = new RuleKey(item.PropertyType, code);
                try
                {
                    if (_typeFormatRuleSource.ContainsKey(key))
                    {
                        Type ruleType = _typeFormatRuleSource[key];
                        IRule formatRule = new FormatRuleBuilder().GetRule(ruleType, value);
                        object targetValue = formatRule.Format();
                        item.SetValue(_source, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
