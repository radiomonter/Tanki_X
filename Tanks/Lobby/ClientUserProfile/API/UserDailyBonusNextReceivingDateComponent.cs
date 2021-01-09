namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d52b621b64d877L)]
    public class UserDailyBonusNextReceivingDateComponent : Component
    {
        [ProtocolOptional]
        public Platform.Library.ClientUnityIntegration.API.Date Date { get; set; }

        public long TotalMillisLength { get; set; }
    }
}

