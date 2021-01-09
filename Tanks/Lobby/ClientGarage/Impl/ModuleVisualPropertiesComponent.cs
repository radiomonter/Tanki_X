namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ModuleVisualPropertiesComponent : Component
    {
        public ModuleVisualPropertiesComponent()
        {
            this.Properties = new List<ModuleVisualProperty>();
        }

        public List<ModuleVisualProperty> Properties { get; set; }
    }
}

