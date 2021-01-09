namespace YamlDotNet.Serialization
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public sealed class YamlIgnoreAttribute : Attribute
    {
    }
}

