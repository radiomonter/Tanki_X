namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158f6f9d97aL)]
    public class XCrystalsAmountSaleComponent : Component
    {
        public Date StartTime { get; set; }

        public Date StopTime { get; set; }

        public double Multiplier { get; set; }

        public long Addition { get; set; }
    }
}

