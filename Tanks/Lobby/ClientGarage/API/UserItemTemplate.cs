namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d287a21af16576L)]
    public interface UserItemTemplate : Template
    {
        [AutoAdded]
        UserItemComponent userItem();
    }
}

