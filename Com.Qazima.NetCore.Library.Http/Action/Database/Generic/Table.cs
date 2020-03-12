using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public class Table : TableReadOnly, ITable {
        public event EventHandler<ActionDeleteEventArgs<Dictionary<string, object>>> OnActionDelete;

        public event EventHandler<ActionPostEventArgs<Dictionary<string, object>>> OnActionPost;

        public event EventHandler<ActionPutEventArgs<Dictionary<string, object>>> OnActionPut;

        public Table(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) : base(connectionString, name, visibleColumns, filterableColumns) { }

        public Table(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>()) { }

        public Table(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>()) { }

        public Dictionary<string, object> DefaultColumns { get; protected set; }

        protected virtual void OnDeleteAction(ActionDeleteEventArgs<Dictionary<string, object>> e) {
            OnActionDelete?.Invoke(this, e);
        }

        protected virtual void OnPostAction(ActionPostEventArgs<Dictionary<string, object>> e) {
            OnActionPost?.Invoke(this, e);
        }

        protected virtual void OnPutAction(ActionPutEventArgs<Dictionary<string, object>> e) {
            OnActionPut?.Invoke(this, e);
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

        protected string PostQuery(NameValueCollection queryString) {
            string result = "UPDATE ";
            if (!string.IsNullOrEmpty(CatalogName)) {
                result += CatalogName + ".";
            }
            result += Name + " SET";

            return result;
        }

        protected bool ProcessPost(HttpListenerContext context) {
            ActionPostEventArgs<Dictionary<string, object>> eventArgs = new ActionPostEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPostAction(eventArgs);

            return result;
        }

        protected virtual bool ProcessPostSql(HttpListenerContext context, string sqlQuery) {
            return true;
        }

        protected bool ProcessPut(HttpListenerContext context) {
            ActionPutEventArgs<Dictionary<string, object>> eventArgs = new ActionPutEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPutAction(eventArgs);

            return result;
        }

        protected virtual bool ProcessPutSql(HttpListenerContext context, string sqlQuery) {
            return true;
        }

        protected bool ProcessDelete(HttpListenerContext context) {
            ActionDeleteEventArgs<Dictionary<string, object>> eventArgs = new ActionDeleteEventArgs<Dictionary<string, object>>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnDeleteAction(eventArgs);

            return result;
        }

        protected virtual bool ProcessDeleteSql(HttpListenerContext context, string sqlQuery) {
            return true;
        }
    }
}
