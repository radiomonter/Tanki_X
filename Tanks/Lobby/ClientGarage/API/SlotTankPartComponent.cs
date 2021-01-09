namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4af33360baf85L)]
    public class SlotTankPartComponent : Component
    {
        public TankPartModuleType TankPart { get; set; }
    }
}

