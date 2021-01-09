namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;

    public class Scalar : NodeEvent
    {
        private readonly string value;
        private readonly ScalarStyle style;
        private readonly bool isPlainImplicit;
        private readonly bool isQuotedImplicit;

        public Scalar(string value) : this(null, null, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty)
        {
        }

        public Scalar(string tag, string value) : this(null, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty)
        {
        }

        public Scalar(string anchor, string tag, string value) : this(anchor, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty)
        {
        }

        public Scalar(string anchor, string tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit) : this(anchor, tag, value, style, isPlainImplicit, isQuotedImplicit, Mark.Empty, Mark.Empty)
        {
        }

        public Scalar(string anchor, string tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit, Mark start, Mark end) : base(anchor, tag, start, end)
        {
            this.value = value;
            this.style = style;
            this.isPlainImplicit = isPlainImplicit;
            this.isQuotedImplicit = isQuotedImplicit;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            object[] args = new object[] { base.Anchor, base.Tag, this.value, this.style, this.isPlainImplicit, this.isQuotedImplicit };
            return string.Format(CultureInfo.InvariantCulture, "Scalar [anchor = {0}, tag = {1}, value = {2}, style = {3}, isPlainImplicit = {4}, isQuotedImplicit = {5}]", args);
        }

        internal override EventType Type =>
            EventType.Scalar;

        public string Value =>
            this.value;

        public ScalarStyle Style =>
            this.style;

        public bool IsPlainImplicit =>
            this.isPlainImplicit;

        public bool IsQuotedImplicit =>
            this.isQuotedImplicit;

        public override bool IsCanonical =>
            !this.isPlainImplicit && !this.isQuotedImplicit;
    }
}

