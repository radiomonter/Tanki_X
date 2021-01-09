namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DescriptionBundleItemComponent : Component
    {
        public Dictionary<string, string> Names { get; set; }
    }
}

