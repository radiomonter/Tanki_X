namespace YamlDotNet.Serialization.TypeInspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    public sealed class ReadablePropertiesTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeResolver _typeResolver;
        [CompilerGenerated]
        private static Func<PropertyInfo, bool> <>f__mg$cache0;

        public ReadablePropertiesTypeInspector(ITypeResolver typeResolver)
        {
            if (typeResolver == null)
            {
                throw new ArgumentNullException("typeResolver");
            }
            this._typeResolver = typeResolver;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<PropertyInfo, bool>(ReadablePropertiesTypeInspector.IsValidProperty);
            }
            return (from p in type.GetPublicProperties().Where<PropertyInfo>(<>f__mg$cache0) select new ReflectionPropertyDescriptor(p, this._typeResolver));
        }

        private static bool IsValidProperty(PropertyInfo property) => 
            property.CanRead && (property.GetGetMethod().GetParameters().Length == 0);

        private sealed class ReflectionPropertyDescriptor : IPropertyDescriptor
        {
            private readonly PropertyInfo _propertyInfo;
            private readonly ITypeResolver _typeResolver;

            public ReflectionPropertyDescriptor(PropertyInfo propertyInfo, ITypeResolver typeResolver)
            {
                this._propertyInfo = propertyInfo;
                this._typeResolver = typeResolver;
                this.ScalarStyle = YamlDotNet.Core.ScalarStyle.Any;
            }

            public T GetCustomAttribute<T>() where T: Attribute => 
                this._propertyInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault<object>();

            public IObjectDescriptor Read(object target)
            {
                object obj2 = this._propertyInfo.ReadValue(target);
                return new ObjectDescriptor(obj2, this.TypeOverride ?? this._typeResolver.Resolve(this.Type, obj2), this.Type, this.ScalarStyle);
            }

            public void Write(object target, object value)
            {
                this._propertyInfo.SetValue(target, value, null);
            }

            public string Name =>
                this._propertyInfo.Name;

            public System.Type Type =>
                this._propertyInfo.PropertyType;

            public System.Type TypeOverride { get; set; }

            public int Order { get; set; }

            public bool CanWrite =>
                this._propertyInfo.CanWrite;

            public YamlDotNet.Core.ScalarStyle ScalarStyle { get; set; }
        }
    }
}

