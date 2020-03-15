using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Json
{
    public class JsonReadOnly<ListObjectType, ObjectType> : Action, IAction where ListObjectType : IEnumerable<ObjectType>
    {
        public event EventHandler<GetEventArgs> OnGet;

        protected ListObjectType Item { get; set; }

        public JsonReadOnly(ListObjectType item)
        {
            Item = item;
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

        protected virtual void OnGetAction(GetEventArgs e)
        {
            OnGet?.Invoke(this, e);
        }

        protected bool ProcessGet(HttpListenerContext context)
        {
            GetEventArgs eventArgs = new GetEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try
            {
                PrepareResponse(context);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnGetAction(eventArgs);

            return result;
        }

        protected bool ProcessHead(HttpListenerContext context)
        {
            try
            {
                DateTime currDate = DateTime.Now;
                //Adding permanent http response headers
                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = 0;
                context.Response.AddHeader("Date", currDate.ToString("r"));
                context.Response.AddHeader("Last-Modified", currDate.ToString("r"));

                byte[] buffer = new byte[0];
                Buffer.BlockCopy(string.Empty.ToCharArray(), 0, buffer, 0, 0);
                context.Response.OutputStream.Write(buffer, 0, 0);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return false;
            }

            return true;
        }

        protected void PrepareResponse(HttpListenerContext context)
        {
            ProcessEventArgs eventArgs = new ProcessEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            IEnumerable<ObjectType> filteredItems = Item;
            List<string> keys = context.Request.QueryString.AllKeys.ToList();

            foreach (string key in keys)
            {
                PropertyInfo property = Item.First().GetType().GetProperty(key);
                if (property != null)
                {
                    filteredItems = filteredItems.Where(item => property.GetValue(item, null).ToString().Equals(Encoding.UTF8.GetString(Encoding.Default.GetBytes(context.Request.QueryString[key])), StringComparison.InvariantCultureIgnoreCase));
                }
            }

            string strItem = JsonSerializer.Serialize(filteredItems);
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
        }
    }
}
