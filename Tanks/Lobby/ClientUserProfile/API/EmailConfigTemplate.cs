namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    [SerialVersionUID(0x153171e0e0cL)]
    public interface EmailConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        EmailConfirmationCodeConfigComponent emailConfirmationCodeConfig();
    }
}

