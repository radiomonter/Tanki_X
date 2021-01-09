namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(LeagueGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, Inherited=false)]
    public class JoinByLeague : Attribute
    {
    }
}

