namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158bebce718L)]
    public class QiwiProcessPaymentEvent : ProcessPaymentEvent
    {
        public string Account { get; set; }
    }
}

