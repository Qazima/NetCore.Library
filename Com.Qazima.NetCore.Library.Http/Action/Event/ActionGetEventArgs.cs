using System;
using System.Net;

namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class ActionGetEventArgs : EventArgs {
        public Uri AskedUrl { get; set; }

        public DateTime AskedDate { get; set; }

        public DateTime EndDate { get; set; }

        public HttpStatusCode ResponseHttpStatusCode { get; set; }
    }
}
