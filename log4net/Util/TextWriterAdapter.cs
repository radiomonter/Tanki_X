namespace log4net.Util
{
    using System;
    using System.IO;
    using System.Text;

    public abstract class TextWriterAdapter : TextWriter
    {
        private TextWriter m_writer;

        protected TextWriterAdapter(TextWriter writer)
        {
            this.m_writer = writer;
        }

        public override void Close()
        {
            this.m_writer.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.m_writer.Dispose();
            }
        }

        public override void Flush()
        {
            this.m_writer.Flush();
        }

        public override void Write(char value)
        {
            this.m_writer.Write(value);
        }

        public override void Write(string value)
        {
            this.m_writer.Write(value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.m_writer.Write(buffer, index, count);
        }

        protected TextWriter Writer
        {
            get => 
                this.m_writer;
            set => 
                this.m_writer = value;
        }

        public override System.Text.Encoding Encoding =>
            this.m_writer.Encoding;

        public override IFormatProvider FormatProvider =>
            this.m_writer.FormatProvider;

        public override string NewLine
        {
            get => 
                this.m_writer.NewLine;
            set => 
                this.m_writer.NewLine = value;
        }
    }
}

