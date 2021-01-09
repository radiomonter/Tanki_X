namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class Scalar : Token
    {
        private readonly string value;
        private readonly ScalarStyle style;

        public Scalar(string value) : this(value, ScalarStyle.Any)
        {
        }

        public Scalar(string value, ScalarStyle style) : this(value, style, Mark.Empty, Mark.Empty)
        {
        }

        public Scalar(string value, ScalarStyle style, Mark start, Mark end) : base(start, end)
        {
            this.value = value;
            this.style = style;
        }

        public string Value =>
            this.value;

        public ScalarStyle Style =>
            this.style;
    }
}

