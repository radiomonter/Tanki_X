namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14e6cbf2074L)]
    public class UpgradeLevelItemComponent : Component
    {
        public int Level { get; set; }
    }
}

