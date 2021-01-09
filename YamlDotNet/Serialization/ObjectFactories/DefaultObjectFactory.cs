namespace YamlDotNet.Serialization.ObjectFactories
{
    using System;
    using System.Collections.Generic;
    using YamlDotNet;
    using YamlDotNet.Serialization;

    public sealed class DefaultObjectFactory : IObjectFactory
    {
        private static readonly Dictionary<Type, Type> defaultInterfaceImplementations;

        static DefaultObjectFactory()
        {
            Dictionary<Type, Type> dictionary = new Dictionary<Type, Type> {
                { 
                    typeof(IEnumerable<>),
                    typeof(List<>)
                },
                { 
                    typeof(ICollection<>),
                    typeof(List<>)
                },
                { 
                    typeof(IList<>),
                    typeof(List<>)
                },
                { 
                    typeof(IDictionary<,>),
                    typeof(Dictionary<,>)
                }
            };
            defaultInterfaceImplementations = dictionary;
        }

        public object Create(Type type)
        {
            Type type2;
            object obj2;
            if (type.IsInterface() && defaultInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), out type2))
            {
                type = type2.MakeGenericType(type.GetGenericArguments());
            }
            try
            {
                obj2 = Activator.CreateInstance(type);
            }
            catch (Exception exception1)
            {
                throw new InvalidOperationException($"Failed to create an instance of type '{type}'.", exception1);
            }
            return obj2;
        }
    }
}

