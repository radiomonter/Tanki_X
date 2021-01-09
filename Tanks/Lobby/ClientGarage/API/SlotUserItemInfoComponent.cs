namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x159f357960eL)]
    public class SlotUserItemInfoComponent : Component
    {
        public Tanks.Lobby.ClientGarage.API.Slot Slot { get; set; }

        public Tanks.Lobby.ClientGarage.API.ModuleBehaviourType ModuleBehaviourType { get; set; }

        public int UpgradeLevel { get; set; }
    }
}

