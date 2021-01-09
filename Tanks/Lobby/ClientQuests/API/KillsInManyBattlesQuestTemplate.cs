namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15852db34e9L)]
    public interface KillsInManyBattlesQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        KillsInManyBattlesQuestComponent killsInManyBattlesQuest();
    }
}

