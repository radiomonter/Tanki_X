namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15dea9eb7adL)]
    public class SteamUserComponent : Component
    {
        public string SteamId { get; set; }

        public bool FreeUidChange { get; set; }
    }
}

