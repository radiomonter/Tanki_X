namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x1610e44e423L)]
    public interface BattleQuestTemplate : Template
    {
        [AutoAdded]
        BattleQuestComponent battleQuest();
        BattleQuestRewardComponent battleQuestReward();
        BattleQuestTargetComponent battleQuestTarget();
        [AutoAdded, PersistentConfig("", false)]
        DescriptionItemComponent descriptionItem();
        [AutoAdded, PersistentConfig("", false)]
        ImageItemComponent imageItem();
    }
}

