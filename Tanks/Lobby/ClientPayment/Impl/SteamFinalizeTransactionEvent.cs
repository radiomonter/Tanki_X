namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d47b4250a7d6edL), Shared]
    public class SteamFinalizeTransactionEvent : Event
    {
        public SteamFinalizeTransactionEvent()
        {
        }

        public SteamFinalizeTransactionEvent(string orderId, long appId, bool autorized)
        {
            this.OrderId = orderId;
            this.AppId = appId;
            this.Autorized = autorized;
        }

        public string OrderId { get; set; }

        public long AppId { get; set; }

        public bool Autorized { get; set; }
    }
}

