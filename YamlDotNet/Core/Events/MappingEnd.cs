namespace YamlDotNet.Core.Events
{
    using System;
    using YamlDotNet.Core;

    public class MappingEnd : ParsingEvent
    {
        public MappingEnd() : this(Mark.Empty, Mark.Empty)
        {
        }

        public MappingEnd(Mark start, Mark end) : base(start, end)
        {
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => 
            "Mapping end";

        public override int NestingIncrease =>
            -1;

        internal override EventType Type =>
            EventType.MappingEnd;
    }
}

