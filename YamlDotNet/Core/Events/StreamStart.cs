namespace YamlDotNet.Core.Events
{
    using System;
    using YamlDotNet.Core;

    public class StreamStart : ParsingEvent
    {
        public StreamStart() : this(Mark.Empty, Mark.Empty)
        {
        }

        public StreamStart(Mark start, Mark end) : base(start, end)
        {
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => 
            "Stream start";

        public override int NestingIncrease =>
            1;

        internal override EventType Type =>
            EventType.StreamStart;
    }
}

