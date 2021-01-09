namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public class AliasEventInfo : EventInfo
    {
        public AliasEventInfo(IObjectDescriptor source) : base(source)
        {
        }

        public string Alias { get; set; }
    }
}

