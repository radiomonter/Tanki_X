namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Helpers;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class DictionaryNodeDeserializer : INodeDeserializer
    {
        private readonly IObjectFactory _objectFactory;

        public DictionaryNodeDeserializer(IObjectFactory objectFactory)
        {
            this._objectFactory = objectFactory;
        }

        private static void DeserializeHelper(Type tKey, Type tValue, EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, IDictionary result)
        {
            <DeserializeHelper>c__AnonStorey0 storey = new <DeserializeHelper>c__AnonStorey0 {
                result = result
            };
            reader.Expect<MappingStart>();
            while (!reader.Accept<MappingEnd>())
            {
                <DeserializeHelper>c__AnonStorey1 storey2 = new <DeserializeHelper>c__AnonStorey1 {
                    <>f__ref$0 = storey,
                    key = nestedObjectDeserializer(reader, tKey)
                };
                IValuePromise key = storey2.key as IValuePromise;
                storey2.value = nestedObjectDeserializer(reader, tValue);
                IValuePromise promise2 = storey2.value as IValuePromise;
                if (key == null)
                {
                    if (promise2 == null)
                    {
                        storey.result[storey2.key] = storey2.value;
                        continue;
                    }
                    promise2.ValueAvailable += new Action<object>(storey2.<>m__0);
                    continue;
                }
                if (promise2 == null)
                {
                    key.ValueAvailable += new Action<object>(storey2.<>m__1);
                    continue;
                }
                <DeserializeHelper>c__AnonStorey2 storey3 = new <DeserializeHelper>c__AnonStorey2 {
                    <>f__ref$0 = storey,
                    <>f__ref$1 = storey2,
                    hasFirstPart = false
                };
                key.ValueAvailable += new Action<object>(storey3.<>m__0);
                promise2.ValueAvailable += new Action<object>(storey3.<>m__1);
            }
            reader.Expect<MappingEnd>();
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            IDictionary dictionary;
            Type type;
            Type type2;
            Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IDictionary<,>));
            if (implementedGenericInterface != null)
            {
                Type[] genericArguments = implementedGenericInterface.GetGenericArguments();
                type = genericArguments[0];
                type2 = genericArguments[1];
                value = this._objectFactory.Create(expectedType);
                dictionary = value as IDictionary;
                dictionary = new GenericDictionaryToNonGenericAdapter(value, implementedGenericInterface);
            }
            else
            {
                if (!typeof(IDictionary).IsAssignableFrom(expectedType))
                {
                    value = null;
                    return false;
                }
                type = typeof(object);
                type2 = typeof(object);
                value = this._objectFactory.Create(expectedType);
                dictionary = (IDictionary) value;
            }
            DeserializeHelper(type, type2, reader, expectedType, nestedObjectDeserializer, dictionary);
            return true;
        }

        [CompilerGenerated]
        private sealed class <DeserializeHelper>c__AnonStorey0
        {
            internal IDictionary result;
        }

        [CompilerGenerated]
        private sealed class <DeserializeHelper>c__AnonStorey1
        {
            internal object key;
            internal object value;
            internal DictionaryNodeDeserializer.<DeserializeHelper>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0(object v)
            {
                this.<>f__ref$0.result[this.key] = v;
            }

            internal void <>m__1(object v)
            {
                this.<>f__ref$0.result[v] = this.value;
            }
        }

        [CompilerGenerated]
        private sealed class <DeserializeHelper>c__AnonStorey2
        {
            internal bool hasFirstPart;
            internal DictionaryNodeDeserializer.<DeserializeHelper>c__AnonStorey0 <>f__ref$0;
            internal DictionaryNodeDeserializer.<DeserializeHelper>c__AnonStorey1 <>f__ref$1;

            internal void <>m__0(object v)
            {
                if (this.hasFirstPart)
                {
                    this.<>f__ref$0.result[v] = this.<>f__ref$1.value;
                }
                else
                {
                    this.<>f__ref$1.key = v;
                    this.hasFirstPart = true;
                }
            }

            internal void <>m__1(object v)
            {
                if (this.hasFirstPart)
                {
                    this.<>f__ref$0.result[this.<>f__ref$1.key] = v;
                }
                else
                {
                    this.<>f__ref$1.value = v;
                    this.hasFirstPart = true;
                }
            }
        }
    }
}

