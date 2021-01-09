namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SaturationLevelVariantComponent : Component
    {
        public float Value { get; set; }
    }
}

