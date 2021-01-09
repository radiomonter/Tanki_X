namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientCommunicator.API;

    [SerialVersionUID(0x15d1c79ac72L)]
    public interface BattleLobbyChatTemplate : ChatTemplate, Template
    {
        [AutoAdded]
        BattleLobbyChatComponent battleLobbyChat();
    }
}

