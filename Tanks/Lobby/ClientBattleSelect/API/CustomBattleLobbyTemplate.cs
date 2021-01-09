namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15ce33b91c9L)]
    public interface CustomBattleLobbyTemplate : BattleLobbyTemplate, Template
    {
        [AutoAdded]
        CustomBattleLobbyComponent customBattleLobby();
    }
}

