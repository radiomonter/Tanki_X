namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AdyenPublicKeyComponent : FromConfigBehaviour, Component
    {
        public override string ConfigPath =>
            "payment/provider/adyen";

        public string PublicKey { get; set; }
    }
}

