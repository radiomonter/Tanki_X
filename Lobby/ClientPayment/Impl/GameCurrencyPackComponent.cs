namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x152d554dc93L)]
    public class GameCurrencyPackComponent : Component
    {
        public int Amount { get; set; }

        public int Bonus { get; set; }
    }
}

