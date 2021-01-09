namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class FlowMappingEnd : Token
    {
        public FlowMappingEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public FlowMappingEnd(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

