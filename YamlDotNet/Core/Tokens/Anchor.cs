namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class Anchor : Token
    {
        private readonly string value;

        public Anchor(string value) : this(value, Mark.Empty, Mark.Empty)
        {
        }

        public Anchor(string value, Mark start, Mark end) : base(start, end)
        {
            this.value = value;
        }

        public string Value =>
            this.value;
    }
}

