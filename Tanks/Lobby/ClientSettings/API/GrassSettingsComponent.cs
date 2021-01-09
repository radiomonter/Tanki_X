namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GrassSettingsComponent : Component
    {
        public int Value { get; set; }

        public float GrassNearDrawDistance { get; set; }

        public float GrassFarDrawDistance { get; set; }

        public float GrassFadeRange { get; set; }

        public float GrassDensityMultiplier { get; set; }

        public bool GrassCastsShadow { get; set; }
    }
}

