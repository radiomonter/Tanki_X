namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x15c19d1eb96L)]
    public class ExchangeRateComponent : Component
    {
        private static float rate;

        public static float ExhchageRate =>
            rate;

        public float Rate
        {
            get => 
                rate;
            set => 
                rate = value;
        }
    }
}

