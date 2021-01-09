namespace YamlDotNet.Core.Tokens
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    [Serializable]
    public class Comment : Token
    {
        public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
        {
        }

        public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
        {
            this.IsInline = isInline;
            this.Value = value;
        }

        public string Value { get; private set; }

        public bool IsInline { get; private set; }
    }
}

