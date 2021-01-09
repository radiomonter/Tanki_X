namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FlowInstancesCache : AbstratFlowInstancesCache
    {
        public readonly Cache<List<Entity>> listEntity;
        public readonly Cache<HashSet<Entity>> setEntity;
        public readonly Cache<List<Type>> listType;
        public readonly Cache<List<Handler>> listHandlers;
        public readonly Cache<List<NodeDescription>> listNodeDescription;
        public readonly Cache<HashSet<NodeDescription>> setNodeDescription;
        public readonly Cache<List<HandlerInvokeData>> listHandlersInvokeData;
        public readonly Cache<FlowHandlerInvokeDate> flowInvokeData;
        public readonly Cache<EventBuilderImpl> eventBuilder;
        public readonly Cache<EntityNode> entityNode;
        public readonly Cache<ArrayList> arrayList;
        public readonly Cache<HandlerExecutor> handlerExecutor;
        public CacheMultisizeArray<object> array = new CacheMultisizeArrayImpl<object>();
        public CacheMultisizeArray<Entity> entityArray = new CacheMultisizeArrayImpl<Entity>();
        private Dictionary<Type, Type> genericListInstances = new Dictionary<Type, Type>();
        private Dictionary<Type, NodeInstanceCache> nodeInstancesCache = new Dictionary<Type, NodeInstanceCache>();
        [CompilerGenerated]
        private static Action<ArrayList> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<List<Entity>> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<HashSet<Entity>> <>f__am$cache2;
        [CompilerGenerated]
        private static Action<List<Type>> <>f__am$cache3;
        [CompilerGenerated]
        private static Action<HashSet<NodeDescription>> <>f__am$cache4;
        [CompilerGenerated]
        private static Action<List<NodeDescription>> <>f__am$cache5;
        [CompilerGenerated]
        private static Action<List<HandlerInvokeData>> <>f__am$cache6;
        [CompilerGenerated]
        private static Action<List<Handler>> <>f__am$cache7;
        [CompilerGenerated]
        private static Action<EntityNode> <>f__am$cache8;
        [CompilerGenerated]
        private static Func<Type, Type> <>f__am$cache9;

        public FlowInstancesCache()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action<ArrayList>(FlowInstancesCache.<FlowInstancesCache>m__0);
            }
            this.arrayList = this.Register<ArrayList>(<>f__am$cache0);
            <>f__am$cache1 ??= new Action<List<Entity>>(FlowInstancesCache.<FlowInstancesCache>m__1);
            this.listEntity = this.Register<List<Entity>>(<>f__am$cache1);
            <>f__am$cache2 ??= new Action<HashSet<Entity>>(FlowInstancesCache.<FlowInstancesCache>m__2);
            this.setEntity = this.Register<HashSet<Entity>>(<>f__am$cache2);
            <>f__am$cache3 ??= new Action<List<Type>>(FlowInstancesCache.<FlowInstancesCache>m__3);
            this.listType = this.Register<List<Type>>(<>f__am$cache3);
            <>f__am$cache4 ??= new Action<HashSet<NodeDescription>>(FlowInstancesCache.<FlowInstancesCache>m__4);
            this.setNodeDescription = this.Register<HashSet<NodeDescription>>(<>f__am$cache4);
            <>f__am$cache5 ??= new Action<List<NodeDescription>>(FlowInstancesCache.<FlowInstancesCache>m__5);
            this.listNodeDescription = this.Register<List<NodeDescription>>(<>f__am$cache5);
            <>f__am$cache6 ??= new Action<List<HandlerInvokeData>>(FlowInstancesCache.<FlowInstancesCache>m__6);
            this.listHandlersInvokeData = this.Register<List<HandlerInvokeData>>(<>f__am$cache6);
            this.flowInvokeData = base.Register<FlowHandlerInvokeDate>();
            this.flowInvokeData.SetMaxSize(0x7d0);
            <>f__am$cache7 ??= new Action<List<Handler>>(FlowInstancesCache.<FlowInstancesCache>m__7);
            this.listHandlers = this.Register<List<Handler>>(<>f__am$cache7);
            <>f__am$cache8 ??= new Action<EntityNode>(FlowInstancesCache.<FlowInstancesCache>m__8);
            this.entityNode = this.Register<EntityNode>(<>f__am$cache8);
            this.entityNode.SetMaxSize(0x3e8);
            this.eventBuilder = base.Register<EventBuilderImpl>();
            this.handlerExecutor = base.Register<HandlerExecutor>();
            this.handlerExecutor.SetMaxSize(0x3e8);
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__0(ArrayList list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__1(List<Entity> list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__2(HashSet<Entity> set)
        {
            set.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__3(List<Type> list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__4(HashSet<NodeDescription> set)
        {
            set.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__5(List<NodeDescription> list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__6(List<HandlerInvokeData> list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__7(List<Handler> list)
        {
            list.Clear();
        }

        [CompilerGenerated]
        private static void <FlowInstancesCache>m__8(EntityNode e)
        {
            e.Clear();
        }

        public void FreeNodeInstance(Node node)
        {
            NodeInstanceCache cache;
            if (this.nodeInstancesCache.TryGetValue(node.GetType(), out cache))
            {
                cache.Free(node);
            }
        }

        public IList GetGenericListInstance(Type nodeClass, int count)
        {
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = k => typeof(List<>).MakeGenericType(new Type[] { k });
            }
            object[] args = new object[] { count };
            return (IList) Activator.CreateInstance(this.genericListInstances.ComputeIfAbsent<Type, Type>(nodeClass, <>f__am$cache9), args);
        }

        public Node GetNodeInstance(Type nodeClass)
        {
            NodeInstanceCache cache;
            if (!this.nodeInstancesCache.TryGetValue(nodeClass, out cache))
            {
                cache = new NodeInstanceCache(nodeClass);
                this.nodeInstancesCache.Add(nodeClass, cache);
            }
            return cache.GetInstance();
        }

        public override void OnFlowClean()
        {
            base.OnFlowClean();
            this.array.FreeAll();
            Dictionary<Type, NodeInstanceCache>.Enumerator enumerator = this.nodeInstancesCache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, NodeInstanceCache> current = enumerator.Current;
                current.Value.OnFlowClean();
            }
        }
    }
}

