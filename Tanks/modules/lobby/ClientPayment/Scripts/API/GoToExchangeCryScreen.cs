namespace tanks.modules.lobby.ClientPayment.Scripts.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GoToExchangeCryScreen : Event
    {
        public GoToExchangeCryScreen()
        {
            this.ExchangingCrystalls = 0x3e8L;
        }

        public GoToExchangeCryScreen(long exchangingCrystalls)
        {
            this.ExchangingCrystalls = exchangingCrystalls;
        }

        public long ExchangingCrystalls { get; set; }
    }
}

