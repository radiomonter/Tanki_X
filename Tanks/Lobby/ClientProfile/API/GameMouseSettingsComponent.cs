namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GameMouseSettingsComponent : Component
    {
        public bool MouseControlAllowed { get; set; }

        public bool MouseVerticalInverted { get; set; }

        public float MouseSensivity { get; set; }
    }
}

