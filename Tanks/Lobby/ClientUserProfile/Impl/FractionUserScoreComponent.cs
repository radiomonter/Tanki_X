namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167a0bde773L)]
    public class FractionUserScoreComponent : Component
    {
        public long TotalEarnedPoints { get; set; }
    }
}

