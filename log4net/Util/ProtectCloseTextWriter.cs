namespace log4net.Util
{
    using System;
    using System.IO;

    public class ProtectCloseTextWriter : TextWriterAdapter
    {
        public ProtectCloseTextWriter(TextWriter writer) : base(writer)
        {
        }

        public void Attach(TextWriter writer)
        {
            base.Writer = writer;
        }

        public override void Close()
        {
        }
    }
}

