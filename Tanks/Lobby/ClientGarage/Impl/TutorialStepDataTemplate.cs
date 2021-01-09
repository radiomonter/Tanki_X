namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15e2d8e54ceL)]
    public interface TutorialStepDataTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialStepDataComponent tutorialStepData();
    }
}

