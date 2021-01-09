namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    internal class FireOnlyEventSender : EventSender
    {
        private readonly ICollection<Handler> fireHandlers;

        internal FireOnlyEventSender(ICollection<Handler> fireHandlers)
        {
            this.fireHandlers = fireHandlers;
        }

        public void Send(Flow flow, Event e, ICollection<Entity> entities)
        {
            flow.TryInvoke(this.fireHandlers, e, entities);
        }
    }
}

