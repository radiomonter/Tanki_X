namespace YamlDotNet.Core.Events
{
    using System;
    using System.Globalization;
    using YamlDotNet.Core;

    public class AnchorAlias : ParsingEvent
    {
        private readonly string value;

        public AnchorAlias(string value) : this(value, Mark.Empty, Mark.Empty)
        {
        }

        public AnchorAlias(string value, Mark start, Mark end) : base(start, end)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new YamlException(start, end, "Anchor value must not be empty.");
            }
            if (!NodeEvent.anchorValidator.IsMatch(value))
            {
                throw new YamlException(start, end, "Anchor value must contain alphanumerical characters only.");
            }
            this.value = value;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.value };
            return string.Format(CultureInfo.InvariantCulture, "Alias [value = {0}]", args);
        }

        internal override EventType Type =>
            EventType.Alias;

        public string Value =>
            this.value;
    }
}

