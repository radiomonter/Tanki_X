namespace YamlDotNet.Serialization.NamingConventions
{
    using System;
    using YamlDotNet.Serialization;

    public sealed class NullNamingConvention : INamingConvention
    {
        public string Apply(string value) => 
            value;
    }
}

