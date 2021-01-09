namespace Tanks.Battle.ClientCore.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(-2043703779834243389L)]
    public interface BattleUserTemplate : Template
    {
        BattleGroupComponent battleJoin();
        BattleUserComponent battleUser();
        [AutoAdded]
        IdleBeginTimeComponent idleBeginTime();
        IdleCounterComponent idleCounter();
        [AutoAdded]
        IdleKickCheckLastTimeComponent IdleKickCheckLastTime();
        [PersistentConfig("", false), AutoAdded]
        IdleKickConfigComponent idleKickConfig();
        [AutoAdded]
        MouseControlStateHolderComponent mouseControlStateHolder();
        SelfBattleUserComponent selfBattleUser();
        [PersistentConfig("", false), AutoAdded]
        SelfDestructionConfigComponent selfDestructionConfig();
        [PersistentConfig("", false), AutoAdded]
        UpsideDownConfigComponent upsideDownConfig();
        UserInBattleAsSpectatorComponent userInBattleAsSpectator();
        UserInBattleAsTankComponent userInBattleAsTank();
        UserGroupComponent userJoin();
    }
}

