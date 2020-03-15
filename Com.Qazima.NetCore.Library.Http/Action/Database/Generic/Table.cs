using Com.Qazima.NetCore.Library.Http.Action.Database.Helper;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Generic
{
    public class Table : TableReadOnly
    {
        public event EventHandler<DeleteEventArgs<Dictionary<string, object>>> OnDelete;

        public event EventHandler<PostEventArgs<Dictionary<string, object>>> OnPost;

        public event EventHandler<PutEventArgs<Dictionary<string, object>>> OnPut;

        public bool AllowPost { get; set; }

        public bool AllowDelete { get; set; }

        public bool AllowPut { get; set; }

        public Table(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) : this(connectionString, name, visibleColumns, filterableColumns, new Dictionary<string, object>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns, Dictionary<string, object> defaultColumns) : base(connectionString, name, visibleColumns, filterableColumns)
        {
            DefaultColumns = defaultColumns;
        }

        public Dictionary<string, object> DefaultColumns { get; protected set; }

        protected void OnDeleteAction(DeleteEventArgs<Dictionary<string, object>> e)
        {
            OnDelete?.Invoke(this, e);
        }

        protected void OnPostAction(PostEventArgs<Dictionary<string, object>> e)
        {
            OnPost?.Invoke(this, e);
        }

        protected void OnPutAction(PutEventArgs<Dictionary<string, object>> e)
        {
            OnPut?.Invoke(this, e);
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
                case "POST":
                    result = ProcessPost(context);
                    break;
                case "PUT":
                    result = ProcessPut(context);
                    break;
                case "DELETE":
                    result = ProcessDelete(context);
                    break;
            }

            return result;
        }

        protected string PostQuery(NameValueCollection queryString)
        {
            string result = "INSERT INTO ";
            if (!string.IsNullOrEmpty(CatalogName))
            {
                result += CatalogName + ".";
            }
            result += Name + " (";
            string values = " VALUES (";

            foreach (string name in queryString)
            {
                if (!VisibleColumns.Any() || VisibleColumns.Contains(name))
                {
                    result += name + ", ";
                    values += queryString[name].ToSqlValue() + ", ";
                }
            }

            result += string.Join(", ", DefaultColumns.Keys) + ")" + values + string.Join(", ", DefaultColumns.Select(c => c.Value.ToSqlValue())) + ")";
            return result;
        }

        protected bool ProcessPost(HttpListenerContext context)
        {
            PostEventArgs<Dictionary<string, object>> eventArgs = new PostEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result;
            if (AllowPost)
            {
                result = ProcessPostSql(context, PostQuery(context.Request.QueryString));
            }
            else
            {
                result = Process403(context);
            }
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPostAction(eventArgs);
            return result;
        }

        protected bool ProcessPostSql(HttpListenerContext context, string commandText)
        {
            bool result = true;
            string strItem = " ";

            using (DbConnection conn = GetConnection(ConnectionString))
            {
                conn.Open();
                using DbCommand cmd = GetCommand(conn);
                cmd.CommandText = commandText;
                strItem = cmd.ExecuteNonQuery().ToString();
                conn.Close();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(strItem);
            int bytesCount = buffer.Length;
            DateTime currDate = DateTime.Now;
            //Adding permanent http response headers
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = bytesCount;
            context.Response.AddHeader("Date", currDate.ToString("r"));
            context.Response.AddHeader("Last-Modified", currDate.ToString("r"));

            context.Response.OutputStream.Write(buffer, 0, bytesCount);
            context.Response.OutputStream.Flush();

            return result;
        }

        protected string PutQuery(NameValueCollection queryString)
        {
            string result = "UPDATE ";
            if (!string.IsNullOrEmpty(CatalogName))
            {
                result += CatalogName + ".";
            }
            result += Name + " SET";
            string predicat = " WHERE 1=1";

            foreach (string name in queryString)
            {
                if (!FilterableColumns.Any() || FilterableColumns.Contains(name))
                {
                    if (Criteria.TryParse(queryString[name], out Criteria criteria))
                    {
                        predicat += " AND " + criteria.toSqlWhereClause(name);
                    }
                    else
                    {
                        predicat += " AND " + name;
                        if (queryString[name].Contains("%"))
                        {
                            predicat += " LIKE '" + queryString[name] + "'";
                        }
                        else
                        {
                            predicat += "=" + queryString[name].ToSqlValue();
                        }
                    }
                }
                else
                {
                    if (!VisibleColumns.Any() || VisibleColumns.Contains(name))
                    {
                        result += name + "=" + queryString[name].ToSqlValue() + ", ";
                    }
                }
            }

            result += string.Join(", ", DefaultColumns.Select(c => c.Key + "=" + c.Value.ToSqlValue())) + predicat;
            return result;
        }

        protected bool ProcessPut(HttpListenerContext context)
        {
            PutEventArgs<Dictionary<string, object>> eventArgs = new PutEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result;
            if (AllowPut)
            {
                result = ProcessPutSql(context, PostQuery(context.Request.QueryString));
            }
            else
            {
                result = Process403(context);
            }
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPutAction(eventArgs);
            return result;
        }

        protected bool ProcessPutSql(HttpListenerContext context, string commandText)
        {
            bool result = true;
            string strItem = " ";

            using (DbConnection conn = GetConnection(ConnectionString))
            {
                conn.Open();
                using DbCommand cmd = GetCommand(conn);
                cmd.CommandText = commandText;
                strItem = cmd.ExecuteNonQuery().ToString();
                conn.Close();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(strItem);
            int bytesCount = buffer.Length;
            DateTime currDate = DateTime.Now;
            //Adding permanent http response headers
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = bytesCount;
            context.Response.AddHeader("Date", currDate.ToString("r"));
            context.Response.AddHeader("Last-Modified", currDate.ToString("r"));

            context.Response.OutputStream.Write(buffer, 0, bytesCount);
            context.Response.OutputStream.Flush();

            return result;
        }

        protected string DeleteQuery(NameValueCollection queryString)
        {
            string result = "DELETE ";
            if (!string.IsNullOrEmpty(CatalogName))
            {
                result += CatalogName + ".";
            }
            result += Name + " WHERE 1=1";

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

            return result;
        }

        protected bool ProcessDelete(HttpListenerContext context)
        {
            DeleteEventArgs<Dictionary<string, object>> eventArgs = new DeleteEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result;
            if (AllowDelete)
            {
                result = ProcessDeleteSql(context, PostQuery(context.Request.QueryString));
            }
            else
            {
                result = Process403(context);
            }
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnDeleteAction(eventArgs);
            return result;
        }

        protected virtual bool ProcessDeleteSql(HttpListenerContext context, string commandText)
        {
            bool result = true;
            string strItem = " ";

            using (DbConnection conn = GetConnection(ConnectionString))
            {
                conn.Open();
                using DbCommand cmd = GetCommand(conn);
                cmd.CommandText = commandText;
                strItem = cmd.ExecuteNonQuery().ToString();
                conn.Close();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(strItem);
            int bytesCount = buffer.Length;
            DateTime currDate = DateTime.Now;
            //Adding permanent http response headers
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = bytesCount;
            context.Response.AddHeader("Date", currDate.ToString("r"));
            context.Response.AddHeader("Last-Modified", currDate.ToString("r"));

            context.Response.OutputStream.Write(buffer, 0, bytesCount);
            context.Response.OutputStream.Flush();

            return result;
        }
    }
}
