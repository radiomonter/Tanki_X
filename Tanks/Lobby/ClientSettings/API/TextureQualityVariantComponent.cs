namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TextureQualityVariantComponent : Component
    {
        public int Value { get; set; }

        public int MasterTextureLimit { get; set; }
    }
}

