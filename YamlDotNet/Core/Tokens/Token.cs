namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public abstract class Token
    {
        private readonly Mark start;
        private readonly Mark end;

        protected Token(Mark start, Mark end)
        {
            this.start = start;
            this.end = end;
        }

        public Mark Start =>
            this.start;

        public Mark End =>
            this.end;
    }
}

