namespace YamlDotNet.Serialization
{
    using System;

    public interface IAliasProvider
    {
        string GetAlias(object target);
    }
}

