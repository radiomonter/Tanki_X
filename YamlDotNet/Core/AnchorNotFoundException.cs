namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AnchorNotFoundException : YamlException
    {
        public AnchorNotFoundException()
        {
        }

        public AnchorNotFoundException(string message) : base(message)
        {
        }

        protected AnchorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AnchorNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        public AnchorNotFoundException(Mark start, Mark end, string message) : base(start, end, message)
        {
        }
    }
}

