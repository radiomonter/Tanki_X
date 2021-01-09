namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d427a0cc5cb59aL)]
    public class SpecialOfferComponent : Component
    {
        public double GetSalePrice(double price) => 
            Math.Round((double) ((price * (100.0 - this.Discount)) / 100.0));

        public double Discount { get; set; }
    }
}

