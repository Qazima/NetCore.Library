using System;

namespace Com.Qazima.NetCore.Library.Attribute {
    [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class MinimumLengthAttribute : System.Attribute, IAttribute {

        public MinimumLengthAttribute(int length) {
            Length = length;
        }

        public int Length { get; private set; }

        public bool IsValid(object value) {
            return true;
        }
    }
}
