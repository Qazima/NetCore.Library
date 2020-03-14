namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class DeleteEventArgs<ObjectType> : GetEventArgs {
        public ObjectType Old { get; set; }
    }
}
