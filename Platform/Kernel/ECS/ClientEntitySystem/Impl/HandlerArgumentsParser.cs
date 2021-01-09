namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class HandlerArgumentsParser
    {
        private readonly MethodInfo method;
        private Type[] NodeChangeEventTypes = new Type[] { typeof(NodeAddedEvent), typeof(NodeRemoveEvent) };

        public HandlerArgumentsParser(MethodInfo method)
        {
            this.method = method;
        }

        private void CheckArguments(IList<HandlerArgument> arguments)
        {
            this.CheckFirstNotJoin(arguments);
        }

        private void CheckFirstNotJoin(IList<HandlerArgument> arguments)
        {
            if (arguments.Count > 0)
            {
                HandlerArgument handlerArgument = arguments[0];
                if (handlerArgument.JoinType.IsPresent() && handlerArgument.JoinType.Get().ContextComponent.IsPresent())
                {
                    throw new JoinFirstNodeArgumentException(this.method, handlerArgument);
                }
            }
        }

        private static void CollectGroupComponent(Optional<JoinType> join, HashSet<Type> components)
        {
            if (join.IsPresent())
            {
                Optional<Type> contextComponent = join.Get().ContextComponent;
                if (contextComponent.IsPresent())
                {
                    components.Add(contextComponent.Get());
                }
            }
        }

        private static HashSet<Type> Concat(HashSet<Type> s1, HashSet<Type> s2)
        {
            HashSet<Type> set = new HashSet<Type>(s1);
            foreach (Type type in s2)
            {
                set.Add(type);
            }
            return set;
        }

        private static HandlerArgument CreateNodeType(int position, Type type, Optional<JoinType> join, Optional<JoinType> rightJoin, object[] annotatedTypes, bool isNodeChangeHandler)
        {
            Type nodeType = GetNodeType(type);
            if (nodeType == null)
            {
                return null;
            }
            HashSet<Type> components = new HashSet<Type>();
            bool context = IsContextNode(annotatedTypes, join);
            if (isNodeChangeHandler && context)
            {
                CollectGroupComponent(join, components);
                CollectGroupComponent(rightJoin, components);
            }
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(nodeType, components);
            return new HandlerArgumentBuilder().SetPosition(position).SetType(type).SetJoinType(join).SetContext(context).SetCollection(IsCollection(type)).SetNodeClassInstanceDescription(orCreateNodeClassDescription).SetMandatory(IsMandatory(annotatedTypes)).SetCombine(IsCombine(annotatedTypes)).SetOptional(IsOptional(type)).Build();
        }

        private static HashSet<Type> GetComponentsFromNodes(IList<HandlerArgument> handlerArguments)
        {
            HashSet<Type> set = new HashSet<Type>();
            foreach (HandlerArgument argument in handlerArguments)
            {
                NodeDescription nodeDescription = argument.NodeDescription;
                foreach (Type type in nodeDescription.Components)
                {
                    set.Add(type);
                }
                foreach (Type type2 in nodeDescription.NotComponents)
                {
                    set.Add(type2);
                }
            }
            return set;
        }

        private static Optional<JoinType> GetJoinType(object[] annotatedTypes)
        {
            List<object> list = new List<object>();
            object[] objArray = annotatedTypes;
            int index = 0;
            while (true)
            {
                Optional<JoinType> optional;
                if (index < objArray.Length)
                {
                    object item = objArray[index];
                    list.Add(item);
                    object[] customAttributes = item.GetType().GetCustomAttributes(true);
                    list.AddRange(customAttributes);
                    index++;
                    continue;
                }
                using (List<object>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            object current = enumerator.Current;
                            if (current is JoinAll)
                            {
                                optional = Optional<JoinType>.of(new JoinAllType());
                            }
                            else if (current is JoinBy)
                            {
                                optional = Optional<JoinType>.of(new JoinByType(((JoinBy) current).value));
                            }
                            else
                            {
                                if (!(current is JoinSelf))
                                {
                                    continue;
                                }
                                optional = Optional<JoinType>.of(new JoinSelfType());
                            }
                        }
                        else
                        {
                            break;
                        }
                        break;
                    }
                }
                return optional;
            }
        }

        private static Type GetNodeType(Type type)
        {
            if (IsOptional(type))
            {
                return GetNodeType(type.GetGenericArguments()[0]);
            }
            if (IsNode(type))
            {
                return type;
            }
            if (IsCollection(type))
            {
                Type type2 = type.GetGenericArguments()[0];
                if (IsNode(type2))
                {
                    return type2;
                }
                if (type2.IsSubclassOf(typeof(AbstractSingleNode)))
                {
                    Type type3 = type2.GetGenericArguments()[0];
                    if (IsNode(type3))
                    {
                        return type3;
                    }
                }
            }
            return null;
        }

        private static bool HasAttrubte(object[] attrubtes, Type type)
        {
            foreach (object obj2 in attrubtes)
            {
                if (ReferenceEquals(obj2.GetType(), type))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsCollection(Type type) => 
            !IsOptional(type) ? (type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(ICollection<>))) : IsCollection(type.GetGenericArguments()[0]);

        private static bool IsCombine(object[] annotatedTypes) => 
            HasAttrubte(annotatedTypes, typeof(Combine));

        private static bool IsContextNode(object[] annotatedTypes, Optional<JoinType> joinType) => 
            !HasAttrubte(annotatedTypes, typeof(Context)) ? !joinType.IsPresent() : true;

        private static bool IsMandatory(object[] annotatedTypes) => 
            (!TestContext.IsTestMode || !TestContext.Current.IsDataExists(typeof(MandatoryDisabled))) ? HasAttrubte(annotatedTypes, typeof(Mandatory)) : false;

        private static bool IsNode(Type type) => 
            type.IsSubclassOf(typeof(Node)) || ReferenceEquals(type, typeof(Node));

        private bool IsNodeChangeHandler(IEnumerable<Type> eventClasses) => 
            eventClasses.Any<Type>(e => this.NodeChangeEventTypes.Contains<Type>(e));

        private static bool IsOptional(Type type) => 
            type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Optional<>));

        public HandlerArgumentsDescription Parse()
        {
            HashSet<Type> eventClasses = this.ParseEvents();
            List<HandlerArgument> handlerArguments = this.ParseHandlerArguments(this.IsNodeChangeHandler(eventClasses));
            return new HandlerArgumentsDescription(handlerArguments, eventClasses, this.ParseComponents(handlerArguments));
        }

        private HashSet<Type> ParseClasses(Type clazz)
        {
            HashSet<Type> set = new HashSet<Type>();
            ParameterInfo[] parameters = this.method.GetParameters();
            int index = 0;
            while (index < parameters.Length)
            {
                ParameterInfo info = parameters[index];
                set.Add(info.ParameterType);
                Type[] typeArray2 = info.ParameterType.GetGenericArguments();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= typeArray2.Length)
                    {
                        index++;
                        break;
                    }
                    Type item = typeArray2[num2];
                    set.Add(item);
                    num2++;
                }
            }
            HashSet<Type> set2 = new HashSet<Type>();
            foreach (Type type2 in set)
            {
                if (type2.IsSubclassOf(clazz))
                {
                    set2.Add(type2);
                }
            }
            return set2;
        }

        private HashSet<Type> ParseComponents(IList<HandlerArgument> handlerArguments) => 
            Concat(this.ParseClasses(typeof(Component)), GetComponentsFromNodes(handlerArguments));

        private HashSet<Type> ParseEvents() => 
            this.ParseClasses(typeof(Event));

        private List<HandlerArgument> ParseHandlerArguments(bool isNodeChangeHandler)
        {
            ParameterInfo[] parameters = this.method.GetParameters();
            HandlerArgument[] arguments = new HandlerArgument[parameters.Length - 1];
            int position = 0;
            Optional<JoinType> rightJoin = Optional<JoinType>.empty();
            for (int i = parameters.Length - 1; i > 0; i--)
            {
                position = i - 1;
                ParameterInfo parameterInfo = parameters[i];
                object[] customAttributes = parameterInfo.GetCustomAttributes(true);
                Optional<JoinType> joinType = GetJoinType(customAttributes);
                HandlerArgument argument = CreateNodeType(position, parameterInfo.ParameterType, joinType, rightJoin, customAttributes, isNodeChangeHandler);
                rightJoin = joinType;
                if (argument == null)
                {
                    throw new ArgumentMustBeNodeException(this.method, parameterInfo);
                }
                arguments[position] = argument;
            }
            this.CheckArguments(arguments);
            return arguments.ToList<HandlerArgument>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

