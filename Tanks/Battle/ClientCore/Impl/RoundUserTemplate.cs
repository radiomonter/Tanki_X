namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.API;

    [SerialVersionUID(0x1f2923959df5c98L)]
    public interface RoundUserTemplate : Template
    {
        BattleGroupComponent battleJoinComponent();
        RoundUserComponent roundUserComponent();
        RoundUserStatisticsComponent roundUserStatisticsComponent();
        UserGroupComponent userJoinComponent();
    }
}

