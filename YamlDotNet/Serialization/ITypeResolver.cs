namespace YamlDotNet.Serialization
{
    using System;

    public interface ITypeResolver
    {
        Type Resolve(Type staticType, object actualValue);
    }
}

