namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using YamlDotNet;
    using YamlDotNet.Serialization;

    public sealed class DefaultExclusiveObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private static readonly IEqualityComparer<object> _objectComparer = EqualityComparer<object>.Default;

        public DefaultExclusiveObjectGraphVisitor(IObjectGraphVisitor nextVisitor) : base(nextVisitor)
        {
        }

        public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value) => 
            !_objectComparer.Equals(value, GetDefault(value.Type)) && base.EnterMapping(key, value);

        public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value)
        {
            DefaultValueAttribute customAttribute = key.GetCustomAttribute<DefaultValueAttribute>();
            object y = (customAttribute == null) ? GetDefault(key.Type) : customAttribute.Value;
            return (!_objectComparer.Equals(value.Value, y) && base.EnterMapping(key, value));
        }

        private static object GetDefault(Type type) => 
            !type.IsValueType() ? null : Activator.CreateInstance(type);
    }
}

