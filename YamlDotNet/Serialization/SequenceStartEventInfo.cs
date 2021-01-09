namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core.Events;

    public sealed class SequenceStartEventInfo : ObjectEventInfo
    {
        public SequenceStartEventInfo(IObjectDescriptor source) : base(source)
        {
        }

        public bool IsImplicit { get; set; }

        public SequenceStyle Style { get; set; }
    }
}

