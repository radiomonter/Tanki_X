namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CustomOfferPriceForUIComponent : Component
    {
        public CustomOfferPriceForUIComponent(double price)
        {
            this.Price = price;
        }

        public double Price { get; private set; }
    }
}

