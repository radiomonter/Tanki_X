namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x56da1b782be47a45L)]
    public class TeamColorComponent : Component
    {
        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }
    }
}

