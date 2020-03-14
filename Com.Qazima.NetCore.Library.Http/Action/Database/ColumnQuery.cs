namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public struct ColumnQuery {
        public string Query { get; set; }

        public int IndexOfTableSchemaName { get; set; }

        public int IndexOfTableName { get; set; }

        public int IndexOfName { get; set; }

        public int IndexOfOwner { get; set; }

        public int IndexOfType { get; set; }
    }
}
