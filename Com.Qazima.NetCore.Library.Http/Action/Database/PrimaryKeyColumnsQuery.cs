namespace Com.Qazima.NetCore.Library.Http.Action.Database
{
    public struct PrimaryKeyColumnsQuery
    {
        public string Query { get; set; }

        public int IndexOfPrimaryKeySchemaName { get; set; }

        public int IndexOfTableSchemaName { get; set; }

        public int IndexOfPrimaryKeyName { get; set; }

        public int IndexOfTableName { get; set; }

        public int IndexOfName { get; set; }
    }
}
