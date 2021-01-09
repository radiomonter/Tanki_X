namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AntialiasingQualityVariantComponent : Component
    {
        public int Value { get; set; }
    }
}

