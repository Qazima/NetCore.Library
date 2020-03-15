using System.Net;

namespace Com.Qazima.NetCore.Library.Http.Action
{
    public interface IAction
    {
        bool Process(HttpListenerContext context, string rawUrl);

        bool StoreInCache { get; set; }
    }
}
