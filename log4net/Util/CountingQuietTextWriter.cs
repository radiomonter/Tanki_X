namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.IO;

    public class CountingQuietTextWriter : QuietTextWriter
    {
        private long m_countBytes;

        public CountingQuietTextWriter(TextWriter writer, IErrorHandler errorHandler) : base(writer, errorHandler)
        {
            this.m_countBytes = 0L;
        }

        public override void Write(char value)
        {
            try
            {
                base.Write(value);
                char[] chars = new char[] { value };
                this.m_countBytes += this.Encoding.GetByteCount(chars);
            }
            catch (Exception exception)
            {
                base.ErrorHandler.Error("Failed to write [" + value + "].", exception, ErrorCode.WriteFailure);
            }
        }

        public override void Write(string str)
        {
            if ((str != null) && (str.Length > 0))
            {
                try
                {
                    base.Write(str);
                    this.m_countBytes += this.Encoding.GetByteCount(str);
                }
                catch (Exception exception)
                {
                    base.ErrorHandler.Error("Failed to write [" + str + "].", exception, ErrorCode.WriteFailure);
                }
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (count > 0)
            {
                try
                {
                    base.Write(buffer, index, count);
                    this.m_countBytes += this.Encoding.GetByteCount(buffer, index, count);
                }
                catch (Exception exception)
                {
                    base.ErrorHandler.Error("Failed to write buffer.", exception, ErrorCode.WriteFailure);
                }
            }
        }

        public long Count
        {
            get => 
                this.m_countBytes;
            set => 
                this.m_countBytes = value;
        }
    }
}

