namespace Platform.System.Data.Statics.ClientYaml.Impl
{
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Serialization;

    public class YamlServiceImpl : YamlService
    {
        public YamlServiceImpl()
        {
            this.Serializer = new YamlDotNet.Serialization.Serializer(SerializationOptions.None, new CamelToPascalCaseNamingConvention(), null);
            this.Deserializer = new YamlDotNet.Serialization.Deserializer(null, new PascalToCamelCaseNamingConvertion(), false, null);
        }

        public string Dump(object data)
        {
            StringWriter writer = new StringWriter();
            this.Serializer.Serialize(writer, data);
            return writer.ToString();
        }

        public void Dump(object data, FileInfo file)
        {
            using (FileStream stream = file.OpenWrite())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    this.Serializer.Serialize(writer, data);
                }
            }
        }

        public T Load<T>(YamlNodeImpl node) => 
            (T) this.Load(node, typeof(T));

        public YamlNodeImpl Load(FileInfo file)
        {
            YamlNodeImpl impl;
            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    impl = this.Load(reader);
                }
            }
            return impl;
        }

        public T Load<T>(FileInfo file)
        {
            T local;
            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    local = this.Deserializer.Deserialize<T>(reader);
                }
            }
            return local;
        }

        public YamlNodeImpl Load(TextReader data) => 
            new YamlNodeImpl((Dictionary<object, object>) this.Deserializer.Deserialize(data));

        public T Load<T>(TextReader reader) => 
            this.Deserializer.Deserialize<T>(reader);

        public YamlNodeImpl Load(string data) => 
            this.Load(new StringReader(data));

        public T Load<T>(string data)
        {
            StringReader input = new StringReader(data);
            return this.Deserializer.Deserialize<T>(input);
        }

        public virtual object Load(YamlNodeImpl node, Type type)
        {
            string data = this.Dump(node.innerDictionary);
            return this.Load(data, type);
        }

        public object Load(string data, Type type)
        {
            StringReader input = new StringReader(data);
            return this.Deserializer.Deserialize(input, type);
        }

        public void RegisterConverter(IYamlTypeConverter converter)
        {
            this.Serializer.RegisterTypeConverter(converter);
            this.Deserializer.RegisterTypeConverter(converter);
        }

        protected YamlDotNet.Serialization.Serializer Serializer { get; set; }

        protected YamlDotNet.Serialization.Deserializer Deserializer { get; set; }
    }
}

