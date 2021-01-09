namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public class ObjectEventInfo : EventInfo
    {
        protected ObjectEventInfo(IObjectDescriptor source) : base(source)
        {
        }

        public string Anchor { get; set; }

        public string Tag { get; set; }
    }
}

