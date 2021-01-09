namespace Platform.Library.ClientProtocol.API
{
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SerialVersionUIDAttribute : Attribute
    {
        public readonly long value;

        public SerialVersionUIDAttribute(long value)
        {
            this.value = value;
        }
    }
}

