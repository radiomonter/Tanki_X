namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x154a8a3342eL), Shared]
    public class ServerShutdownComponent : Component
    {
        public Date StartDate { get; set; }

        public Date StopDateForClient { get; set; }
    }
}

