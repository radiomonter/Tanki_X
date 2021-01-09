namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Collections.Generic;

    public interface EventListener
    {
        void OnEventSend(Event evt, ICollection<Entity> entities);
    }
}

