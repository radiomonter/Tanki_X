namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(ScoreTableGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinByScoreTable : Attribute
    {
    }
}

