using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Com.Qazima.NetCore.Library.Http {
    public class ServerPrefixe {
        public bool IsSecured { get; private set; }

        public int ListeningPort { get; private set; }

        public string ListeningUrl { get; private set; }

        public ServerPrefixe(string listeningUrl, int listeningPort = 80) {
            ListeningUrl = listeningUrl;
            ListeningPort = listeningPort;
            IsSecured = false;
        }
        /*
        public ServerPrefixe(string listeningUrl, int listeningPort = 80, bool isSecured = false) {
            ListeningUrl = listeningUrl;
            ListeningPort = listeningPort;
            IsSecured = isSecured;
        }
        */

        public string Prefixe {
            get {
                string result = "http";
                if (IsSecured) {
                    result += "s";
                }
                result += "://" + ListeningUrl;
                if ((!IsSecured && ListeningPort != 80) || (IsSecured && ListeningPort != 443)) {
                    result += ":" + ListeningPort.ToString();
                }
                result += "/";
                return result;
            }
        }
    }
}
