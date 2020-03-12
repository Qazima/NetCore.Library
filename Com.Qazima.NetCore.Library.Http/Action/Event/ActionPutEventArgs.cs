namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class ActionPutEventArgs<ObjectType> : ActionGetEventArgs {
        public ObjectType New { get; set; }

        public ObjectType Old { get; set; }
    }
}
