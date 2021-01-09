namespace Platform.Kernel.ECS.ClientEntitySystem
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BroadcastHandlerResolver
    {
        protected static HandlerArgumetCombinator combinator = new HandlerArgumetCombinator();
        private readonly BroadcastEventHandlerCollector handlerCollector;
        protected List<Entity> entityAsList = new List<Entity>();

        public BroadcastHandlerResolver(BroadcastEventHandlerCollector handlerCollector)
        {
            this.handlerCollector = handlerCollector;
        }

        public IList<HandlerInvokeData> Resolve(Event eventInstance, Type handlerType)
        {
            List<HandlerInvokeData> instance = Cache.listHandlersInvokeData.GetInstance();
            IList<HandlerBroadcastInvokeData> handlers = this.handlerCollector.GetHandlers(handlerType);
            int count = handlers.Count;
            for (int i = 0; i < count; i++)
            {
                HandlerBroadcastInvokeData invokeData = handlers[i];
                if (invokeData.IsActual() || this.UpdateInvokeData(invokeData, eventInstance))
                {
                    instance.Add(invokeData);
                }
            }
            return instance;
        }

        protected bool UpdateInvokeData(HandlerBroadcastInvokeData invokeData, Event eventInstance)
        {
            HandlerInvokeGraph handlerInvokeGraph = invokeData.Handler.HandlerInvokeGraph.Init();
            this.entityAsList.Clear();
            this.entityAsList.Add(invokeData.Entity);
            bool flag = combinator.Combine(handlerInvokeGraph, this.entityAsList);
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
    }
}

