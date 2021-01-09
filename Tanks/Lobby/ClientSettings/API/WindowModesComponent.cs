namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WindowModesComponent : Component
    {
        public string Fullscreen { get; set; }

        public string Windowed { get; set; }
    }
}

