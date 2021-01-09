namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15de54a4da7L)]
    public interface LeaguesConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CurrentSeasonNameComponent currentSeasonName();
        CurrentSeasonNumberComponent currentSeasonNumber();
        SeasonEndDateComponent seasonEndDate();
    }
}

