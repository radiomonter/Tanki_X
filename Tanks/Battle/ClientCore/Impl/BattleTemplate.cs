namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(-943942723589794079L)]
    public interface BattleTemplate : Template
    {
        BattleComponent battle();
        BattleGroupComponent battleJoin();
        [PersistentConfig("", false), AutoAdded]
        BonusClientConfigPrefabComponent bonusClientConfigPrefab();
        ScoreLimitComponent scoreLimit();
        TimeLimitComponent timeLimit();
        UserLimitComponent userLimit();
    }
}

