namespace YamlDotNet.Serialization.TypeInspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Serialization;

    public sealed class ReadableAndWritablePropertiesTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _innerTypeDescriptor;
        [CompilerGenerated]
        private static Func<IPropertyDescriptor, bool> <>f__am$cache0;

        public ReadableAndWritablePropertiesTypeInspector(ITypeInspector innerTypeDescriptor)
        {
            this._innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.CanWrite;
            }
            return this._innerTypeDescriptor.GetProperties(type, container).Where<IPropertyDescriptor>(<>f__am$cache0);
        }
    }
}

