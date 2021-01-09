namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x5a14058161a040e6L)]
    public interface TeamBattleTemplate : BattleTemplate, Template
    {
        TeamBattleComponent teamBattle();
    }
}

