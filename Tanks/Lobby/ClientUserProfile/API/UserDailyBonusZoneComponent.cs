namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d5282515ec1348L)]
    public class UserDailyBonusZoneComponent : Component
    {
        public long ZoneNumber { get; set; }
    }
}

