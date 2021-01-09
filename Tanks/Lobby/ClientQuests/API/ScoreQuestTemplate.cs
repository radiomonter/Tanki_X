namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15bc950f9e0L)]
    public interface ScoreQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        ScoreQuestComponent scoreQuest();
    }
}

