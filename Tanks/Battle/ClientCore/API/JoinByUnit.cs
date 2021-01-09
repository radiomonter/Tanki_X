namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    [JoinBy(typeof(UnitGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinByUnit : Attribute
    {
    }
}

