namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ScreenResolutionVariantComponent : Component
    {
        public int Width { get; set; }

        public int Height { get; set; }
    }
}

