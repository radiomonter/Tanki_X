namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class BlockEntry : Token
    {
        public BlockEntry() : this(Mark.Empty, Mark.Empty)
        {
        }

        public BlockEntry(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

