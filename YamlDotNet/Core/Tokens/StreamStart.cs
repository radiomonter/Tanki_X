namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class StreamStart : Token
    {
        public StreamStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public StreamStart(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

