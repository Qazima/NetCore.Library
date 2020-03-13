using Com.Qazima.NetCore.Library.Http.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Com.Qazima.NetCore.Library.Http {
    public class Server {
        private Dictionary<string, IAction> Actions { get; set; }

        private Thread ServerThread;

        private HttpListener Listener { get; set; }

        private List<ServerPrefixe> ServerPrefixes { get; set; }

        public Server(params ServerPrefixe[] serverPrefixes) {
            ServerPrefixes = new List<ServerPrefixe>();
            ServerPrefixes.AddRange(serverPrefixes.ToList());
        }

        public Server(string listeningUrl) : this(new ServerPrefixe(listeningUrl)) { }

        private void Initialize() {
            Actions = new Dictionary<string, IAction>();
            ServerThread = new Thread(Listen);
        }

        public void AddAction(string directory, IAction action) {
            if (!directory.EndsWith("/")) {
                directory += "/";
            }

            Actions.Add(directory, action);
        }

        public void Start() {
            ServerThread.Start();
        }

        public void Stop() {
            ServerThread.Abort();
            Listener.Stop();
        }

        private void Listen() {
            Listener = new HttpListener();
            foreach(ServerPrefixe serverPrefixe in ServerPrefixes) {
                Listener.Prefixes.Add(serverPrefixe.Prefixe);
            }

            Listener.Start();
            while (true) {
                try {
                    HttpListenerContext context = Listener.GetContext();
                    Process(context);
                } catch {
                }
            }
        }

        private void Process(HttpListenerContext context) {
            try {
                if (!ProcessRawUrl(context.Request.RawUrl, context)) {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.OutputStream.Close();
                }
            } catch (Exception) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
            }
        }

        private bool ProcessRawUrl(string path, HttpListenerContext context) {
            if (!path.EndsWith("/")) {
                path += "/";
            }

            if (Actions.ContainsKey(path)) {
                KeyValuePair<string, IAction> currentAction = Actions.First(item => item.Key == path);
                return currentAction.Value.Process(context, (context.Request.Url.LocalPath).Substring(currentAction.Key.Length - 1));
            } else {
                if (path.EndsWith("/")) {
                    path = path.Substring(0, path.Length - 1);
                }

                if (!string.IsNullOrWhiteSpace(path)) {
                    List<string> pathExplode = path.Split('/').ToList();
                    string parentPath = string.Join("/", pathExplode.Take(pathExplode.Count - 1));

                    if (!parentPath.EndsWith("/")) {
                        parentPath += "/";
                    }

                    if (!string.IsNullOrWhiteSpace(parentPath)) {
                        return ProcessRawUrl(parentPath, context);
                    }
                }
            }

            return false;
        }
    }
}
