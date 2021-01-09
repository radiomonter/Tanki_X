namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15669bd2839L)]
    public class GoToPaymentRequestEvent : Event
    {
        private string steamId;
        private string ticket;

        public bool SteamIsActive { get; set; }

        public string SteamId
        {
            get => 
                this.steamId ?? string.Empty;
            set => 
                this.steamId = value;
        }

        public string Ticket
        {
            get => 
                this.ticket ?? string.Empty;
            set => 
                this.ticket = value;
        }
    }
}

