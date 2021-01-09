namespace YamlDotNet.Core.Events
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    public class Comment : ParsingEvent
    {
        public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
        {
        }

        public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
        {
            this.Value = value;
            this.IsInline = isInline;
        }

        public override void Accept(IParsingEventVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Value { get; private set; }

        public bool IsInline { get; private set; }

        internal override EventType Type =>
            EventType.Comment;
    }
}

