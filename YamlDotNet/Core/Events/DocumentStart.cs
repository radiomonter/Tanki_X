namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Tokens;

    public class DocumentStart : ParsingEvent
    {
        private readonly TagDirectiveCollection tags;
        private readonly VersionDirective version;
        private readonly bool isImplicit;

        public DocumentStart() : this(null, null, true, Mark.Empty, Mark.Empty)
        {
        }

        public DocumentStart(Mark start, Mark end) : this(null, null, true, start, end)
        {
        }

        public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit) : this(version, tags, isImplicit, Mark.Empty, Mark.Empty)
        {
        }

        public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit, Mark start, Mark end) : base(start, end)
        {
            this.version = version;
            this.tags = tags;
            this.isImplicit = isImplicit;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.isImplicit };
            return string.Format(CultureInfo.InvariantCulture, "Document start [isImplicit = {0}]", args);
        }

        public override int NestingIncrease =>
            1;

        internal override EventType Type =>
            EventType.DocumentStart;

        public TagDirectiveCollection Tags =>
            this.tags;

        public VersionDirective Version =>
            this.version;

        public bool IsImplicit =>
            this.isImplicit;
    }
}

