namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class BlockMappingStart : Token
    {
        public BlockMappingStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public BlockMappingStart(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

