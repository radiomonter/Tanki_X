namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e946b032dL)]
    public class SeasonEndDateComponent : Component
    {
        [ProtocolOptional]
        public Date EndDate { get; set; }
    }
}

