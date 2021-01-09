namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15282b5b204L)]
    public class GoodsPriceComponent : Component
    {
        private const double roundRatio = 100.0;

        public double Round(double value) => 
            Math.Round((double) (value * 100.0)) / 100.0;

        public string Currency { get; set; }

        public double Price { get; set; }
    }
}

