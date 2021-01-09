namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1655ffd52ffL)]
    public interface FractionsCompetitionTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        FractionsCompetitionInfoComponent fractionCompetitionInfo();
        [AutoAdded]
        FractionsCompetitionScoresComponent fractionCompetitionScores();
    }
}

