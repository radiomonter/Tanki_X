namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15bc9516007L)]
    public interface WinQuestTemplate : QuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        WinQuestComponent winQuest();
    }
}

