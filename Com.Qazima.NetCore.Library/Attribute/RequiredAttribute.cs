using System;

namespace Com.Qazima.NetCore.Library.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class RequiredAttribute : System.Attribute, IAttribute
    {
        public RequiredAttribute() { }

        public bool IsValid(object value)
        {
            return true;
        }
    }
}
