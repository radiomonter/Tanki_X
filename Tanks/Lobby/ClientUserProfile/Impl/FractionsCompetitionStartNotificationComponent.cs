namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167a6aa836dL)]
    public class FractionsCompetitionStartNotificationComponent : Component
    {
        public long[] FractionsInCompetition { get; set; }
    }
}

