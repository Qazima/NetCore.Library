namespace Com.Qazima.NetCore.Library.Http.Action.Database
{
    public struct SchemaQuery
    {
        public string Query { get; set; }

        public int IndexOfName { get; set; }

        public int IndexOfOwner { get; set; }
    }
}
