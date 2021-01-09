namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x15901507de9L)]
    public interface SaleEndNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        SaleEndNotificationTextComponent saleEndNotificationText();
    }
}

