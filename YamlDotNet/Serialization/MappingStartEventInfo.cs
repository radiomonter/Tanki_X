namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core.Events;

    public sealed class MappingStartEventInfo : ObjectEventInfo
    {
        public MappingStartEventInfo(IObjectDescriptor source) : base(source)
        {
        }

        public bool IsImplicit { get; set; }

        public MappingStyle Style { get; set; }
    }
}

