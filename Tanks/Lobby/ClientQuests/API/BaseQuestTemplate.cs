namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientQuests.Impl;

    [SerialVersionUID(0x15751362521L)]
    public interface BaseQuestTemplate : ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        QuestConditionDescriptionComponent questConditionDescription();
        QuestProgressComponent questProgress();
    }
}

