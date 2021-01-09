namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Serialization.TypeInspectors;

    public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;
        [CompilerGenerated]
        private static Func<IPropertyDescriptor, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<IPropertyDescriptor, IPropertyDescriptor> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<IPropertyDescriptor, int> <>f__am$cache2;

        public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
        {
            this.innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.GetCustomAttribute<YamlIgnoreAttribute>() == null;
            }
            <>f__am$cache1 ??= delegate (IPropertyDescriptor p) {
                PropertyDescriptor descriptor = new PropertyDescriptor(p);
                YamlAliasAttribute customAttribute = p.GetCustomAttribute<YamlAliasAttribute>();
                if (customAttribute != null)
                {
                    descriptor.Name = customAttribute.Alias;
                }
                YamlMemberAttribute attribute2 = p.GetCustomAttribute<YamlMemberAttribute>();
                if (attribute2 != null)
                {
                    if (attribute2.SerializeAs != null)
                    {
                        descriptor.TypeOverride = attribute2.SerializeAs;
                    }
                    descriptor.Order = attribute2.Order;
                    descriptor.ScalarStyle = attribute2.ScalarStyle;
                    if (attribute2.Alias != null)
                    {
                        if (customAttribute != null)
                        {
                            throw new InvalidOperationException("Mixing YamlAlias(...) with YamlMember(Alias = ...) is an error. The YamlAlias attribute is obsolete and should be removed.");
                        }
                        descriptor.Name = attribute2.Alias;
                    }
                }
                return descriptor;
            };
            <>f__am$cache2 ??= p => p.Order;
            return this.innerTypeDescriptor.GetProperties(type, container).Where<IPropertyDescriptor>(<>f__am$cache0).Select<IPropertyDescriptor, IPropertyDescriptor>(<>f__am$cache1).OrderBy<IPropertyDescriptor, int>(<>f__am$cache2);
        }
    }
}

