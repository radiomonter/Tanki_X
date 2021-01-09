namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x80d50dc534aL)]
    public interface LobbyLoadScreenTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LoadScreenLocalizedTextComponent loadScreenLocalizedText();
    }
}

