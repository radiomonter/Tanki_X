namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d49a63f9468c4eL), Shared]
    public class CheckTicketRequestEvent : Event
    {
        public CheckTicketRequestEvent(string steamId, string ticket)
        {
            this.SteamId = steamId;
            this.Ticket = ticket;
        }

        public string SteamId { get; set; }

        public string Ticket { get; set; }
    }
}

