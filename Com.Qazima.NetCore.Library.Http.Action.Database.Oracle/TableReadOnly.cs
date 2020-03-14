using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Oracle
{
    public class TableReadOnly : Generic.TableReadOnly
    {
        public TableReadOnly(string connectionString, string name) : this(connectionString, name, new List<string>(), new List<string>()) { }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns) : this(connectionString, name, visibleColumns, new List<string>()) { }

        public TableReadOnly(string connectionString, string name, List<string> visibleColumns, List<string> filterableColumns) : base(connectionString, name, visibleColumns, filterableColumns) { }

        protected override DbCommand GetCommand(DbConnection dbConnection)
        {
            DbCommand result = null;
            if (dbConnection is OracleConnection)
            {
                result = ((OracleConnection)dbConnection).CreateCommand();
            }
            return result;
        }

        protected override DbConnection GetConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }
    }
}
