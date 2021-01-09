namespace YamlDotNet.Serialization.TypeInspectors
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet.Serialization;

    public sealed class CachedTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;
        private readonly Dictionary<Type, List<IPropertyDescriptor>> cache = new Dictionary<Type, List<IPropertyDescriptor>>();

        public CachedTypeInspector(ITypeInspector innerTypeDescriptor)
        {
            if (innerTypeDescriptor == null)
            {
                throw new ArgumentNullException("innerTypeDescriptor");
            }
            this.innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            List<IPropertyDescriptor> list;
            if (!this.cache.TryGetValue(type, out list))
            {
                list = new List<IPropertyDescriptor>(this.innerTypeDescriptor.GetProperties(type, container));
            }
            return list;
        }
    }
}

