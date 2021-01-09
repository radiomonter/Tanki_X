namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using YamlDotNet;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Helpers;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class CollectionNodeDeserializer : INodeDeserializer
    {
        private readonly IObjectFactory _objectFactory;

        public CollectionNodeDeserializer(IObjectFactory objectFactory)
        {
            this._objectFactory = objectFactory;
        }

        internal static void DeserializeHelper(Type tItem, EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, IList result, bool canUpdate)
        {
            <DeserializeHelper>c__AnonStorey0 storey = new <DeserializeHelper>c__AnonStorey0 {
                tItem = tItem,
                result = result
            };
            reader.Expect<SequenceStart>();
            while (!reader.Accept<SequenceEnd>())
            {
                ParsingEvent current = reader.Parser.Current;
                object obj2 = nestedObjectDeserializer(reader, storey.tItem);
                IValuePromise promise = obj2 as IValuePromise;
                if (promise == null)
                {
                    storey.result.Add(TypeConverter.ChangeType(obj2, storey.tItem));
                    continue;
                }
                if (!canUpdate)
                {
                    throw new ForwardAnchorNotSupportedException(current.Start, current.End, "Forward alias references are not allowed because this type does not implement IList<>");
                }
                <DeserializeHelper>c__AnonStorey1 storey2 = new <DeserializeHelper>c__AnonStorey1 {
                    <>f__ref$0 = storey,
                    index = storey.result.Add(!storey.tItem.IsValueType() ? null : Activator.CreateInstance(storey.tItem))
                };
                promise.ValueAvailable += new Action<object>(storey2.<>m__0);
            }
            reader.Expect<SequenceEnd>();
        }

        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            IList list;
            Type type;
            bool canUpdate = true;
            Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(ICollection<>));
            if (implementedGenericInterface == null)
            {
                if (!typeof(IList).IsAssignableFrom(expectedType))
                {
                    value = false;
                    return false;
                }
                type = typeof(object);
                value = this._objectFactory.Create(expectedType);
                list = (IList) value;
            }
            else
            {
                type = implementedGenericInterface.GetGenericArguments()[0];
                value = this._objectFactory.Create(expectedType);
                list = value as IList;
                if (list == null)
                {
                    Type objA = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IList<>));
                    canUpdate = !ReferenceEquals(objA, null);
                    list = new GenericCollectionToNonGenericAdapter(value, implementedGenericInterface, objA);
                }
            }
            DeserializeHelper(type, reader, expectedType, nestedObjectDeserializer, list, canUpdate);
            return true;
        }

        [CompilerGenerated]
        private sealed class <DeserializeHelper>c__AnonStorey0
        {
            internal Type tItem;
            internal IList result;
        }

        [CompilerGenerated]
        private sealed class <DeserializeHelper>c__AnonStorey1
        {
            internal int index;
            internal CollectionNodeDeserializer.<DeserializeHelper>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0(object v)
            {
                this.<>f__ref$0.result[this.index] = TypeConverter.ChangeType(v, this.<>f__ref$0.tItem);
            }
        }
    }
}

