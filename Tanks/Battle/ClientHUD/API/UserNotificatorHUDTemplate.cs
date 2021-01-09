namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x8d3f107ef385b3dL)]
    public interface UserNotificatorHUDTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        UserNotificatorHUDTextComponent userNotificatorHUDText();
    }
}

