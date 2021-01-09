namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;

    public class SequenceStart : NodeEvent
    {
        private readonly bool isImplicit;
        private readonly SequenceStyle style;

        public SequenceStart(string anchor, string tag, bool isImplicit, SequenceStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
        {
        }

        public SequenceStart(string anchor, string tag, bool isImplicit, SequenceStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
        {
            this.isImplicit = isImplicit;
            this.style = style;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            object[] args = new object[] { base.Anchor, base.Tag, this.isImplicit, this.style };
            return string.Format(CultureInfo.InvariantCulture, "Sequence start [anchor = {0}, tag = {1}, isImplicit = {2}, style = {3}]", args);
        }

        public override int NestingIncrease =>
            1;

        internal override EventType Type =>
            EventType.SequenceStart;

        public bool IsImplicit =>
            this.isImplicit;

        public override bool IsCanonical =>
            !this.isImplicit;

        public SequenceStyle Style =>
            this.style;
    }
}

