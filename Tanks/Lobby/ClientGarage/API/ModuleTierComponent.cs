namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4b31b98f91c26L)]
    public class ModuleTierComponent : Component
    {
        public int TierNumber { get; set; }
    }
}

