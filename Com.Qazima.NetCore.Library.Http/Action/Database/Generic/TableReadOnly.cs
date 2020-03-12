using Com.Qazima.NetCore.Library.Http.Action.Database.Helper;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public class TableReadOnly : Action, ITableReadOnly {
        public event EventHandler<ActionGetEventArgs> OnActionGet;

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) {
            ConnectionString = connectionString;
            Name = name;
            FilterableColumns = filterableColumns;
            VisibleColumns = visibleColumns;
        }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>()) { }

        public TableReadOnly(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>()) { }

        public string ConnectionString { get; protected set; }

        public string CatalogName { get; protected set; }

        public List<string> FilterableColumns { get; protected set; }

        public string Name { get; protected set; }

        public List<string> VisibleColumns { get; protected set; }

        protected virtual void OnGetAction(ActionGetEventArgs e) {
            OnActionGet?.Invoke(this, e);
        }

        public override bool Process(HttpListenerContext context, string rawUrl) {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper()) {
                case "GET":
                    result = ProcessGet(context);
                    break;
                case "HEAD":
                    result = ProcessHead(context);
                    break;
            }

            return result;
        }

        protected string GetQuery(NameValueCollection queryString) {
            string result = "SELECT " + (VisibleColumns.Any() ? string.Join(", ", VisibleColumns) : "*") + " FROM ";
            if (!string.IsNullOrEmpty(CatalogName)) {
                result += CatalogName + ".";
            }
            result += Name + " WHERE 1 = 1";
            foreach (string name in queryString) {
                if (!FilterableColumns.Any() || FilterableColumns.Contains(name)) {
                    Criteria criteria = null;
                    if (Criteria.TryParse(queryString[name], out criteria)) {
                        result += " AND " + criteria.toSqlWhereClause(name);
                    } else {
                        result += " AND " + name;
                        if (queryString[name].Contains("%")) {
                            result += " LIKE '" + queryString[name] + "'";
                        } else {
                            result += "=" + queryString[name].ToSqlValue();
                        }
                    }
                }
            }

            return result;
        }

        protected bool ProcessGet(HttpListenerContext context) {
            return ProcessGetSql(context, GetQuery(context.Request.QueryString));
        }

        protected virtual bool ProcessGetSql(HttpListenerContext context, string sqlQuery) {
            return true;
        }

        protected virtual bool ProcessHead(HttpListenerContext context) {
            return true;
        }
    }
}
