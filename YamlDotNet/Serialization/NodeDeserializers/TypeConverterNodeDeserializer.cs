namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;

    public sealed class TypeConverterNodeDeserializer : INodeDeserializer
    {
        private readonly IEnumerable<IYamlTypeConverter> converters;

        public TypeConverterNodeDeserializer(IEnumerable<IYamlTypeConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException("converters");
            }
            this.converters = converters;
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0 storey = new <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0 {
                expectedType = expectedType
            };
            IYamlTypeConverter converter = this.converters.FirstOrDefault<IYamlTypeConverter>(new Func<IYamlTypeConverter, bool>(storey.<>m__0));
            if (converter == null)
            {
                value = null;
                return false;
            }
            value = converter.ReadYaml(reader.Parser, storey.expectedType);
            return true;
        }

        [CompilerGenerated]
        private sealed class <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0
        {
            internal Type expectedType;

            internal bool <>m__0(IYamlTypeConverter c) => 
                c.Accepts(this.expectedType);
        }
    }
}

