namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MountItemButtonTextComponent : Component
    {
        public string MountText { get; set; }

        public string MountedText { get; set; }
    }
}

