using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public interface ITable : ITableReadOnly {
        Dictionary<string, object> DefaultColumns { get; }
    }
}
