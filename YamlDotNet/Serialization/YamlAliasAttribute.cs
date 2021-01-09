namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    [Obsolete("Please use YamlMember instead"), AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class YamlAliasAttribute : Attribute
    {
        public YamlAliasAttribute(string alias)
        {
            this.Alias = alias;
        }

        public string Alias { get; set; }
    }
}

