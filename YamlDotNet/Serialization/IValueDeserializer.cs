namespace YamlDotNet.Serialization
{
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization.Utilities;

    public interface IValueDeserializer
    {
        object DeserializeValue(EventReader reader, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer);
    }
}

