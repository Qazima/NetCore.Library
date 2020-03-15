using Com.Qazima.NetCore.Library.Http.Action.Database.Helper;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Generic
{
    public class TableReadOnly : Action
    {
        public event EventHandler<GetEventArgs> OnGet;

        public bool AllowGet { get; set; }

        public TableReadOnly(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>(), new Dictionary<string, OrderType>()) { }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>(), new Dictionary<string, OrderType>()) { }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) : this(connectionString, name, visibleColumns, filterableColumns, new Dictionary<string, OrderType>()) { }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns, Dictionary<string, OrderType> orderColumns)
        {
            ConnectionString = connectionString;
            Name = name;
            FilterableColumns = filterableColumns;
            OrderColumns = orderColumns;
            VisibleColumns = visibleColumns;
        }

        public string ConnectionString { get; protected set; }

        public string CatalogName { get; protected set; }

        public List<string> FilterableColumns { get; protected set; }

        public string Name { get; protected set; }

        public Dictionary<string, OrderType> OrderColumns { get; protected set; }

        public List<string> VisibleColumns { get; protected set; }

        protected virtual void OnGetAction(GetEventArgs e)
        {
            OnGet?.Invoke(this, e);
        }

        public override bool Process(HttpListenerContext context, string rawUrl)
        {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    result = ProcessGet(context);
                    break;
                case "HEAD":
                    result = ProcessHead(context);
                    break;
            }

            return result;
        }

        protected string GetQuery(NameValueCollection queryString)
        {
            string result = "SELECT " + (VisibleColumns.Any() ? string.Join(", ", VisibleColumns) : "*") + " FROM ";
            if (!string.IsNullOrEmpty(CatalogName))
            {
                result += CatalogName + ".";
            }
            result += Name + " WHERE 1 = 1";
            foreach (string name in queryString)
            {
                if (!FilterableColumns.Any() || FilterableColumns.Contains(name))
                {
                    if (Criteria.TryParse(queryString[name], out Criteria criteria))
                    {
                        result += " AND " + criteria.toSqlWhereClause(name);
                    }
                    else
                    {
                        result += " AND " + name;
                        if (queryString[name].Contains("%"))
                        {
                            result += " LIKE '" + queryString[name] + "'";
                        }
                        else
                        {
                            result += "=" + queryString[name].ToSqlValue();
                        }
                    }
                }
            }
            if (OrderColumns.Any())
            {
                result += " ORDER BY " + string.Join(", ", OrderColumns.Select(c => c.Key + (c.Value == OrderType.Ascending ? " ASC" : " DESC")));
            }

            return result;
        }

        protected bool ProcessGet(HttpListenerContext context)
        {
            GetEventArgs eventArgs = new GetEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };

            bool result;
            if (AllowGet)
            {
                result = ProcessGetSql(context, GetQuery(context.Request.QueryString));
            }
            else
            {
                result = Process403(context);
            }
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnGetAction(eventArgs);
            return result;
        }

        protected bool ProcessGetSql(HttpListenerContext context, string commandText)
        {
            ProcessEventArgs eventArgs = new ProcessEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            string strItem = " ";

            using (DbConnection conn = GetConnection(ConnectionString))
            {
                conn.Open();
                using DbCommand cmd = GetCommand(conn);
                cmd.CommandText = commandText;
                using DbDataReader reader = cmd.ExecuteReader();
                strItem = JsonSerializer.Serialize(reader.DbDataReader2JSON());
                conn.Close();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(strItem);
            int bytesCount = buffer.Length;
            DateTime currDate = DateTime.Now;
            //Adding permanent http response headers
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = bytesCount;
            context.Response.AddHeader("Date", currDate.ToString("r"));
            context.Response.AddHeader("Last-Modified", currDate.ToString("r"));

            context.Response.OutputStream.Write(buffer, 0, bytesCount);
            context.Response.OutputStream.Flush();
            eventArgs.Content = buffer;
            eventArgs.EndDate = DateTime.Now;
            OnProcessAction(eventArgs);

            return result;
        }

        protected bool ProcessHead(HttpListenerContext context)
        {
            return true;
        }

        protected virtual DbCommand GetCommand(DbConnection dbConnection)
        {
            return null;
        }

        protected virtual DbConnection GetConnection(string connectionString)
        {
            return null;
        }
    }
}
