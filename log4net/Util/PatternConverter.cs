namespace log4net.Util
{
    using log4net.Repository;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public abstract class PatternConverter
    {
        private static readonly string[] SPACES = new string[] { " ", "  ", "    ", "        ", "                ", "                                " };
        private PatternConverter m_next;
        private int m_min = -1;
        private int m_max = 0x7fffffff;
        private bool m_leftAlign;
        private string m_option;
        private ReusableStringWriter m_formatWriter = new ReusableStringWriter(CultureInfo.InvariantCulture);
        private const int c_renderBufferSize = 0x100;
        private const int c_renderBufferMaxCapacity = 0x400;
        private PropertiesDictionary properties;

        protected PatternConverter()
        {
        }

        protected abstract void Convert(TextWriter writer, object state);
        public virtual void Format(TextWriter writer, object state)
        {
            if ((this.m_min < 0) && (this.m_max == 0x7fffffff))
            {
                this.Convert(writer, state);
            }
            else
            {
                int length;
                string str = null;
                lock (this.m_formatWriter)
                {
                    this.m_formatWriter.Reset(0x400, 0x100);
                    this.Convert(this.m_formatWriter, state);
                    StringBuilder stringBuilder = this.m_formatWriter.GetStringBuilder();
                    length = stringBuilder.Length;
                    if (length <= this.m_max)
                    {
                        str = stringBuilder.ToString();
                    }
                    else
                    {
                        str = stringBuilder.ToString(length - this.m_max, this.m_max);
                        length = this.m_max;
                    }
                }
                if (length >= this.m_min)
                {
                    writer.Write(str);
                }
                else if (this.m_leftAlign)
                {
                    writer.Write(str);
                    SpacePad(writer, this.m_min - length);
                }
                else
                {
                    SpacePad(writer, this.m_min - length);
                    writer.Write(str);
                }
            }
        }

        public virtual PatternConverter SetNext(PatternConverter patternConverter)
        {
            this.m_next = patternConverter;
            return this.m_next;
        }

        protected static void SpacePad(TextWriter writer, int length)
        {
            while (length >= 0x20)
            {
                writer.Write(SPACES[5]);
                length -= 0x20;
            }
            for (int i = 4; i >= 0; i--)
            {
                if ((length & (1 << (i & 0x1f))) != 0)
                {
                    writer.Write(SPACES[i]);
                }
            }
        }

        protected static void WriteDictionary(TextWriter writer, ILoggerRepository repository, IDictionary value)
        {
            WriteDictionary(writer, repository, value.GetEnumerator());
        }

        protected static void WriteDictionary(TextWriter writer, ILoggerRepository repository, IDictionaryEnumerator value)
        {
            writer.Write("{");
            bool flag = true;
            while (value.MoveNext())
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    writer.Write(", ");
                }
                WriteObject(writer, repository, value.Key);
                writer.Write("=");
                WriteObject(writer, repository, value.Value);
            }
            writer.Write("}");
        }

        protected static void WriteObject(TextWriter writer, ILoggerRepository repository, object value)
        {
            if (repository != null)
            {
                repository.RendererMap.FindAndRender(value, writer);
            }
            else if (value == null)
            {
                writer.Write(SystemInfo.NullText);
            }
            else
            {
                writer.Write(value.ToString());
            }
        }

        public virtual PatternConverter Next =>
            this.m_next;

        public virtual log4net.Util.FormattingInfo FormattingInfo
        {
            get => 
                new log4net.Util.FormattingInfo(this.m_min, this.m_max, this.m_leftAlign);
            set
            {
                this.m_min = value.Min;
                this.m_max = value.Max;
                this.m_leftAlign = value.LeftAlign;
            }
        }

        public virtual string Option
        {
            get => 
                this.m_option;
            set => 
                this.m_option = value;
        }

        public PropertiesDictionary Properties
        {
            get => 
                this.properties;
            set => 
                this.properties = value;
        }
    }
}

