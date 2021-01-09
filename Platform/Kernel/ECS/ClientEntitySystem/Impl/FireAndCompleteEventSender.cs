namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class FireAndCompleteEventSender : EventSender
    {
        private readonly ICollection<Handler> fireHandlers;
        private readonly ICollection<Handler> completeHandlers;

        internal FireAndCompleteEventSender(ICollection<Handler> fireHandlers, ICollection<Handler> completeHandlers)
        {
            this.fireHandlers = fireHandlers;
            this.completeHandlers = completeHandlers;
        }

        public void Send(Flow flow, Event e, ICollection<Entity> entities)
        {
            flow.TryInvoke(this.fireHandlers, e, entities);
            flow.TryInvoke(this.completeHandlers, e, entities);
        }
    }
}

