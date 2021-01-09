namespace Tanks.Lobby.ClientPayment.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    [SerialVersionUID(0x155915fd38eL)]
    public interface PaymentNotificationTemplate : NotificationTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        PaymentNotificationTextComponent paymentNotificationText();
    }
}

