namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientMatchMaking.Impl;

    [SerialVersionUID(0x15c8b1d6dfdL)]
    public interface MatchMakingModeTemplate : ItemImagedTemplate, Template
    {
        [AutoAdded, PersistentConfig("", false)]
        DescriptionItemComponent descriptionItem();
        [AutoAdded]
        MatchMakingModeComponent matchMakingMode();
        [AutoAdded, PersistentConfig("", false)]
        MatchMakingModeActivationComponent matchMakingModeActivation();
        [AutoAdded, PersistentConfig("", false)]
        MatchMakingModeRestrictionsComponent matchMakingModeRestrictions();
        [AutoAdded, PersistentConfig("order", false)]
        OrderItemComponent OrderItem();
    }
}

