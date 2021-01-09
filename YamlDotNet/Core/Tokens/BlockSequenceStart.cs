namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class BlockSequenceStart : Token
    {
        public BlockSequenceStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public BlockSequenceStart(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

