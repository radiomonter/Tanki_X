namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(FractionGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, Inherited=false)]
    public class JoinByFraction : Attribute
    {
    }
}

