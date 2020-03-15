using Com.Qazima.NetCore.Library.Http.Action.Database.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Database
{
    public class Criteria
    {
        public bool? CaseSensitive { get; set; }

        public CriteriaOperator Operator { get; set; }

        public object Value { get; set; }

        public static Criteria Parse(string s)
        {
            Criteria result;
            TryParse(s, out result);
            return result;
        }

        public static bool TryParse(string s, out Criteria criteria)
        {
            bool result = true;
            criteria = null;
            try
            {
                criteria = JsonSerializer.Deserialize<Criteria>(s);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public string toSqlWhereClause(string columnName)
        {
            string result = caseSensitiveColumn(columnName);

            switch (Operator)
            {
                case CriteriaOperator.Contains:
                    {
                        result += " like " + caseSensitiveValue("%" + Value + "%");
                    }
                    break;
                case CriteriaOperator.Different:
                    {
                        result += " != " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.EndWith:
                    {
                        result += " like" + caseSensitiveValue("%" + Value);
                    }
                    break;
                case CriteriaOperator.Equal:
                    {
                        result += " = " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.Greater:
                    {
                        result += " > " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.GreaterOrEqual:
                    {
                        result += " >= " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.In:
                    {
                        if (((IEnumerable<object>)Value).Any())
                        {
                            if (((IEnumerable<object>)Value).First().GetType().IsPrimitive)
                            {
                                result += " in (" + caseSensitiveValue(((IEnumerable<object>)Value).ToSqlValue(",")) + ")";
                            }
                            else
                            {
                                result += " in (" + caseSensitiveValue(((IEnumerable<object>)Value).ToSqlValue(",")) + ")";
                            }
                        }
                    }
                    break;
                case CriteriaOperator.Lower:
                    {
                        result += " < " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.LowerOrEqual:
                    {
                        result += " <= " + caseSensitiveValue(Value);
                    }
                    break;
                case CriteriaOperator.StartWith:
                    {
                        result += " like " + caseSensitiveValue(Value + "%");
                    }
                    break;
                default:
                    {
                        result += " = " + caseSensitiveValue(Value);
                    }
                    break;
            }

            return result;
        }

        private string caseSensitiveColumn(string columnName)
        {
            string result;
            if (!CaseSensitive.HasValue || CaseSensitive.Value)
            {
                result = columnName;
            }
            else
            {
                result = "lower(" + columnName + ")";
            }
            return result;
        }

        private string caseSensitiveValue(object value)
        {
            string result = string.Empty;
            if (value.GetType() != string.Empty.GetType())
            {
                result += value.ToString();
            }
            else
            {
                if (!CaseSensitive.HasValue || CaseSensitive.Value)
                {
                    result += value.ToString().ToSqlValue();
                }
                else
                {
                    result += value.ToString().ToLower().ToSqlValue();
                }
            }
            return result;
        }
    }
}