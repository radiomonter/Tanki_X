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

    public class HandlerCollector
    {
        public HashSet<string> HandlersToIgnore;
        private readonly IList<HandlerFactory> factories = new List<HandlerFactory>();
        private readonly MultiMap<Type, Handler> groupType2Handler = new MultiMap<Type, Handler>();
        private readonly Type[] nodeChangeEventTypes = new Type[] { typeof(NodeAddedEvent), typeof(NodeRemoveEvent) };
        private readonly IDictionary<NodeDescription, List<Handler>> handlersByNode = new Dictionary<NodeDescription, List<Handler>>();
        private readonly IDictionary<Type, IDictionary<Type, List<Handler>>> handlersByEvent = new Dictionary<Type, IDictionary<Type, List<Handler>>>();
        private readonly IDictionary<Type, IDictionary<NodeDescription, List<Handler>>> handlersByContextNode = new Dictionary<Type, IDictionary<NodeDescription, List<Handler>>>();
        private readonly ICollection<EngineHandlerRegistrationListener> handlerListeners = new List<EngineHandlerRegistrationListener>();
        [CompilerGenerated]
        private static Comparison<MethodInfo> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<Type, List<Handler>> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<IDictionary<Type, List<Handler>>> <>f__am$cache2;
        [CompilerGenerated]
        private static Action<IDictionary<NodeDescription, List<Handler>>> <>f__am$cache3;
        [CompilerGenerated]
        private static Action<List<Handler>> <>f__am$cache4;
        [CompilerGenerated]
        private static Action<List<Handler>> <>f__am$cache5;

        private bool AddHandlerIfNeed(MethodInfo declaredMethod, ECSSystem system, ICollection<Handler> systemHandler)
        {
            bool flag;
            using (IEnumerator<HandlerFactory> enumerator = this.factories.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Handler handler = enumerator.Current.CreateHandler(declaredMethod, system);
                        if (handler == null)
                        {
                            continue;
                        }
                        if (this.HandlerMustBeIgnored(handler))
                        {
                            flag = true;
                        }
                        else
                        {
                            if (!declaredMethod.IsPublic)
                            {
                                throw new PrivateHandlerFoundException(declaredMethod);
                            }
                            foreach (EngineHandlerRegistrationListener listener in this.handlerListeners)
                            {
                                listener.OnHandlerAdded(handler);
                            }
                            this.Register(handler);
                            systemHandler.Add(handler);
                            flag = true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        public void AddHandlerListener(EngineHandlerRegistrationListener listener)
        {
            <AddHandlerListener>c__AnonStorey2 storey = new <AddHandlerListener>c__AnonStorey2 {
                listener = listener
            };
            this.handlerListeners.Add(storey.listener);
            this.handlersByEvent.Values.ForEach<IDictionary<Type, List<Handler>>>(new Action<IDictionary<Type, List<Handler>>>(storey.<>m__0));
        }

        private void CheckMethodIsNotHandler(MethodInfo method)
        {
            if (((method.GetParameters().Length != 0) && method.IsPublic) && method.GetParameters()[0].ParameterType.IsSubclassOf(typeof(Event)))
            {
                throw new MissingHandlerAnnotationException(method);
            }
        }

        private void CollectGroup2HandlerReference(Handler handler)
        {
            if (!this.nodeChangeEventTypes.Contains<Type>(handler.EventType))
            {
                foreach (HandlerArgument argument in handler.HandlerArgumentsDescription.HandlerArguments)
                {
                    Optional<JoinType> joinType = argument.JoinType;
                    if (joinType.IsPresent() && argument.JoinType.Get().ContextComponent.IsPresent())
                    {
                        Type key = argument.JoinType.Get().ContextComponent.Get();
                        this.groupType2Handler.Add(key, handler);
                    }
                }
            }
        }

        public ICollection<Handler> CollectHandlers(ECSSystem system)
        {
            ICollection<Handler> systemHandler = new List<Handler>();
            Type objA = system.GetType();
            while (!ReferenceEquals(objA, typeof(ECSSystem)))
            {
                MethodInfo[] methods = objA.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = (a, b) => string.CompareOrdinal(a.Name, b.Name);
                }
                Array.Sort<MethodInfo>(methods, <>f__am$cache0);
                MethodInfo[] infoArray2 = methods;
                int index = 0;
                while (true)
                {
                    if (index >= infoArray2.Length)
                    {
                        objA = objA.BaseType;
                        break;
                    }
                    MethodInfo declaredMethod = infoArray2[index];
                    if (!this.AddHandlerIfNeed(declaredMethod, system, systemHandler))
                    {
                        this.CheckMethodIsNotHandler(declaredMethod);
                    }
                    index++;
                }
            }
            return systemHandler;
        }

        private ICollection<Handler> Get(Type handlerType, Type eventType)
        {
            List<Handler> list;
            return (!this.handlersByEvent[handlerType].TryGetValue(eventType, out list) ? Collections.EmptyList<Handler>() : list);
        }

        public ICollection<Handler> GetHandlers(Type handlerType, NodeDescription nodeDescription)
        {
            List<Handler> list;
            return (!this.handlersByContextNode[handlerType].TryGetValue(nodeDescription, out list) ? Collections.EmptyList<Handler>() : list);
        }

        public ICollection<Handler> GetHandlers(Type handlerType, Type eventType)
        {
            if (this.IsNotInheritableEvent(eventType))
            {
                return this.Get(handlerType, eventType);
            }
            IList<Type> inheritableEventTypes = this.GetInheritableEventTypes(eventType);
            List<Handler> list2 = new List<Handler>();
            int count = inheritableEventTypes.Count;
            for (int i = 0; i < count; i++)
            {
                list2.AddRange(this.Get(handlerType, inheritableEventTypes[i]));
            }
            return list2;
        }

        public ICollection<Handler> GetHandlersByGroupComponent(Type groupComponentType) => 
            this.groupType2Handler.GetValues(groupComponentType);

        public ICollection<Handler> GetHandlersWithoutContext(NodeDescription nodeDescription)
        {
            List<Handler> list;
            return (!this.handlersByNode.TryGetValue(nodeDescription, out list) ? Collections.EmptyList<Handler>() : list);
        }

        private IList<Type> GetInheritableEventTypes(Type eventType)
        {
            if (eventType.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException();
            }
            List<Type> instance = Cache.listType.GetInstance();
            return ClassUtils.GetClasses(eventType, this.InheritableEventLimit, instance);
        }

        private bool HandlerMustBeIgnored(Handler handler) => 
            ((this.HandlersToIgnore != null) && (this.HandlersToIgnore.Count > 0)) && this.HandlersToIgnore.Contains(handler.FullMethodName);

        private bool IsNotInheritableEvent(Type eventType) => 
            !eventType.IsGenericTypeDefinition && ReferenceEquals(eventType.BaseType, this.InheritableEventLimit);

        private void Register(Handler handler)
        {
            this.RegisterByEvent(handler);
            this.RegisterByContextNode(handler);
            this.RegisterByNode(handler);
            this.CollectGroup2HandlerReference(handler);
        }

        private void RegisterByContextNode(Handler handler)
        {
            <RegisterByContextNode>c__AnonStorey0 storey = new <RegisterByContextNode>c__AnonStorey0 {
                handler = handler
            };
            storey.handlersByTask = this.handlersByContextNode[storey.handler.GetType()];
            storey.nodes = new HashSet<NodeDescription>();
            storey.handler.ContextArguments.ForEach<HandlerArgument>(new Action<HandlerArgument>(storey.<>m__0));
            storey.nodes.ForEach<NodeDescription>(new Action<NodeDescription>(storey.<>m__1));
        }

        private void RegisterByEvent(Handler handler)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = t => new List<Handler>();
            }
            this.handlersByEvent[handler.GetType()].ComputeIfAbsent<Type, List<Handler>>(handler.EventType, <>f__am$cache1).Add(handler);
        }

        private void RegisterByNode(Handler handler)
        {
            <RegisterByNode>c__AnonStorey1 storey = new <RegisterByNode>c__AnonStorey1 {
                handler = handler,
                $this = this,
                nodes = new HashSet<NodeDescription>()
            };
            storey.handler.HandlerArgumentsDescription.HandlerArguments.ForEach<HandlerArgument>(new Action<HandlerArgument>(storey.<>m__0));
            storey.nodes.ForEach<NodeDescription>(new Action<NodeDescription>(storey.<>m__1));
        }

        public void RegisterHandlerFactory(HandlerFactory factory)
        {
            this.factories.Add(factory);
        }

        public void RegisterTasksForHandler(Type handlerType)
        {
            this.handlersByEvent[handlerType] = new Dictionary<Type, List<Handler>>();
            this.handlersByContextNode[handlerType] = new Dictionary<NodeDescription, List<Handler>>();
        }

        public void SortHandlers()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (IDictionary<Type, List<Handler>> m) {
                    if (<>f__am$cache4 == null)
                    {
                        <>f__am$cache4 = c => c.Sort();
                    }
                    m.Values.ForEach<List<Handler>>(<>f__am$cache4);
                };
            }
            this.handlersByEvent.Values.ForEach<IDictionary<Type, List<Handler>>>(<>f__am$cache2);
            <>f__am$cache3 ??= delegate (IDictionary<NodeDescription, List<Handler>> m) {
                if (<>f__am$cache5 == null)
                {
                    <>f__am$cache5 = c => c.Sort();
                }
                m.Values.ForEach<List<Handler>>(<>f__am$cache5);
            };
            this.handlersByContextNode.Values.ForEach<IDictionary<NodeDescription, List<Handler>>>(<>f__am$cache3);
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        protected virtual Type InheritableEventLimit =>
            typeof(Event);

        [CompilerGenerated]
        private sealed class <AddHandlerListener>c__AnonStorey2
        {
            internal EngineHandlerRegistrationListener listener;

            internal void <>m__0(IDictionary<Type, List<Handler>> m)
            {
                m.Values.ForEach<List<Handler>>(h => h.ForEach(new Action<Handler>(this.listener.OnHandlerAdded)));
            }

            internal void <>m__1(List<Handler> h)
            {
                h.ForEach(new Action<Handler>(this.listener.OnHandlerAdded));
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterByContextNode>c__AnonStorey0
        {
            internal HashSet<NodeDescription> nodes;
            internal IDictionary<NodeDescription, List<Handler>> handlersByTask;
            internal Handler handler;
            private static Func<NodeDescription, List<Handler>> <>f__am$cache0;

            internal void <>m__0(HandlerArgument a)
            {
                this.nodes.Add(a.NodeDescription);
            }

            internal void <>m__1(NodeDescription desc)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = t => new List<Handler>();
                }
                this.handlersByTask.ComputeIfAbsent<NodeDescription, List<Handler>>(desc, <>f__am$cache0).Add(this.handler);
            }

            private static List<Handler> <>m__2(NodeDescription t) => 
                new List<Handler>();
        }

        [CompilerGenerated]
        private sealed class <RegisterByNode>c__AnonStorey1
        {
            internal HashSet<NodeDescription> nodes;
            internal Handler handler;
            internal HandlerCollector $this;
            private static Func<NodeDescription, List<Handler>> <>f__am$cache0;

            internal void <>m__0(HandlerArgument a)
            {
                this.nodes.Add(a.NodeDescription);
            }

            internal void <>m__1(NodeDescription desc)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = t => new List<Handler>();
                }
                this.$this.handlersByNode.ComputeIfAbsent<NodeDescription, List<Handler>>(desc, <>f__am$cache0).Add(this.handler);
            }

            private static List<Handler> <>m__2(NodeDescription t) => 
                new List<Handler>();
        }
    }
}

