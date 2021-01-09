namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15ce33947a0L)]
    public interface BattleLobbyTemplate : Template
    {
        [AutoAdded]
        BattleLobbyComponent battleLobby();
        BattleLobbyGroupComponent battleLobbyGroup();
    }
}

