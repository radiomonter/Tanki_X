namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class Tag : Token
    {
        private readonly string handle;
        private readonly string suffix;

        public Tag(string handle, string suffix) : this(handle, suffix, Mark.Empty, Mark.Empty)
        {
        }

        public Tag(string handle, string suffix, Mark start, Mark end) : base(start, end)
        {
            this.handle = handle;
            this.suffix = suffix;
        }

        public string Handle =>
            this.handle;

        public string Suffix =>
            this.suffix;
    }
}

