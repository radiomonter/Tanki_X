namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;

    public class DocumentEnd : ParsingEvent
    {
        private readonly bool isImplicit;

        public DocumentEnd(bool isImplicit) : this(isImplicit, Mark.Empty, Mark.Empty)
        {
        }

        public DocumentEnd(bool isImplicit, Mark start, Mark end) : base(start, end)
        {
            this.isImplicit = isImplicit;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.isImplicit };
            return string.Format(CultureInfo.InvariantCulture, "Document end [isImplicit = {0}]", args);
        }

        public override int NestingIncrease =>
            -1;

        internal override EventType Type =>
            EventType.DocumentEnd;

        public bool IsImplicit =>
            this.isImplicit;
    }
}

