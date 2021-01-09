namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15ee7644efaL)]
    public interface SquadTemplate : Template
    {
        [AutoAdded]
        SquadComponent squad();
        [AutoAdded, PersistentConfig("", false)]
        SquadConfigComponent squadConfig();
    }
}

