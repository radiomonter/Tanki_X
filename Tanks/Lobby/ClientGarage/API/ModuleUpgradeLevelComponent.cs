namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4b24c3378f178L)]
    public class ModuleUpgradeLevelComponent : Component
    {
        public long Level { get; set; }
    }
}

