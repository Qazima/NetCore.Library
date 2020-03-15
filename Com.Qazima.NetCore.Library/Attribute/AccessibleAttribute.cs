using System;

namespace Com.Qazima.NetCore.Library.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class AccessibleAttribute : System.Attribute
    {
        public AccessibleAttribute(bool accessible)
        {
            Accessible = accessible;
        }

        public bool Accessible { get; }
    }
}
