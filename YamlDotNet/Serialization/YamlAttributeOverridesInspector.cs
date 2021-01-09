namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization.TypeInspectors;

    public sealed class YamlAttributeOverridesInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;
        private readonly YamlAttributeOverrides overrides;

        public YamlAttributeOverridesInspector(ITypeInspector innerTypeDescriptor, YamlAttributeOverrides overrides)
        {
            this.innerTypeDescriptor = innerTypeDescriptor;
            this.overrides = overrides;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            <GetProperties>c__AnonStorey0 storey = new <GetProperties>c__AnonStorey0 {
                type = type,
                $this = this
            };
            return ((this.overrides != null) ? this.innerTypeDescriptor.GetProperties(storey.type, container).Select<IPropertyDescriptor, IPropertyDescriptor>(new Func<IPropertyDescriptor, IPropertyDescriptor>(storey.<>m__0)) : this.innerTypeDescriptor.GetProperties(storey.type, container));
        }

        [CompilerGenerated]
        private sealed class <GetProperties>c__AnonStorey0
        {
            internal Type type;
            internal YamlAttributeOverridesInspector $this;

            internal IPropertyDescriptor <>m__0(IPropertyDescriptor p) => 
                new YamlAttributeOverridesInspector.OverridePropertyDescriptor(p, this.$this.overrides, this.type);
        }

        public sealed class OverridePropertyDescriptor : IPropertyDescriptor
        {
            private readonly IPropertyDescriptor baseDescriptor;
            private readonly YamlAttributeOverrides overrides;
            private readonly System.Type classType;

            public OverridePropertyDescriptor(IPropertyDescriptor baseDescriptor, YamlAttributeOverrides overrides, System.Type classType)
            {
                this.baseDescriptor = baseDescriptor;
                this.overrides = overrides;
                this.classType = classType;
            }

            public T GetCustomAttribute<T>() where T: Attribute
            {
                PropertyInfo publicProperty = this.classType.GetPublicProperty(this.Name);
                if (publicProperty != null)
                {
                    T attribute = this.overrides.GetAttribute<T>(publicProperty.DeclaringType, this.Name);
                    if (attribute != null)
                    {
                        return attribute;
                    }
                }
                return this.baseDescriptor.GetCustomAttribute<T>();
            }

            public IObjectDescriptor Read(object target) => 
                this.baseDescriptor.Read(target);

            public void Write(object target, object value)
            {
                this.baseDescriptor.Write(target, value);
            }

            public string Name =>
                this.baseDescriptor.Name;

            public bool CanWrite =>
                this.baseDescriptor.CanWrite;

            public System.Type Type =>
                this.baseDescriptor.Type;

            public System.Type TypeOverride
            {
                get => 
                    this.baseDescriptor.TypeOverride;
                set => 
                    this.baseDescriptor.TypeOverride = value;
            }

            public int Order
            {
                get => 
                    this.baseDescriptor.Order;
                set => 
                    this.baseDescriptor.Order = value;
            }

            public YamlDotNet.Core.ScalarStyle ScalarStyle
            {
                get => 
                    this.baseDescriptor.ScalarStyle;
                set => 
                    this.baseDescriptor.ScalarStyle = value;
            }
        }
    }
}

