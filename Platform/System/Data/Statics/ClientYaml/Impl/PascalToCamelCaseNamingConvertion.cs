namespace Platform.System.Data.Statics.ClientYaml.Impl
{
    using System;
    using YamlDotNet.Serialization;

    public class PascalToCamelCaseNamingConvertion : INamingConvention
    {
        public string Apply(string value) => 
            char.ToLower(value[0]) + value.Substring(1);
    }
}

