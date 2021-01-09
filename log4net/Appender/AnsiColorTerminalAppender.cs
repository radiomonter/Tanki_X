namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.Text;

    public class AnsiColorTerminalAppender : AppenderSkeleton
    {
        public const string ConsoleOut = "Console.Out";
        public const string ConsoleError = "Console.Error";
        private bool m_writeToErrorStream;
        private LevelMapping m_levelMapping = new LevelMapping();
        private const string PostEventCodes = "\x001b[0m";

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
        }

        public void AddMapping(LevelColors mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            string str = base.RenderLoggingEvent(loggingEvent);
            LevelColors colors = this.m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
            if (colors != null)
            {
                str = colors.CombinedColor + str;
            }
            str = (str.Length <= 1) ? (((str[0] == '\n') || (str[0] == '\r')) ? ("\x001b[0m" + str) : (str + "\x001b[0m")) : ((str.EndsWith("\r\n") || str.EndsWith("\n\r")) ? str.Insert(str.Length - 2, "\x001b[0m") : ((str.EndsWith("\n") || str.EndsWith("\r")) ? str.Insert(str.Length - 1, "\x001b[0m") : (str + "\x001b[0m")));
            if (this.m_writeToErrorStream)
            {
                Console.Error.Write(str);
            }
            else
            {
                Console.Write(str);
            }
        }

        public virtual string Target
        {
            get => 
                !this.m_writeToErrorStream ? "Console.Out" : "Console.Error";
            set
            {
                string strB = value.Trim();
                this.m_writeToErrorStream = string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0;
            }
        }

        protected override bool RequiresLayout =>
            true;

        [Flags]
        public enum AnsiAttributes
        {
            Bright = 1,
            Dim = 2,
            Underscore = 4,
            Blink = 8,
            Reverse = 0x10,
            Hidden = 0x20,
            Strikethrough = 0x40,
            Light = 0x80
        }

        public enum AnsiColor
        {
            Black,
            Red,
            Green,
            Yellow,
            Blue,
            Magenta,
            Cyan,
            White
        }

        public class LevelColors : LevelMappingEntry
        {
            private AnsiColorTerminalAppender.AnsiColor m_foreColor;
            private AnsiColorTerminalAppender.AnsiColor m_backColor;
            private AnsiColorTerminalAppender.AnsiAttributes m_attributes;
            private string m_combinedColor = string.Empty;

            public override void ActivateOptions()
            {
                base.ActivateOptions();
                StringBuilder builder = new StringBuilder();
                builder.Append("\x001b[0;");
                int num = ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Light) <= 0) ? 0 : 60;
                builder.Append((int) ((30 + num) + this.m_foreColor));
                builder.Append(';');
                builder.Append((int) ((40 + num) + this.m_backColor));
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Bright) > 0)
                {
                    builder.Append(";1");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Dim) > 0)
                {
                    builder.Append(";2");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Underscore) > 0)
                {
                    builder.Append(";4");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Blink) > 0)
                {
                    builder.Append(";5");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Reverse) > 0)
                {
                    builder.Append(";7");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Hidden) > 0)
                {
                    builder.Append(";8");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Strikethrough) > 0)
                {
                    builder.Append(";9");
                }
                builder.Append('m');
                this.m_combinedColor = builder.ToString();
            }

            public AnsiColorTerminalAppender.AnsiColor ForeColor
            {
                get => 
                    this.m_foreColor;
                set => 
                    this.m_foreColor = value;
            }

            public AnsiColorTerminalAppender.AnsiColor BackColor
            {
                get => 
                    this.m_backColor;
                set => 
                    this.m_backColor = value;
            }

            public AnsiColorTerminalAppender.AnsiAttributes Attributes
            {
                get => 
                    this.m_attributes;
                set => 
                    this.m_attributes = value;
            }

            internal string CombinedColor =>
                this.m_combinedColor;
        }
    }
}

