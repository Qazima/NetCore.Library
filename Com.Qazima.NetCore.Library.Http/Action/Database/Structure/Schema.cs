using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure
{
    public class Schema
    {
        public Schema()
        {
            ForeignKeys = new List<ForeignKey>();
            PrimaryKeys = new List<PrimaryKey>();
            Tables = new List<Table>();
        }

        public List<ForeignKey> ForeignKeys { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public List<PrimaryKey> PrimaryKeys { get; set; }

        public List<Table> Tables { get; set; }

        public override string ToString()
        {
            return "Schema " + Name + ": " + Tables.Count + " table" + (Tables.Count > 1 ? "s" : "");
        }
    }
}
