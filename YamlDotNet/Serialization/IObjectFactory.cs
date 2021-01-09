namespace YamlDotNet.Serialization
{
    using System;

    public interface IObjectFactory
    {
        object Create(Type type);
    }
}

