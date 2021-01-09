namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Steamworks;
    using System;
    using System.Runtime.CompilerServices;

    public class MicroTxnAuthorizationResponseEvent : Event
    {
        public MicroTxnAuthorizationResponseEvent(MicroTxnAuthorizationResponse_t response)
        {
            this.OrderId = response.m_ulOrderID.ToString();
            this.AppId = response.m_unAppID;
            this.Autorized = response.m_bAuthorized != 0;
        }

        public string OrderId { get; set; }

        public long AppId { get; set; }

        public bool Autorized { get; set; }
    }
}

