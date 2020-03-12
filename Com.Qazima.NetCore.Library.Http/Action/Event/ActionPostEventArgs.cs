namespace Com.Qazima.NetCore.Library.Http.Action.Event {
    public class ActionPostEventArgs<ObjectType> : ActionGetEventArgs {
        public ObjectType New { get; set; }
    }
}
