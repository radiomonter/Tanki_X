namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x159f35590dbL)]
    public interface SlotUserItemTemplate : UserItemTemplate, Template
    {
        [AutoAdded]
        SlotItemComponent slotItem();
    }
}

