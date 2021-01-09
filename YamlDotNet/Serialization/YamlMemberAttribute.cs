namespace YamlDotNet.Serialization
{
    using System;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class YamlMemberAttribute : Attribute
    {
        public YamlMemberAttribute()
        {
            this.ScalarStyle = YamlDotNet.Core.ScalarStyle.Any;
        }

        public YamlMemberAttribute(Type serializeAs) : this()
        {
            this.SerializeAs = serializeAs;
        }

        public Type SerializeAs { get; set; }

        public int Order { get; set; }

        public string Alias { get; set; }

        public YamlDotNet.Core.ScalarStyle ScalarStyle { get; set; }
    }
}

