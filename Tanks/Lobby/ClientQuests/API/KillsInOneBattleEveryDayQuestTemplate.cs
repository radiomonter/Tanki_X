namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x157d2b1ae8eL)]
    public interface KillsInOneBattleEveryDayQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest();
    }
}

