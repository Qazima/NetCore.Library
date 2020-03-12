namespace Com.Qazima.NetCore.Library.Http.Action.Database.Structure {
    public enum SourceType {
        Function,
        Procedure,
        Package
    }

    public class Source {
        public string Body { get; set; }

        public string Header { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public SourceType Type { get; set; }

        public override string ToString() {
            return Type.ToString() + " " + Name;
        }
    }
}
