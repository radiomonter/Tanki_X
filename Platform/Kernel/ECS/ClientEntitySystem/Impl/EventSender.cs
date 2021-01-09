namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public interface EventSender
    {
        void Send(Flow flow, Event e, ICollection<Entity> entities);
    }
}

