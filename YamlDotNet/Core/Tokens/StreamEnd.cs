namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class StreamEnd : Token
    {
        public StreamEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public StreamEnd(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

