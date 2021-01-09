namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WindowModeVariantComponent : Component
    {
        public bool Windowed { get; set; }
    }
}

