namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LocalizedVisualPropertiesComponent : Component
    {
        public Dictionary<string, string> Names { get; set; }

        public Dictionary<string, string> Units { get; set; }
    }
}

