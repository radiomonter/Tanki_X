namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d52cd4ef4538aaL)]
    public class ReceiveTargetItemFromDetailsByDailyBonusEvent : Event
    {
        public long DetailMarketItemId { get; set; }
    }
}

