using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.Qazima.NetCore.Library.Http.Action {
    public abstract class Action : IAction {
        public Action() {
            HttpStatusPages = new Dictionary<HttpStatusCode, string>();
        }

        public virtual bool Process(HttpListenerContext context, string rawUrl) {
            return true;
        }

        public Dictionary<HttpStatusCode, string> HttpStatusPages { get; }

        protected bool ProcessError(HttpListenerContext context, HttpStatusCode statusCode) {
            byte[] buffer;
            DateTime creationTime;
            DateTime lastWriteTime;
            string contentType;
            if (HttpStatusPages.ContainsKey(statusCode)) {
                string filePath = HttpStatusPages[statusCode];
                buffer = File.ReadAllBytes(filePath);
                FileInfo fileInfo = new FileInfo(filePath);
                creationTime = fileInfo.CreationTime;
                lastWriteTime = fileInfo.LastWriteTime;
                if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType)) {
                    contentType = "application/octet-stream";
                }
            } else {
                buffer = Encoding.UTF8.GetBytes(" ");
                DateTime currDate = DateTime.Now;
                creationTime = currDate;
                lastWriteTime = currDate;
                contentType = "application/octet-stream";
            }
            return ProcessError(context, statusCode, buffer, contentType, creationTime, lastWriteTime);
        }

        protected bool ProcessError(HttpListenerContext context, HttpStatusCode statusCode, byte[] buffer, string contentType, DateTime creationTime, DateTime lastWriteTime) {
            bool result = true;
            //Adding permanent http response headers
            context.Response.ContentType = contentType;
            int bytesCount = buffer.Length;
            //Adding permanent http response headers
            context.Response.ContentLength64 = bytesCount;
            context.Response.StatusCode = (int)statusCode;
            context.Response.AddHeader("Date", creationTime.ToString("r"));
            context.Response.AddHeader("Last-Modified", lastWriteTime.ToString("r"));

            context.Response.OutputStream.Write(buffer, 0, bytesCount);
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Close();
            return result;
        }
    }
}
