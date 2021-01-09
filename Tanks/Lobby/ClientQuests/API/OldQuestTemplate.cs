namespace Tanks.Lobby.ClientQuests.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;

    [SerialVersionUID(0x15ba49d5fe0L)]
    public interface OldQuestTemplate : BaseQuestTemplate, ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("order", false)]
        OrderItemComponent orderItem();
        [AutoAdded, PersistentConfig("", false)]
        QuestVariationsComponent questVariations();
        UserRankComponent userRank();
    }
}

