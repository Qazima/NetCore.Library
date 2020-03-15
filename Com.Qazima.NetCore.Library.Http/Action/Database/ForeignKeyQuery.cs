namespace Com.Qazima.NetCore.Library.Http.Action.Database
{
    public struct ForeignKeyQuery
    {
        public string Query { get; set; }

        public int IndexOfForeignKeySchemaName { get; set; }

        public int IndexOfReferencedConstraintSchemaName { get; set; }

        public int IndexOfReferencedConstraintName { get; set; }

        public int IndexOfTableSchemaName { get; set; }

        public int IndexOfTableName { get; set; }

        public int IndexOfName { get; set; }
    }
}
