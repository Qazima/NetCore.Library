﻿using Com.Qazima.NetCore.Library.Http.Action.Event;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Com.Qazima.NetCore.Library.Http.Action.Directory
{
    public class Directory : Action
    {
        public bool AllowPost { get; set; }

        public bool AllowExplore { get; set; }

        public bool AllowDelete { get; set; }

        public bool AllowGet { get; set; }

        public bool AllowPut { get; set; }

        public string Path { get; set; }

        public List<string> HomePages { get; set; }

        public Directory(string path, List<string> homePages, bool allowPost = false, bool allowDelete = false, bool allowExplore = false, bool allowGet = true, bool allowPust = false)
        {
            Path = path;
            HomePages = homePages;
            AllowPost = allowPost;
            AllowDelete = allowDelete;
            AllowExplore = allowExplore;
            AllowGet = allowGet;
            AllowPut = allowPust;
        }

        public override bool Process(HttpListenerContext context, string rawUrl)
        {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    result = ProcessGet(context, rawUrl);
                    break;
                case "POST":
                    result = ProcessPost(context, rawUrl);
                    break;
                case "PUT":
                    result = ProcessPut(context, rawUrl);
                    break;
                case "DELETE":
                    result = ProcessDelete(context, rawUrl);
                    break;
            }

            return result;
        }

        public bool ProcessDelete(HttpListenerContext context, string rawUrl)
        {
            if (AllowDelete)
            {
                try
                {
                    return true;
                }
                catch (Exception e)
                {
                    return Process500(context, e);
                }
            }

            return false;
        }

        public bool ProcessGet(HttpListenerContext context, string rawUrl)
        {
            if (AllowGet)
            {
                try
                {
                    if (!rawUrl.Contains(".") && !rawUrl.EndsWith("/"))
                    {
                        rawUrl += "/";
                    }

                    if (rawUrl.EndsWith("/"))
                    {
                        rawUrl += HomePages.FirstOrDefault(item => File.Exists(System.IO.Path.Combine(Path, rawUrl[0..^1], item)));
                    }

                    if (!rawUrl.StartsWith("/"))
                    {
                        rawUrl = "/" + rawUrl;
                    }

                    if (!string.IsNullOrWhiteSpace(rawUrl))
                    {
                        string filePath = System.IO.Path.Combine(Path, rawUrl.Substring(1, rawUrl.Length - 1));
                        if (!File.Exists(filePath))
                        {
                            return Process404(context);
                        }
                        else
                        {
                            ProcessEventArgs eventArgs = new ProcessEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
                            FileInfo fileInfo = new FileInfo(filePath);
                            byte[] buffer = File.ReadAllBytes(filePath);
                            int bytesCount = buffer.Length;
                            //Adding permanent http response headers
                            string contentType;
                            if (!new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType))
                            {
                                contentType = "application/octet-stream";
                            }
                            context.Response.ContentType = contentType;
                            context.Response.ContentLength64 = bytesCount;
                            context.Response.AddHeader("Date", fileInfo.CreationTime.ToString("r"));
                            context.Response.AddHeader("Last-Modified", fileInfo.LastWriteTime.ToString("r"));

                            context.Response.OutputStream.Write(buffer, 0, bytesCount);
                            context.Response.OutputStream.Flush();
                            context.Response.OutputStream.Close();
                            eventArgs.Content = buffer;
                            eventArgs.EndDate = DateTime.Now;
                            OnProcessAction(eventArgs);
                        }
                    }
                    else
                    {
                        return Process404(context);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return Process500(context, e);
                }
            }

            return true;
        }

        public bool ProcessPost(HttpListenerContext context, string rawUrl)
        {
            if (AllowPost)
            {
                try
                {
                    return true;
                }
                catch (Exception e)
                {
                    return Process500(context, e);
                }
            }

            return false;
        }

        public bool ProcessPut(HttpListenerContext context, string rawUrl)
        {
            if (AllowPut)
            {
                try
                {
                    return true;
                }
                catch (Exception e)
                {
                    return Process500(context, e);
                }
            }

            return false;
        }
    }
}
