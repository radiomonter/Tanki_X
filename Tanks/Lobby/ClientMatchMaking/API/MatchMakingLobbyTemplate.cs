namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    [SerialVersionUID(0x15c35333577L)]
    public interface MatchMakingLobbyTemplate : BattleLobbyTemplate, Template
    {
    }
}

