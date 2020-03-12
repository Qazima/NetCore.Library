namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class ActionDeleteEventArgs<ObjectType> : ActionGetEventArgs {
        public ObjectType Old { get; set; }
    }
}
