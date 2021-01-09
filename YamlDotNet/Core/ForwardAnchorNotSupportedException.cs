namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ForwardAnchorNotSupportedException : YamlException
    {
        public ForwardAnchorNotSupportedException()
        {
        }

        public ForwardAnchorNotSupportedException(string message) : base(message)
        {
        }

        protected ForwardAnchorNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ForwardAnchorNotSupportedException(string message, Exception inner) : base(message, inner)
        {
        }

        public ForwardAnchorNotSupportedException(Mark start, Mark end, string message) : base(start, end, message)
        {
        }
    }
}

