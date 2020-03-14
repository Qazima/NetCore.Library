using System.Collections.Specialized;

namespace Com.Qazima.NetCore.Library.Http.Action.Event
{
    public class ProcessEventArgs : GetEventArgs
    {
        public byte[] Content { get; set; }

        public NameValueCollection QueryString { get; set; }
    }
}
