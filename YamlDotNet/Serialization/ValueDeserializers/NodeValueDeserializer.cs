namespace YamlDotNet.Serialization.ValueDeserializers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class NodeValueDeserializer : IValueDeserializer
    {
        private readonly IList<INodeDeserializer> deserializers;
        private readonly IList<INodeTypeResolver> typeResolvers;

        public NodeValueDeserializer(IList<INodeDeserializer> deserializers, IList<INodeTypeResolver> typeResolvers)
        {
            if (deserializers == null)
            {
                throw new ArgumentNullException("deserializers");
            }
            this.deserializers = deserializers;
            if (typeResolvers == null)
            {
                throw new ArgumentNullException("typeResolvers");
            }
            this.typeResolvers = typeResolvers;
        }

        public object DeserializeValue(EventReader reader, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
        {
            <DeserializeValue>c__AnonStorey0 storey = new <DeserializeValue>c__AnonStorey0 {
                nestedObjectDeserializer = nestedObjectDeserializer,
                state = state
            };
            NodeEvent nodeEvent = reader.Peek<NodeEvent>();
            Type typeFromEvent = this.GetTypeFromEvent(nodeEvent, expectedType);
            try
            {
                using (IEnumerator<INodeDeserializer> enumerator = this.deserializers.GetEnumerator())
                {
                    while (true)
                    {
                        object obj2;
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        INodeDeserializer current = enumerator.Current;
                        if (current.Deserialize(reader, typeFromEvent, new Func<EventReader, Type, object>(storey.<>m__0), out obj2))
                        {
                            return obj2;
                        }
                    }
                }
            }
            catch (YamlException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new YamlException(nodeEvent.Start, nodeEvent.End, "Exception during deserialization", exception);
            }
            throw new YamlException(nodeEvent.Start, nodeEvent.End, $"No node deserializer was able to deserialize the node into type {expectedType.AssemblyQualifiedName}");
        }

        private Type GetTypeFromEvent(NodeEvent nodeEvent, Type currentType)
        {
            foreach (INodeTypeResolver resolver in this.typeResolvers)
            {
                if (resolver.Resolve(nodeEvent, ref currentType))
                {
                    break;
                }
            }
            return currentType;
        }

        [CompilerGenerated]
        private sealed class <DeserializeValue>c__AnonStorey0
        {
            internal IValueDeserializer nestedObjectDeserializer;
            internal SerializerState state;

            internal object <>m__0(EventReader r, Type t) => 
                this.nestedObjectDeserializer.DeserializeValue(r, t, this.state, this.nestedObjectDeserializer);
        }
    }
}

