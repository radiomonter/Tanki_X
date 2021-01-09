namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class YamlException : Exception
    {
        public YamlException()
        {
        }

        public YamlException(string message) : base(message)
        {
        }

        protected YamlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Start = (Mark) info.GetValue("Start", typeof(Mark));
            this.End = (Mark) info.GetValue("End", typeof(Mark));
        }

        public YamlException(string message, Exception inner) : base(message, inner)
        {
        }

        public YamlException(Mark start, Mark end, string message) : this(start, end, message, null)
        {
        }

        public YamlException(Mark start, Mark end, string message, Exception innerException) : base($"({start}) - ({end}): {message}", innerException)
        {
            this.Start = start;
            this.End = end;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Start", this.Start);
            info.AddValue("End", this.End);
        }

        public Mark Start { get; private set; }

        public Mark End { get; private set; }
    }
}

