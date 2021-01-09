namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeChangedHandlerResolver
    {
        protected static HandlerArgumetCombinator combinator = new HandlerArgumetCombinator();
        protected static NodeChangedReverseCombinator reverseCombinator = new NodeChangedReverseCombinator();
        protected List<Entity> changedEntityAsList = new List<Entity>();

        public virtual IList<HandlerInvokeData> Resolve(ICollection<Handler> handlers, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes)
        {
            if (handlers.Count == 0)
            {
                return Collections.EmptyList<HandlerInvokeData>();
            }
            List<HandlerInvokeData> instance = Cache.listHandlersInvokeData.GetInstance();
            Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator<Handler>(handlers);
            while (enumerator.MoveNext())
            {
                Handler current = enumerator.Current;
                HandlerInvokeData invokeData = Cache.flowInvokeData.GetInstance().Init(current);
                if (this.UpdateInvokeData(invokeData, current, eventInstance, entity, changedNodes))
                {
                    instance.Add(invokeData);
                }
            }
            return instance;
        }

        protected bool UpdateInvokeData(HandlerInvokeData invokeData, Handler handler, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes)
        {
            HandlerInvokeGraph handlerInvokeGraph = handler.HandlerInvokeGraph.Init();
            bool flag = reverseCombinator.Combine(handlerInvokeGraph, entity, changedNodes) && combinator.Combine(handlerInvokeGraph, null);
            if (flag)
            {
                invokeData.Update(eventInstance, handlerInvokeGraph);
            }
            handlerInvokeGraph.Clear();
            return flag;
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

