namespace Lobby.ClientPayment.main.csharp.Impl.Platbox
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1550c080512L)]
    public class PlatBoxBuyGoodsEvent : ProcessPaymentEvent
    {
        public string Phone { get; set; }
    }
}

