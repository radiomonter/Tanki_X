namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(ParentGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinByParentGroup : Attribute
    {
    }
}

