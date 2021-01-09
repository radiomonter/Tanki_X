namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167d0b5b5d0L)]
    public class FinishedFractionsCompetitionComponent : Component
    {
        public Entity Winner { get; set; }
    }
}

