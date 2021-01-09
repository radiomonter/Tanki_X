namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;

    internal class LiteralPatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            throw new InvalidOperationException("Should never get here because of the overridden Format method");
        }

        public override void Format(TextWriter writer, object state)
        {
            writer.Write(this.Option);
        }

        public override PatternConverter SetNext(PatternConverter pc)
        {
            LiteralPatternConverter converter = pc as LiteralPatternConverter;
            if (converter == null)
            {
                return base.SetNext(pc);
            }
            this.Option = this.Option + converter.Option;
            return this;
        }
    }
}

