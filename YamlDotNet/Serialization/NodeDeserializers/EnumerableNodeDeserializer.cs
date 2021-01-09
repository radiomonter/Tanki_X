namespace YamlDotNet.Serialization.NodeDeserializers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public sealed class EnumerableNodeDeserializer : INodeDeserializer
    {
        bool INodeDeserializer.Deserialize(EventReader reader, Type expectedType, Func<EventReader, Type, object> nestedObjectDeserializer, out object value)
        {
            Type type;
            if (ReferenceEquals(expectedType, typeof(IEnumerable)))
            {
                type = typeof(object);
            }
            else
            {
                Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IEnumerable<>));
                if (!ReferenceEquals(implementedGenericInterface, expectedType))
                {
                    value = null;
                    return false;
                }
                type = implementedGenericInterface.GetGenericArguments()[0];
            }
            Type[] typeArguments = new Type[] { type };
            Type type3 = typeof(List<>).MakeGenericType(typeArguments);
            value = nestedObjectDeserializer(reader, type3);
            return true;
        }
    }
}

