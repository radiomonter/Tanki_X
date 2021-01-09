namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using YamlDotNet;
    using YamlDotNet.Helpers;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.Utilities;

    public class FullObjectGraphTraversalStrategy : IObjectGraphTraversalStrategy
    {
        protected readonly Serializer serializer;
        private readonly int maxRecursion;
        private readonly ITypeInspector typeDescriptor;
        private readonly ITypeResolver typeResolver;
        private INamingConvention namingConvention;

        public FullObjectGraphTraversalStrategy(Serializer serializer, ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention)
        {
            if (maxRecursion <= 0)
            {
                throw new ArgumentOutOfRangeException("maxRecursion", maxRecursion, "maxRecursion must be greater than 1");
            }
            this.serializer = serializer;
            if (typeDescriptor == null)
            {
                throw new ArgumentNullException("typeDescriptor");
            }
            this.typeDescriptor = typeDescriptor;
            if (typeResolver == null)
            {
                throw new ArgumentNullException("typeResolver");
            }
            this.typeResolver = typeResolver;
            this.maxRecursion = maxRecursion;
            this.namingConvention = namingConvention;
        }

        private IObjectDescriptor GetObjectDescriptor(object value, Type staticType) => 
            new ObjectDescriptor(value, this.typeResolver.Resolve(staticType, value), staticType);

        protected virtual void Traverse(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth)
        {
            if (++currentDepth > this.maxRecursion)
            {
                throw new InvalidOperationException("Too much recursion when traversing the object graph");
            }
            if (visitor.Enter(value))
            {
                TypeCode typeCode = value.Type.GetTypeCode();
                switch (typeCode)
                {
                    case TypeCode.Empty:
                    {
                        object[] args = new object[] { typeCode };
                        throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "TypeCode.{0} is not supported.", args));
                    }
                    case TypeCode.DBNull:
                        visitor.VisitScalar(new ObjectDescriptor(null, typeof(object), typeof(object)));
                        break;

                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        visitor.VisitScalar(value);
                        break;

                    default:
                        if ((value.Value == null) || ReferenceEquals(value.Type, typeof(TimeSpan)))
                        {
                            visitor.VisitScalar(value);
                        }
                        else
                        {
                            Type underlyingType = Nullable.GetUnderlyingType(value.Type);
                            if (underlyingType != null)
                            {
                                this.Traverse(new ObjectDescriptor(value.Value, underlyingType, value.Type, value.ScalarStyle), visitor, currentDepth);
                            }
                            else
                            {
                                this.TraverseObject(value, visitor, currentDepth);
                            }
                        }
                        break;
                }
            }
        }

        protected virtual void TraverseDictionary(IObjectDescriptor dictionary, IObjectGraphVisitor visitor, int currentDepth, Type keyType, Type valueType)
        {
            visitor.VisitMappingStart(dictionary, keyType, valueType);
            bool flag = dictionary.Type.FullName.Equals("System.Dynamic.ExpandoObject");
            IDictionaryEnumerator enumerator = ((IDictionary) dictionary.Value).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    string str = !flag ? current.Key.ToString() : this.namingConvention.Apply(current.Key.ToString());
                    IObjectDescriptor objectDescriptor = this.GetObjectDescriptor(str, keyType);
                    IObjectDescriptor descriptor2 = this.GetObjectDescriptor(current.Value, valueType);
                    if (visitor.EnterMapping(objectDescriptor, descriptor2))
                    {
                        this.Traverse(objectDescriptor, visitor, currentDepth);
                        this.Traverse(descriptor2, visitor, currentDepth);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            visitor.VisitMappingEnd(dictionary);
        }

        private void TraverseList(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth)
        {
            Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(value.Type, typeof(IEnumerable<>));
            Type elementType = (implementedGenericInterface == null) ? typeof(object) : implementedGenericInterface.GetGenericArguments()[0];
            visitor.VisitSequenceStart(value, elementType);
            IEnumerator enumerator = ((IEnumerable) value.Value).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.Traverse(this.GetObjectDescriptor(current, elementType), visitor, currentDepth);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            visitor.VisitSequenceEnd(value);
        }

        protected virtual void TraverseObject(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth)
        {
            if (typeof(IDictionary).IsAssignableFrom(value.Type))
            {
                this.TraverseDictionary(value, visitor, currentDepth, typeof(object), typeof(object));
            }
            else
            {
                Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(value.Type, typeof(IDictionary<,>));
                if (implementedGenericInterface != null)
                {
                    GenericDictionaryToNonGenericAdapter adapter = new GenericDictionaryToNonGenericAdapter(value.Value, implementedGenericInterface);
                    Type[] genericArguments = implementedGenericInterface.GetGenericArguments();
                    this.TraverseDictionary(new ObjectDescriptor(adapter, value.Type, value.StaticType, value.ScalarStyle), visitor, currentDepth, genericArguments[0], genericArguments[1]);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(value.Type))
                {
                    this.TraverseList(value, visitor, currentDepth);
                }
                else
                {
                    this.TraverseProperties(value, visitor, currentDepth);
                }
            }
        }

        protected virtual void TraverseProperties(IObjectDescriptor value, IObjectGraphVisitor visitor, int currentDepth)
        {
            visitor.VisitMappingStart(value, typeof(string), typeof(object));
            foreach (IPropertyDescriptor descriptor in this.typeDescriptor.GetProperties(value.Type, value.Value))
            {
                IObjectDescriptor descriptor2 = descriptor.Read(value.Value);
                if (visitor.EnterMapping(descriptor, descriptor2))
                {
                    this.Traverse(new ObjectDescriptor(descriptor.Name, typeof(string), typeof(string)), visitor, currentDepth);
                    this.Traverse(descriptor2, visitor, currentDepth);
                }
            }
            visitor.VisitMappingEnd(value);
        }

        void IObjectGraphTraversalStrategy.Traverse(IObjectDescriptor graph, IObjectGraphVisitor visitor)
        {
            this.Traverse(graph, visitor, 0);
        }
    }
}

