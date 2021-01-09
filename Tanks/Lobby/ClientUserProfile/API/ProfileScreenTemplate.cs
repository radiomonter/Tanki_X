namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1525f1de367L)]
    public interface ProfileScreenTemplate : Template
    {
        [PersistentConfig("", false)]
        ProfileScreenLocalizationComponent profileScreenLocalization();
    }
}

