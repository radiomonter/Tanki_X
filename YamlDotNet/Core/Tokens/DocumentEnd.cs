namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class DocumentEnd : Token
    {
        public DocumentEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public DocumentEnd(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

