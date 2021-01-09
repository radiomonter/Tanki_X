namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class CompleteOnlyEventSender : EventSender
    {
        private readonly ICollection<Handler> completeHandlers;

        internal CompleteOnlyEventSender(ICollection<Handler> completeHandlers)
        {
            this.completeHandlers = completeHandlers;
        }

        public void Send(Flow flow, Event e, ICollection<Entity> entities)
        {
            flow.TryInvoke(this.completeHandlers, e, entities);
        }
    }
}

