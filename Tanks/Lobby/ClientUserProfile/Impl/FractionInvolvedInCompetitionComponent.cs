namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167bf880a39L)]
    public class FractionInvolvedInCompetitionComponent : Component
    {
        public long UserCount { get; set; }
    }
}

