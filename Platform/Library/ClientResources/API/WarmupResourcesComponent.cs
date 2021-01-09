namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WarmupResourcesComponent : Component
    {
        public List<string> AssetGuids { get; set; }
    }
}

