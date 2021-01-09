namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Flow
    {
        private readonly EngineServiceInternal engineService;
        private static Flow current;
        private EventMaker eventMaker;
        private FlowListener flowListener;
        private HandlerResolver handlerResolver = new HandlerResolver();
        private NodeChangedHandlerResolver nodeChangedHandlerResolver = new NodeChangedHandlerResolver();
        private BroadcastHandlerResolver broadcastHandlerResolver;
        [CompilerGenerated]
        private static Action<FlowListener> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<FlowListener> <>f__am$cache1;

        public Flow(EngineServiceInternal engineService)
        {
            Current = this;
            this.engineService = engineService;
            this.NodeCollector = engineService.NodeCollector;
            this.EntityRegistry = engineService.EntityRegistry;
            this.eventMaker = engineService.EventMaker;
            this.handlerResolver = new HandlerResolver();
            this.nodeChangedHandlerResolver = new NodeChangedHandlerResolver();
            this.broadcastHandlerResolver = new BroadcastHandlerResolver(engineService.BroadcastEventHandlerCollector);
        }

        public void Clean()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = l => l.OnFlowClean();
            }
            Collections.ForEach<FlowListener>(this.engineService.FlowListeners, <>f__am$cache1);
        }

        public void Finish()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = l => l.OnFlowFinish();
            }
            Collections.ForEach<FlowListener>(this.engineService.FlowListeners, <>f__am$cache0);
        }

        private void NotifySendEvent(Event e, ICollection<Entity> entities)
        {
            Collections.Enumerator<EventListener> enumerator = Collections.GetEnumerator<EventListener>(this.engineService.EventListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnEventSend(e, entities);
            }
        }

        public void ScheduleWith(Consumer<Engine> consumer)
        {
            consumer(this.engineService.Engine);
        }

        public void SendEvent(Event e, Entity entity)
        {
            this.SendEvent(e, Collections.SingletonList<Entity>(entity));
        }

        public virtual void SendEvent(Event e, ICollection<Entity> entities)
        {
            this.NotifySendEvent(e, entities);
            this.SendEventSilent(e, entities);
        }

        public void SendEventSilent(Event e, ICollection<Entity> entities)
        {
            this.eventMaker.Send(this, e, entities);
        }

        public void TryInvoke(Event eventInstance, Type handlerType)
        {
            IList<HandlerInvokeData> otherInvokeArguments = this.broadcastHandlerResolver.Resolve(eventInstance, handlerType);
            for (int i = 0; i < otherInvokeArguments.Count; i++)
            {
                otherInvokeArguments[i].Invoke(otherInvokeArguments);
            }
        }

        public void TryInvoke(ICollection<Handler> handlers, Event eventInstance, ICollection<Entity> contextEntities)
        {
            IList<HandlerInvokeData> otherInvokeArguments = this.handlerResolver.Resolve(handlers, eventInstance, contextEntities);
            for (int i = 0; i < otherInvokeArguments.Count; i++)
            {
                otherInvokeArguments[i].Invoke(otherInvokeArguments);
            }
        }

        public void TryInvoke(ICollection<Handler> fireHandlers, ICollection<Handler> completeHandlers, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes)
        {
            IList<HandlerInvokeData> otherInvokeArguments = this.nodeChangedHandlerResolver.Resolve(fireHandlers, eventInstance, entity, changedNodes);
            IList<HandlerInvokeData> list2 = this.nodeChangedHandlerResolver.Resolve(completeHandlers, eventInstance, entity, changedNodes);
            for (int i = 0; i < otherInvokeArguments.Count; i++)
            {
                otherInvokeArguments[i].Invoke(otherInvokeArguments);
            }
            for (int j = 0; j < list2.Count; j++)
            {
                list2[j].Invoke(list2);
            }
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        [Inject]
        public static SharedEntityRegistry sharedEntityRegistry { get; set; }

        public static Flow Current { get; private set; }

        public NodeCollectorImpl NodeCollector { get; internal set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.EntityRegistry EntityRegistry { get; internal set; }
    }
}

