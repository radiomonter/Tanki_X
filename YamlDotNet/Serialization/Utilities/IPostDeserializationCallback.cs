namespace YamlDotNet.Serialization.Utilities
{
    using System;

    public interface IPostDeserializationCallback
    {
        void OnDeserialization();
    }
}

