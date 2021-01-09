namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    [SerialVersionUID(0x152b0499ebdL)]
    public interface NotificationTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        AssetReferenceComponent assetReference();
        [AutoAdded]
        AssetRequestComponent assetRequest();
        NotificationComponent notification();
        [AutoAdded, PersistentConfig("", false)]
        NotificationConfigComponent notificationConfig();
        NotificationInstanceComponent notificationInstance();
    }
}

