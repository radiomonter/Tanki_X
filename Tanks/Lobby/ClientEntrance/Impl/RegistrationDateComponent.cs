namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14f1b2d9cc2L)]
    public class RegistrationDateComponent : Component
    {
        [ProtocolOptional]
        public Date RegistrationDate { get; set; }
    }
}

