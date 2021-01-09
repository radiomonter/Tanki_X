namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15f6bb308dcL)]
    public interface TutorialRewardDataTemplate : TutorialStepDataTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        TutorialRewardDataComponent tutorialRewardData();
    }
}

