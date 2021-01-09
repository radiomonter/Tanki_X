namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d5aceb2e9a99aeL)]
    public class ActivePaymentSaleComponent : Component
    {
        public Date StopTime { get; set; }

        public float PriceMultiplier { get; set; }

        public float AmountMultiplier { get; set; }

        public bool Personal { get; set; }

        [ProtocolTransient]
        public bool PersonalPriceDiscount =>
            this.Personal && (this.PriceMultiplier < 1.0);

        [ProtocolTransient]
        public bool PersonalXCrystalBonus =>
            this.Personal && (this.AmountMultiplier > 1.0);
    }
}

