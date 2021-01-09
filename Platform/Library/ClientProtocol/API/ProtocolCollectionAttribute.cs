namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolCollectionAttribute : Attribute
    {
        public ProtocolCollectionAttribute(bool optional, bool varied)
        {
            this.Optional = optional;
            this.Varied = varied;
        }

        public bool Optional { get; private set; }

        public bool Varied { get; private set; }
    }
}

