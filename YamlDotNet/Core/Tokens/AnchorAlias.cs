namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class AnchorAlias : Token
    {
        private readonly string value;

        public AnchorAlias(string value) : this(value, Mark.Empty, Mark.Empty)
        {
        }

        public AnchorAlias(string value, Mark start, Mark end) : base(start, end)
        {
            this.value = value;
        }

        public string Value =>
            this.value;
    }
}

