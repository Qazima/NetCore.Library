using System;

namespace Com.Qazima.NetCore.Library.Attribute {
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class IgnoreAttribute : System.Attribute {
        public IgnoreAttribute(string positionalString) {
        }
    }
}
