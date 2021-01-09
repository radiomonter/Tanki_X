namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x150f01cf6c0L)]
    public interface ChatTemplate : Template
    {
        ChatComponent Chat();
        [AutoAdded, PersistentConfig("", false)]
        ChatConfigComponent chatConfig();
    }
}

