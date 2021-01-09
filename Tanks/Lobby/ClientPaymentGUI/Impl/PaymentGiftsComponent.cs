namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d51beb34dc1da0L)]
    public class PaymentGiftsComponent : Component
    {
        public Dictionary<long, long> Gifts { get; set; }
    }
}

