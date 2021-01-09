namespace YamlDotNet.Serialization
{
    using System;

    public interface INamingConvention
    {
        string Apply(string value);
    }
}

