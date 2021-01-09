namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15e3cb57805L)]
    public interface TutorialHighlightTankDataTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialHighlightTankStepDataComponent tutorialHighlightTankStepData();
        [AutoAdded, PersistentConfig("", false)]
        TutorialStepDataComponent tutorialStepData();
    }
}

