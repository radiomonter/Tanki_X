namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class FlowSequenceStart : Token
    {
        public FlowSequenceStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public FlowSequenceStart(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

