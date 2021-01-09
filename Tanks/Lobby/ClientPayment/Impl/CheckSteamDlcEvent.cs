namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e12a45159L)]
    public class CheckSteamDlcEvent : Event
    {
        public CheckSteamDlcEvent(string steamId, string ticket)
        {
            this.SteamId = steamId;
            this.Ticket = ticket;
        }

        public string SteamId { get; set; }

        public string Ticket { get; set; }
    }
}

