using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public interface ITableReadOnly : IAction {
        string ConnectionString { get; }

        string CatalogName { get; }

        string Name { get; }

        List<string> FilterableColumns { get; }

        List<string> VisibleColumns { get; }
    }
}
