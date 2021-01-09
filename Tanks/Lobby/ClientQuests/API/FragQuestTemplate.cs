namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15bc950cb17L)]
    public interface FragQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        FragQuestComponent fragQuest();
    }
}

