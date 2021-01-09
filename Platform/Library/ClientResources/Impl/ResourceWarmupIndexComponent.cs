namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ResourceWarmupIndexComponent : Component
    {
        public ResourceWarmupIndexComponent()
        {
            this.Index = 0;
        }

        public int Index { get; set; }
    }
}

