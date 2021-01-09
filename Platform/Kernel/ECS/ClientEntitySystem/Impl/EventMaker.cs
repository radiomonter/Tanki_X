namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class EventMaker
    {
        private static readonly EventSender STUB = new StubEvenSender();
        private Dictionary<Type, EventSender> senderByEventType = new Dictionary<Type, EventSender>();
        private HandlerCollector handlerCollector;

        public EventMaker(HandlerCollector handlerCollector)
        {
            this.handlerCollector = handlerCollector;
        }

        protected EventSender CreateSender(Type eventType)
        {
            ICollection<Handler> handlers = this.handlerCollector.GetHandlers(typeof(EventFireHandler), eventType);
            ICollection<Handler> completeHandlers = this.handlerCollector.GetHandlers(typeof(EventCompleteHandler), eventType);
            return (((handlers.Count <= 0) || (completeHandlers.Count <= 0)) ? ((handlers.Count <= 0) ? ((completeHandlers.Count <= 0) ? STUB : new CompleteOnlyEventSender(completeHandlers)) : new FireOnlyEventSender(handlers)) : new FireAndCompleteEventSender(handlers, completeHandlers));
        }

        private EventSender GetSender(Type eventType) => 
            this.senderByEventType.ComputeIfAbsent<Type, EventSender>(eventType, new Func<Type, EventSender>(this.CreateSender));

        public virtual void Send(Flow flow, Event e, ICollection<Entity> entities)
        {
            this.GetSender(e.GetType()).Send(flow, e, entities);
        }
    }
}

