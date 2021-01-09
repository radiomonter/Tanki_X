namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;

    public class MappingStart : NodeEvent
    {
        private readonly bool isImplicit;
        private readonly MappingStyle style;

        public MappingStart() : this(null, null, true, MappingStyle.Any, Mark.Empty, Mark.Empty)
        {
        }

        public MappingStart(string anchor, string tag, bool isImplicit, MappingStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
        {
        }

        public MappingStart(string anchor, string tag, bool isImplicit, MappingStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
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
            return string.Format(CultureInfo.InvariantCulture, "Mapping start [anchor = {0}, tag = {1}, isImplicit = {2}, style = {3}]", args);
        }

        public override int NestingIncrease =>
            1;

        internal override EventType Type =>
            EventType.MappingStart;

        public bool IsImplicit =>
            this.isImplicit;

        public override bool IsCanonical =>
            !this.isImplicit;

        public MappingStyle Style =>
            this.style;
    }
}

