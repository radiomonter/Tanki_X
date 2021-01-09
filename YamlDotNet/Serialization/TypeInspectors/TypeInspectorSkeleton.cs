namespace YamlDotNet.Serialization.TypeInspectors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using YamlDotNet.Serialization;

    public abstract class TypeInspectorSkeleton : ITypeInspector
    {
        [CompilerGenerated]
        private static Func<IPropertyDescriptor, string> <>f__am$cache0;

        protected TypeInspectorSkeleton()
        {
        }

        public abstract IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container);
        public IPropertyDescriptor GetProperty(Type type, object container, string name, bool ignoreUnmatched)
        {
            IPropertyDescriptor descriptor;
            <GetProperty>c__AnonStorey0 storey = new <GetProperty>c__AnonStorey0 {
                name = name
            };
            IEnumerable<IPropertyDescriptor> source = this.GetProperties(type, container).Where<IPropertyDescriptor>(new Func<IPropertyDescriptor, bool>(storey.<>m__0));
            using (IEnumerator<IPropertyDescriptor> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    if (!ignoreUnmatched)
                    {
                        object[] args = new object[] { storey.name, type.FullName };
                        throw new SerializationException(string.Format(CultureInfo.InvariantCulture, "Property '{0}' not found on type '{1}'.", args));
                    }
                    descriptor = null;
                }
                else
                {
                    IPropertyDescriptor current = enumerator.Current;
                    if (enumerator.MoveNext())
                    {
                        object[] args = new object[3];
                        args[0] = storey.name;
                        args[1] = type.FullName;
                        object[] objArray3 = args;
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = p => p.Name;
                        }
                        args[2] = string.Join(", ", source.Select<IPropertyDescriptor, string>(<>f__am$cache0).ToArray<string>());
                        throw new SerializationException(string.Format(CultureInfo.InvariantCulture, "Multiple properties with the name/alias '{0}' already exists on type '{1}', maybe you're misusing YamlAlias or maybe you are using the wrong naming convention? The matching properties are: {2}", args));
                    }
                    descriptor = current;
                }
            }
            return descriptor;
        }

        [CompilerGenerated]
        private sealed class <GetProperty>c__AnonStorey0
        {
            internal string name;

            internal bool <>m__0(IPropertyDescriptor p) => 
                p.Name == this.name;
        }
    }
}

