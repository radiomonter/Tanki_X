namespace YamlDotNet.Serialization
{
    using System;
    using YamlDotNet.Core;

    public interface IPropertyDescriptor
    {
        T GetCustomAttribute<T>() where T: Attribute;
        IObjectDescriptor Read(object target);
        void Write(object target, object value);

        string Name { get; }

        bool CanWrite { get; }

        System.Type Type { get; }

        System.Type TypeOverride { get; set; }

        int Order { get; set; }

        YamlDotNet.Core.ScalarStyle ScalarStyle { get; set; }
    }
}

