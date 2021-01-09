namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(MapGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, Inherited=false)]
    public class JoinByMap : Attribute
    {
    }
}

