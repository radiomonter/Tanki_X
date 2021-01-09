namespace Tanks.Lobby.ClientUserProfile.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;

    [TemplatePart]
    public interface UserLeagueTemplatePart : UserTemplate, Template
    {
        [AutoAdded]
        UserLeaguePlaceComponent userLeaguePlace();
    }
}

