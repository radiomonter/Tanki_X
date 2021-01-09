namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4299e94ee81ffL)]
    public class SpecialOfferRemainingTimeComponent : Component
    {
        public long Remain { get; set; }

        [ProtocolTransient]
        public Date EndDate { get; set; }
    }
}

