using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.Qazima.NetCore.Library.Http.Action {
    public abstract class Generic : IAction {
        public Generic() {
            HttpStatusPages = new Dictionary<HttpStatusCode, string>();
        }

        public virtual bool Process(HttpListenerContext context, string rawUrl) {
            return true;
        }

        public Dictionary<HttpStatusCode, string> HttpStatusPages { get; }

        /// <summary>
        /// Process the 404 Error code
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>True if every thing was fine</returns>
        protected bool ProcessError(HttpListenerContext context, HttpStatusCode statusCode) {
            string filePath = HttpStatusPages[statusCode];
            byte[] buffer = File.ReadAllBytes(filePath);
            FileInfo fileInfo = new FileInfo(filePath);
            string contentType;
            if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType)) {
                contentType = "application/octet-stream";
            }
            DateTime creationTime = fileInfo.CreationTime;
            DateTime lastWriteTime = fileInfo.LastWriteTime;
            return ProcessError(context, statusCode, buffer, contentType, creationTime, lastWriteTime);
        }
        /// <summary>
        /// Process the 404 Error code
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>True if every thing was fine</returns>
        protected bool ProcessError(HttpListenerContext context, HttpStatusCode statusCode, byte[] buffer, string mimeType, DateTime creationTime, DateTime lastWriteTime) {
            bool result = true;
            //Adding permanent http response headers
            context.Response.ContentType = mimeType;
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
