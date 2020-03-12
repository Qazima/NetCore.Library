using System.Collections.Generic;
using System.Linq;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure {
    public class PrimaryKey {
        public PrimaryKey() {
            Columns = new List<Column>();
        }

        public List<Column> Columns { get; set; }

        public string Name { get; set; }

        public override string ToString() {
            return "Primary key " + Name + "(" + string.Join(",", Columns.Select(c => c.Name)) + ")";
        }
    }
}
