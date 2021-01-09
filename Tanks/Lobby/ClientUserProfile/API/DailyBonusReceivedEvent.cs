namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d52753b22ee1b0L)]
    public class DailyBonusReceivedEvent : Event
    {
        public long Code { get; set; }
    }
}

