namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolParameterOrderAttribute : Attribute
    {
        public ProtocolParameterOrderAttribute(int order)
        {
            this.Order = order;
        }

        public int Order { get; set; }
    }
}

