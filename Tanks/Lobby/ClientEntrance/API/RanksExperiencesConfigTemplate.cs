namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1523ae67b8bL)]
    public interface RanksExperiencesConfigTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        RanksExperiencesConfigComponent ranksExperiencesConfig();
    }
}

