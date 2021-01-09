namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(TeamGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinByTeam : Attribute
    {
    }
}

