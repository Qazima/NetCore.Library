using System;

namespace Com.Qazima.NetCore.Library.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class ReferencesAttribute : System.Attribute
    {
        public ReferencesAttribute(Type referencedType)
        {
            ReferencedType = referencedType;
        }

        public Type ReferencedType { get; private set; }
    }
}
