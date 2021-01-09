namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class FlowEntry : Token
    {
        public FlowEntry() : this(Mark.Empty, Mark.Empty)
        {
        }

        public FlowEntry(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

