namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientPayment.Impl;

    public class GoodsComponent : Component
    {
        public Tanks.Lobby.ClientPayment.Impl.SaleState SaleState = new Tanks.Lobby.ClientPayment.Impl.SaleState();
    }
}

