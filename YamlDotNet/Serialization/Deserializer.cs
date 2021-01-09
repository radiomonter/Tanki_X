namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization.NamingConventions;
    using YamlDotNet.Serialization.NodeDeserializers;
    using YamlDotNet.Serialization.NodeTypeResolvers;
    using YamlDotNet.Serialization.ObjectFactories;
    using YamlDotNet.Serialization.TypeInspectors;
    using YamlDotNet.Serialization.TypeResolvers;
    using YamlDotNet.Serialization.Utilities;
    using YamlDotNet.Serialization.ValueDeserializers;

    public sealed class Deserializer
    {
        private static readonly Dictionary<string, Type> predefinedTagMappings;
        private readonly Dictionary<string, Type> tagMappings;
        private readonly List<IYamlTypeConverter> converters;
        private TypeDescriptorProxy typeDescriptor = new TypeDescriptorProxy();
        private IValueDeserializer valueDeserializer;

        static Deserializer()
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type> {
                { 
                    "tag:yaml.org,2002:map",
                    typeof(Dictionary<object, object>)
                },
                { 
                    "tag:yaml.org,2002:bool",
                    typeof(bool)
                },
                { 
                    "tag:yaml.org,2002:float",
                    typeof(double)
                },
                { 
                    "tag:yaml.org,2002:int",
                    typeof(int)
                },
                { 
                    "tag:yaml.org,2002:str",
                    typeof(string)
                },
                { 
                    "tag:yaml.org,2002:timestamp",
                    typeof(DateTime)
                }
            };
            predefinedTagMappings = dictionary;
        }

        public Deserializer(IObjectFactory objectFactory = null, INamingConvention namingConvention = null, bool ignoreUnmatched = false, YamlAttributeOverrides overrides = null)
        {
            objectFactory = objectFactory ?? new DefaultObjectFactory();
            INamingConvention convention1 = namingConvention;
            if (namingConvention == null)
            {
                INamingConvention local2 = namingConvention;
                convention1 = new NullNamingConvention();
            }
            namingConvention = convention1;
            this.typeDescriptor.TypeDescriptor = new CachedTypeInspector(new YamlAttributesTypeInspector(new YamlAttributeOverridesInspector(new NamingConventionTypeInspector(new ReadableAndWritablePropertiesTypeInspector(new ReadablePropertiesTypeInspector(new StaticTypeResolver())), namingConvention), overrides)));
            this.converters = new List<IYamlTypeConverter>();
            foreach (IYamlTypeConverter converter in YamlTypeConverters.GetBuiltInConverters(false))
            {
                this.converters.Add(converter);
            }
            this.NodeDeserializers = new List<INodeDeserializer>();
            this.NodeDeserializers.Add(new TypeConverterNodeDeserializer(this.converters));
            this.NodeDeserializers.Add(new NullNodeDeserializer());
            this.NodeDeserializers.Add(new ScalarNodeDeserializer());
            this.NodeDeserializers.Add(new ArrayNodeDeserializer());
            this.NodeDeserializers.Add(new DictionaryNodeDeserializer(objectFactory));
            this.NodeDeserializers.Add(new CollectionNodeDeserializer(objectFactory));
            this.NodeDeserializers.Add(new EnumerableNodeDeserializer());
            this.NodeDeserializers.Add(new ObjectNodeDeserializer(objectFactory, this.typeDescriptor, ignoreUnmatched));
            this.tagMappings = new Dictionary<string, Type>(predefinedTagMappings);
            this.TypeResolvers = new List<INodeTypeResolver>();
            this.TypeResolvers.Add(new TagNodeTypeResolver(this.tagMappings));
            this.TypeResolvers.Add(new TypeNameInTagNodeTypeResolver());
            this.TypeResolvers.Add(new DefaultContainersNodeTypeResolver());
            this.valueDeserializer = new AliasValueDeserializer(new NodeValueDeserializer(this.NodeDeserializers, this.TypeResolvers));
        }

        public object Deserialize(TextReader input) => 
            this.Deserialize(input, typeof(object));

        public T Deserialize<T>(TextReader input) => 
            (T) this.Deserialize(input, typeof(T));

        public object Deserialize(EventReader reader) => 
            this.Deserialize(reader, typeof(object));

        public T Deserialize<T>(EventReader reader) => 
            (T) this.Deserialize(reader, typeof(T));

        public object Deserialize(TextReader input, Type type) => 
            this.Deserialize(new EventReader(new Parser(input)), type);

        public object Deserialize(EventReader reader, Type type)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            bool flag = reader.Allow<StreamStart>() != null;
            bool flag2 = reader.Allow<DocumentStart>() != null;
            object obj2 = null;
            if (!reader.Accept<DocumentEnd>() && !reader.Accept<StreamEnd>())
            {
                using (SerializerState state = new SerializerState())
                {
                    obj2 = this.valueDeserializer.DeserializeValue(reader, type, state, this.valueDeserializer);
                    state.OnDeserialization();
                }
            }
            if (flag2)
            {
                reader.Expect<DocumentEnd>();
            }
            if (flag)
            {
                reader.Expect<StreamEnd>();
            }
            return obj2;
        }

        public void RegisterTagMapping(string tag, Type type)
        {
            this.tagMappings.Add(tag, type);
        }

        public void RegisterTypeConverter(IYamlTypeConverter typeConverter)
        {
            this.converters.Insert(0, typeConverter);
        }

        public IList<INodeDeserializer> NodeDeserializers { get; private set; }

        public IList<INodeTypeResolver> TypeResolvers { get; private set; }

        private class TypeDescriptorProxy : ITypeInspector
        {
            public ITypeInspector TypeDescriptor;

            public IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container) => 
                this.TypeDescriptor.GetProperties(type, container);

            public IPropertyDescriptor GetProperty(Type type, object container, string name, bool ignoreUnmatched) => 
                this.TypeDescriptor.GetProperty(type, container, name, ignoreUnmatched);
        }
    }
}

