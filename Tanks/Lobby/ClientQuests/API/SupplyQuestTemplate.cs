namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15bc9512c5aL)]
    public interface SupplyQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        SupplyQuestComponent supplyQuest();
    }
}

