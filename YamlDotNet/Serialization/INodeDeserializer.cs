namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;

    public interface INodeDeserializer
    {
        bool Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value);
    }
}

