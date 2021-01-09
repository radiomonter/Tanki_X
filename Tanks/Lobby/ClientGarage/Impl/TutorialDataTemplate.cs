namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15e2d639e9fL)]
    public interface TutorialDataTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialDataComponent tutorialData();
        [AutoAdded]
        TutorialGroupComponent tutorialGroup();
        [AutoAdded, PersistentConfig("", false)]
        TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials();
    }
}

