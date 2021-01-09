namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d342a98aa2a384L)]
    public interface ItemImagedTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        ImageItemComponent imageItem();
    }
}

