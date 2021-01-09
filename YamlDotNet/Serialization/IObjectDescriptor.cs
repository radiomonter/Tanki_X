namespace YamlDotNet.Serialization
{
    using System;
    using YamlDotNet.Core;

    public interface IObjectDescriptor
    {
        object Value { get; }

        System.Type Type { get; }

        System.Type StaticType { get; }

        YamlDotNet.Core.ScalarStyle ScalarStyle { get; }
    }
}

