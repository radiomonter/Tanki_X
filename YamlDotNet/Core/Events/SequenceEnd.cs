namespace YamlDotNet.Core.Events
{
    using System;
    using YamlDotNet.Core;

    public class SequenceEnd : ParsingEvent
    {
        public SequenceEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public SequenceEnd(Mark start, Mark end) : base(start, end)
        {
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => 
            "Sequence end";

        public override int NestingIncrease =>
            -1;

        internal override EventType Type =>
            EventType.SequenceEnd;
    }
}

