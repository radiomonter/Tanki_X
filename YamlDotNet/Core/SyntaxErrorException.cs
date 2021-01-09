namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SyntaxErrorException : YamlException
    {
        public SyntaxErrorException()
        {
        }

        public SyntaxErrorException(string message) : base(message)
        {
        }

        protected SyntaxErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SyntaxErrorException(string message, Exception inner) : base(message, inner)
        {
        }

        public SyntaxErrorException(Mark start, Mark end, string message) : base(start, end, message)
        {
        }
    }
}

