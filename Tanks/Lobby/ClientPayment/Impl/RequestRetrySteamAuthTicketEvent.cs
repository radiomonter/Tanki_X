namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4867337b9b523L)]
    public class RequestRetrySteamAuthTicketEvent : Event
    {
        public bool GoToPayment { get; set; }
    }
}

