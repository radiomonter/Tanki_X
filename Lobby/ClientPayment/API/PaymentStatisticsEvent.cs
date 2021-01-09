namespace Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1568d829ea5L)]
    public class PaymentStatisticsEvent : Event
    {
        public PaymentStatisticsAction Action { get; set; }

        public long Item { get; set; }

        public long Method { get; set; }

        public string Screen { get; set; }
    }
}

