namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15e2d3bb646L)]
    public interface TutorialsConfigurationTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialConfigurationComponent tutorialConfiguration();
    }
}

