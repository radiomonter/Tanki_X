namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LoadByEventRequestComponent : Component
    {
        public Type ResourceDataComponentType { get; set; }

        public Entity Owner { get; set; }
    }
}

