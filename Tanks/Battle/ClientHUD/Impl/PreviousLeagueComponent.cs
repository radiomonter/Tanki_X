namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PreviousLeagueComponent : Component
    {
        public Entity PreviousLeague { get; set; }
    }
}

