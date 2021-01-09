namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15de54622cdL)]
    public class AuthenticateSteamUserEvent : Event
    {
        public string SteamId { get; set; }

        public string Ticket { get; set; }

        public string HardwareFingerpring { get; set; }

        public string SteamNickname { get; set; }
    }
}

