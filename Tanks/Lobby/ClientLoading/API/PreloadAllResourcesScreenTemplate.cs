namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x2216c611737L)]
    public interface PreloadAllResourcesScreenTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LoadScreenLocalizedTextComponent loadScreenLocalizedText();
    }
}

