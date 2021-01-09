namespace YamlDotNet.Core.Events
{
    using System;
    using YamlDotNet.Core;

    public class StreamEnd : ParsingEvent
    {
        public StreamEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public StreamEnd(Mark start, Mark end) : base(start, end)
        {
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => 
            "Stream end";

        public override int NestingIncrease =>
            -1;

        internal override EventType Type =>
            EventType.StreamEnd;
    }
}

