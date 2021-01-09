namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SemanticErrorException : YamlException
    {
        public SemanticErrorException()
        {
        }

        public SemanticErrorException(string message) : base(message)
        {
        }

        protected SemanticErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SemanticErrorException(string message, Exception inner) : base(message, inner)
        {
        }

        public SemanticErrorException(Mark start, Mark end, string message) : base(start, end, message)
        {
        }
    }
}

