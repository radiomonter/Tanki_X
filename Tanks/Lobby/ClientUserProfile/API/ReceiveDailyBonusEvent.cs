namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d52752e4ed26ccL)]
    public class ReceiveDailyBonusEvent : Event
    {
        public long Code { get; set; }
    }
}

