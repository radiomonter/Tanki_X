namespace YamlDotNet.Core.Events
{
    using System;
    using YamlDotNet.Core;

    public abstract class ParsingEvent
    {
        private readonly Mark start;
        private readonly Mark end;

        internal ParsingEvent(Mark start, Mark end)
        {
            this.start = start;
            this.end = end;
        }

        public abstract void Accept(IParsingEventVisitor visitor);

        public virtual int NestingIncrease =>
            0;

        internal abstract EventType Type { get; }

        public Mark Start =>
            this.start;

        public Mark End =>
            this.end;
    }
}

