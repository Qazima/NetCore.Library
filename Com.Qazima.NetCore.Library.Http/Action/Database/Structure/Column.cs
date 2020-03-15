namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure
{
    public class Column
    {
        public Column()
        {
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return "Column " + Name + "(" + Type + ")";
        }
    }
}
