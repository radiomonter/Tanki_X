namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GraphicsSettingsBuilderComponent : Component
    {
        public List<Entity> Items { get; set; }
    }
}

