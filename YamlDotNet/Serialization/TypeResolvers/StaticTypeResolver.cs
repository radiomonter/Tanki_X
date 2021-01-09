namespace YamlDotNet.Serialization.TypeResolvers
{
    using System;
    using YamlDotNet.Serialization;

    public sealed class StaticTypeResolver : ITypeResolver
    {
        public Type Resolve(Type staticType, object actualValue) => 
            staticType;
    }
}

