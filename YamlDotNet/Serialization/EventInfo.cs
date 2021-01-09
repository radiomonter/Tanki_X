namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class EventInfo
    {
        protected EventInfo(IObjectDescriptor source)
        {
            this.Source = source;
        }

        public IObjectDescriptor Source { get; private set; }
    }
}

