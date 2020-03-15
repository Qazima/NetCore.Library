using System.Collections.Generic;
using System.Linq;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure
{
    public class ForeignKey
    {
        public ForeignKey()
        {
            Columns = new List<Column>();
        }

        public List<Column> Columns { get; set; }

        public string Name { get; set; }

        public PrimaryKey ReferencedConstraint { get; set; }

        public override string ToString()
        {
            return "Foreign key " + Name + "(" + string.Join(",", Columns.Select(c => c.Name)) + ")";
        }
    }
}
