namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d286f23dfc71a3L)]
    public interface LocalizedTextTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        LocalizedTextComponent localizedText();
    }
}

