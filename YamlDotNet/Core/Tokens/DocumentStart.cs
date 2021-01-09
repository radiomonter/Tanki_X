namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class DocumentStart : Token
    {
        public DocumentStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public DocumentStart(Mark start, Mark end) : base(start, end)
        {
        }
    }
}

