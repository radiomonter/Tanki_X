namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolTransientAttribute : Attribute
    {
        public ProtocolTransientAttribute()
        {
            this.MinimalServerProtocolVersion = 0x7fffffff;
        }

        public ProtocolTransientAttribute(int minimalServerProtocolVersion)
        {
            this.MinimalServerProtocolVersion = minimalServerProtocolVersion;
        }

        public int MinimalServerProtocolVersion { get; set; }
    }
}

