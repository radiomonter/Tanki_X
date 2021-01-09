namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15bc95045aaL)]
    public interface BattleCountQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        BattleCountQuestComponent battleCountQuest();
    }
}

