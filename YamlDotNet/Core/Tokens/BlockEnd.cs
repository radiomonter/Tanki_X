namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class BlockEnd : Token
    {
        public BlockEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public BlockEnd(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

