namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15ba976f75fL)]
    public interface SimpleTextNotificationTemplate : NotificationTemplate, Template
    {
        ServerNotificationMessageComponent serverNotificationMessage();
    }
}

