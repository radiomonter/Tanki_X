namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x150d713a299L)]
    public class AutoRemoveComponentsEvent : Event
    {
        public AutoRemoveComponentsEvent()
        {
        }

        public AutoRemoveComponentsEvent(List<Type> componentsToRemove)
        {
            this.ComponentsToRemove = componentsToRemove;
        }

        public List<Type> ComponentsToRemove { get; set; }
    }
}

