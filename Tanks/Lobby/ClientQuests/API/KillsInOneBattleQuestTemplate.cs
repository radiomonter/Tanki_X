namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x157acc764b4L)]
    public interface KillsInOneBattleQuestTemplate : OldQuestTemplate, BaseQuestTemplate, ItemImagedTemplate, Template
    {
        KillsInOneBattleQuestComponent killsInOneBattleQuest();
    }
}

