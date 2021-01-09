namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15e57050680L)]
    public interface TutorialSelectItemDataTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialSelectItemDataComponent tutorialSelectItemData();
        [AutoAdded, PersistentConfig("", false)]
        TutorialStepDataComponent tutorialStepData();
    }
}

