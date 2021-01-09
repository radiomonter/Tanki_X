namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d5268658680810L)]
    public interface DailyBonusConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        DailyBonusCommonConfigComponent dailyBonusCommonConfig();
        [AutoAdded, PersistentConfig("", false)]
        DailyBonusEndlessCycleComponent dailyBonusEndlessCycle();
        [AutoAdded, PersistentConfig("", false)]
        DailyBonusFirstCycleComponent dailyBonusFirstCycle();
    }
}

