namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateAnchorException : YamlException
    {
        public DuplicateAnchorException()
        {
        }

        public DuplicateAnchorException(string message) : base(message)
        {
        }

        protected DuplicateAnchorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DuplicateAnchorException(string message, Exception inner) : base(message, inner)
        {
        }

        public DuplicateAnchorException(Mark start, Mark end, string message) : base(start, end, message)
        {
        }
    }
}

