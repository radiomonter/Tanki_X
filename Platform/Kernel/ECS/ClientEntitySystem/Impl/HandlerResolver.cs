namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HandlerResolver
    {
        protected static HandlerArgumetCombinator combinator = new HandlerArgumetCombinator();

        public virtual IList<HandlerInvokeData> Resolve(ICollection<Handler> handlers, Event eventInstance, ICollection<Entity> contextEntities)
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
                HandlerInvokeData invokeData = ((EngineServiceImpl) EngineService).HandlerContextDataStorage.GetInvokeData(current, eventInstance.GetType(), contextEntities);
                if (invokeData.Reuse(eventInstance) || this.UpdateInvokeData(invokeData, current, eventInstance, contextEntities))
                {
                    instance.Add(invokeData);
                }
            }
            return instance;
        }

        protected virtual bool UpdateInvokeData(HandlerInvokeData invokeData, Handler handler, Event eventInstance, ICollection<Entity> contextEntities)
        {
            if (handler.IsEventOnlyArguments)
            {
                invokeData.UpdateForEventOnlyArguments(eventInstance);
                return true;
            }
            HandlerInvokeGraph handlerInvokeGraph = handler.HandlerInvokeGraph.Init();
            bool flag = combinator.Combine(handlerInvokeGraph, contextEntities);
            if (flag)
            {
                invokeData.Update(eventInstance, handlerInvokeGraph);
            }
            else
            {
                invokeData.UpdateForEmptyCall();
            }
            handlerInvokeGraph.Clear();
            return flag;
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

