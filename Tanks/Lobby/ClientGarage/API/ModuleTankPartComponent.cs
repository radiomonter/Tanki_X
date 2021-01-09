namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4adb91ab04028L)]
    public class ModuleTankPartComponent : Component
    {
        public TankPartModuleType TankPart { get; set; }
    }
}

