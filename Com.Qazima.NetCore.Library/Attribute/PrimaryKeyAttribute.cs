using System;

namespace Com.Qazima.NetCore.Library.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class PrimaryKeyAttribute : System.Attribute
    {
        public PrimaryKeyAttribute() { }

        public string Name { get; private set; }
    }
}
