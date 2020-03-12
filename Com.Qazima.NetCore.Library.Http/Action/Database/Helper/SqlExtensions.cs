using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Helper {
    public static class SqlExtensions {
        public static List<Dictionary<string, object>> DbDataReader2JSON(this DbDataReader dbDr) {
            return dbDr.DbDataReader2JSON(out _);
        }

        public static List<Dictionary<string, object>> DbDataReader2JSON(this DbDataReader dbDr, out int itemsCount) {
            //Read the results from the query
            List<Dictionary<string, object>> RowList = new List<Dictionary<string, object>>();
            itemsCount = 0;
            while (dbDr.Read()) {
                Dictionary<string, object> ColList = new Dictionary<string, object>();
                int i = 0;
                // For each fo the columns in the table. Find the column name.
                foreach (System.Data.DataRow dr in dbDr.GetSchemaTable().Rows) {
                    string key = dr[0].ToString(); // 0 : Index of the column name

                    if (ColList.Keys.Contains(key)) {
                        key = string.Format("{0}.{1}", dr[10].ToString(), dr[0].ToString()); // 10 : Index of the table name
                    }

                    ColList.Add(key, dbDr[i]);
                    i++;
                }

                itemsCount += 1;
                RowList.Add(ColList);
            }

            return RowList;
        }

        public static string ToSqlValue(this object value, string join = "") {
            string result = string.Empty;
            if (value.GetType().IsPrimitive) {
                switch (Type.GetTypeCode(value.GetType())) {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        result += value.ToString();
                        break;
                    case TypeCode.String:
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                        result += "'" + value.ToString() + "'";
                        break;
                    default:
                        throw new Exception("ToSqlValue : unknown type!");
                }
            } else if (value is IEnumerable<object>) {
                foreach (object elem in (IEnumerable<object>)value) {
                    result += elem.ToSqlValue() + join;
                }
                result = result.Substring(0, result.Length - join.Length);
            } else {
                throw new Exception("ToSqlValue : unknown type!");
            }
            return result;
        }
    }
}
