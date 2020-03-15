namespace Com.Qazima.NetCore.Library.Http.Action.Event
{
    public class PutEventArgs<ObjectType> : GetEventArgs
    {
        public ObjectType New { get; set; }

        public ObjectType Old { get; set; }
    }
}
