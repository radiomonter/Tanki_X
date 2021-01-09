namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FirstBuySaleComponent : Component
    {
        public int SalePercent { get; set; }
    }
}

