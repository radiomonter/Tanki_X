namespace log4net.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LogException : ApplicationException
    {
        public LogException()
        {
        }

        public LogException(string message) : base(message)
        {
        }

        protected LogException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LogException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

