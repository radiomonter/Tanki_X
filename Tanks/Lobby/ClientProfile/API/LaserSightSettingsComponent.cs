namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LaserSightSettingsComponent : Component
    {
        public bool Enabled { get; set; }
    }
}

