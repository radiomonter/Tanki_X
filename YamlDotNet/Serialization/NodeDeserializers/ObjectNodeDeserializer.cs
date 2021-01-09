namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class ObjectNodeDeserializer : INodeDeserializer
    {
        private readonly IObjectFactory _objectFactory;
        private readonly ITypeInspector _typeDescriptor;
        private readonly bool _ignoreUnmatched;

        public ObjectNodeDeserializer(IObjectFactory objectFactory, ITypeInspector typeDescriptor, bool ignoreUnmatched)
        {
            this._objectFactory = objectFactory;
            this._typeDescriptor = typeDescriptor;
            this._ignoreUnmatched = ignoreUnmatched;
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            if (reader.Allow<MappingStart>() == null)
            {
                value = null;
                return false;
            }
            value = this._objectFactory.Create(expectedType);
            while (!reader.Accept<MappingEnd>())
            {
                <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0 storey = new <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0();
                Scalar scalar = reader.Expect<Scalar>();
                storey.property = this._typeDescriptor.GetProperty(expectedType, null, scalar.Value, this._ignoreUnmatched);
                if (storey.property == null)
                {
                    reader.SkipThisAndNestedEvents();
                    continue;
                }
                object obj2 = nestedObjectDeserializer(reader, storey.property.Type);
                IValuePromise promise = obj2 as IValuePromise;
                if (promise == null)
                {
                    object obj3 = TypeConverter.ChangeType(obj2, storey.property.Type);
                    storey.property.Write(value, obj3);
                    continue;
                }
                <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey1 storey2 = new <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey1 {
                    <>f__ref$0 = storey,
                    valueRef = value
                };
                promise.ValueAvailable += new Action<object>(storey2.<>m__0);
            }
            reader.Expect<MappingEnd>();
            return true;
        }

        [CompilerGenerated]
        private sealed class <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0
        {
            internal IPropertyDescriptor property;
        }

        [CompilerGenerated]
        private sealed class <YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey1
        {
            internal object valueRef;
            internal ObjectNodeDeserializer.<YamlDotNet_Serialization_INodeDeserializer_Deserialize>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0(object v)
            {
                object obj2 = TypeConverter.ChangeType(v, this.<>f__ref$0.property.Type);
                this.<>f__ref$0.property.Write(this.valueRef, obj2);
            }
        }
    }
}

