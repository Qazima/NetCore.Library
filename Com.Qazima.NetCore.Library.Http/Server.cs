using Com.Qazima.NetCore.Library.Http.Action;
using Com.Qazima.NetCore.Library.Http.Cache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Com.Qazima.NetCore.Library.Http
{
    public class Server
    {
        private Dictionary<string, Action.Action> Actions { get; set; }

        private Thread ServerThread;

        private HttpListener Listener { get; set; }

        private List<ServerPrefixe> ServerPrefixes { get; set; }

        public Server(string listeningUrl) : this(false, 0, new ServerPrefixe(listeningUrl)) { }

        public Server(params ServerPrefixe[] serverPrefixes) : this(false, 0, serverPrefixes) { }

        public Server(bool cacheEnabled, int cacheDuration, string listeningUrl) : this(cacheEnabled, cacheDuration, new ServerPrefixe(listeningUrl)) { }

        public Server(bool cacheEnabled, int cacheDuration, params ServerPrefixe[] serverPrefixes)
        {
            Manager.Instance.Enabled = cacheEnabled;
            Manager.Instance.CacheDuration = cacheDuration;
            ServerPrefixes = new List<ServerPrefixe>();
            ServerPrefixes.AddRange(serverPrefixes.ToList());
            Actions = new Dictionary<string, Action.Action>();
            ServerThread = new Thread(Listen);
        }

        public void AddAction(string directory, Action.Action action)
        {
            if (!directory.EndsWith("/"))
            {
                directory += "/";
            }

            action.OnProcess += OnProcess;
            Actions.Add(directory, action);
        }

        private void OnProcess(object sender, Action.Event.ProcessEventArgs e)
        {
            Manager.Instance.AddAsync(e.AskedUrl.ToString(), e.QueryString, e.Content);
        }

        public void Start()
        {
            ServerThread.Start();
        }

        public void Stop()
        {
            ServerThread.Abort();
            Listener.Stop();
        }

        private void Listen()
        {
            Listener = new HttpListener();
            foreach (ServerPrefixe serverPrefixe in ServerPrefixes)
            {
                Listener.Prefixes.Add(serverPrefixe.Prefixe);
            }

            Listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = Listener.GetContext();
                    Process(context);
                }
                catch
                {
                }
            }
        }

        private void Process(HttpListenerContext context)
        {
            try
            {
                if (!ProcessRawUrl(context.Request.RawUrl, context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.OutputStream.Close();
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
            }
        }

        private bool ProcessRawUrl(string path, HttpListenerContext context)
        {
            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            if (Actions.ContainsKey(path))
            {
                KeyValuePair<string, Action.Action> currentAction = Actions.First(item => item.Key == path);
                return currentAction.Value.Process(context, (context.Request.Url.LocalPath).Substring(currentAction.Key.Length - 1));
            }
            else
            {
                if (path.EndsWith("/"))
                {
                    path = path.Substring(0, path.Length - 1);
                }

                if (!string.IsNullOrWhiteSpace(path))
                {
                    List<string> pathExplode = path.Split('/').ToList();
                    string parentPath = string.Join("/", pathExplode.Take(pathExplode.Count - 1));

                    if (!parentPath.EndsWith("/"))
                    {
                        parentPath += "/";
                    }

                    if (!string.IsNullOrWhiteSpace(parentPath))
                    {
                        return ProcessRawUrl(parentPath, context);
                    }
                }
            }

            return false;
        }
    }
}
