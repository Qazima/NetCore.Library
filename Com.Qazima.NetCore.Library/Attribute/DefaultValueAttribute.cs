using System;

namespace Com.Qazima.NetCore.Library.Attribute {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class DefaultValueAttribute : System.Attribute{

        public DefaultValueAttribute(object value) {
            Value = value;
        }

        public object Value { get; private set; }
    }
}
