using System.Collections.Generic;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure
{
    public class Table
    {
        public Table()
        {
            Columns = new List<Column>();
            ForeignKeys = new List<ForeignKey>();
            PrimaryKeys = new List<PrimaryKey>();
        }

        public List<Column> Columns { get; set; }

        public List<ForeignKey> ForeignKeys { get; set; }

        public List<PrimaryKey> PrimaryKeys { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return "Table " + Name + ": " + Columns.Count + " column" + (Columns.Count > 1 ? "s" : "");
        }
    }
}
