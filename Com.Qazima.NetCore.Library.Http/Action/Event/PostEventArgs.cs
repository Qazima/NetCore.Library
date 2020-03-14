namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class PostEventArgs<ObjectType> : GetEventArgs {
        public ObjectType New { get; set; }
    }
}
