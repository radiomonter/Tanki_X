namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class Key : Token
    {
        public Key() : this(Mark.Empty, Mark.Empty)
        {
        }

        public Key(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

