namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d52ce6a1c5884fL)]
    public class TargetItemFromDailyBonusReceivedEvent : Event
    {
        public long DetailMarketItemId { get; set; }
    }
}

