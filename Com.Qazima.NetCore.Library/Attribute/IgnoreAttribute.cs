using System;

namespace Com.Qazima.NetCore.Library.Attribute
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class IgnoreAttribute : System.Attribute
    {
        public IgnoreAttribute(string positionalString) { }
    }
}
