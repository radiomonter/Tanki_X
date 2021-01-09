namespace Tanks.Lobby.ClientUserProfile.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;

    [TemplatePart]
    public interface ClientSessionTemplatePart : ClientSessionTemplate, Template
    {
        ClientLocaleComponent clientLocale();
    }
}

