namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class FlowSequenceEnd : Token
    {
        public FlowSequenceEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public FlowSequenceEnd(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

