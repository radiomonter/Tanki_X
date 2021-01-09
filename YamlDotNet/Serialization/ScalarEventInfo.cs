namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    public sealed class ScalarEventInfo : ObjectEventInfo
    {
        public ScalarEventInfo(IObjectDescriptor source) : base(source)
        {
            this.Style = source.ScalarStyle;
        }

        public string RenderedValue { get; set; }

        public ScalarStyle Style { get; set; }

        public bool IsPlainImplicit { get; set; }

        public bool IsQuotedImplicit { get; set; }
    }
}

