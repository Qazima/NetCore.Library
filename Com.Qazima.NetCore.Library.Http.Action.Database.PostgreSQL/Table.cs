using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.PostgreSQL
{
    public class Table : Generic.Table
    {
        public Table(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) : this(connectionString, name, visibleColumns, filterableColumns, new Dictionary<string, object>()) { }

        public Table(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns, Dictionary<string, object> defaultColumns) : base(connectionString, name, visibleColumns, filterableColumns, defaultColumns) { }
    }
}
