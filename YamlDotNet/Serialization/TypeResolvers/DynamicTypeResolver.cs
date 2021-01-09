namespace YamlDotNet.Serialization.TypeResolvers
{
    using System;
    using YamlDotNet.Serialization;

    public sealed class DynamicTypeResolver : ITypeResolver
    {
        public Type Resolve(Type staticType, object actualValue) => 
            (actualValue == null) ? staticType : actualValue.GetType();
    }
}

