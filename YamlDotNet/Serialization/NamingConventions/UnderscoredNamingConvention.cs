namespace YamlDotNet.Serialization.NamingConventions
{
    using System;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class UnderscoredNamingConvention : INamingConvention
    {
        public string Apply(string value) => 
            value.FromCamelCase("_");
    }
}

