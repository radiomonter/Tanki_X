namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class VisualPropertiesComponent : Component
    {
        public VisualPropertiesComponent()
        {
            this.MainProperties = new List<MainVisualProperty>();
            this.Properties = new List<VisualProperty>();
        }

        public string Feature { get; set; }

        public List<MainVisualProperty> MainProperties { get; set; }

        public List<VisualProperty> Properties { get; set; }
    }
}

