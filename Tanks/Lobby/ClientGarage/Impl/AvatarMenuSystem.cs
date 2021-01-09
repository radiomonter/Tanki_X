namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientHangar.Impl;

    public class AvatarMenuSystem : ECSSystem
    {
        [OnEventFire]
        public void AddAvatar(NodeAddedEvent e, ICollection<AvatarMarketItemNode> marketItem, [Context, JoinAll] SelfUserNode user, [Context, JoinAll] SingleNode<AvatarUIComponent> uiNode)
        {
            List<long> avatarIds = new List<long>();
            foreach (AvatarMarketItemNode node in marketItem)
            {
                avatarIds.Add(node.Entity.Id);
            }
            uiNode.component.AddMarketItem(avatarIds, user.userRank.Rank);
        }

        [OnEventComplete]
        public void AddAvatar(NodeAddedEvent e, [Combine] AvatarMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] AvatarUserItemNode avatarNode, [JoinByUser] SelfUserNode user, [Context, JoinAll] SingleNode<AvatarUIComponent> uiNode)
        {
            uiNode.component.AddUserItem(avatarNode.marketItemGroup.Key, user.userRank.Rank);
        }

        [OnEventComplete]
        public void CheckAvatars(NodeAddedEvent e, SingleNode<AvatarUIComponent> uiNode)
        {
            uiNode.component.UpdateAvatars();
        }

        [OnEventFire]
        public void GoBackByKey(UpdateEvent e, SingleNode<MainScreenComponent> node, [JoinAll] MountedAvatarNode mountedAvatar, [JoinAll, Combine] SingleNode<SelfUserAvatarComponent> selfAvatar, [JoinAll] Optional<SingleNode<AvatarUIComponent>> avatarsUi)
        {
            if (InputMapping.Cancel)
            {
                selfAvatar.component.SetAvatar(mountedAvatar.avatarItem.Id);
                if (avatarsUi.IsPresent())
                {
                    avatarsUi.Get().component.OnSelect(mountedAvatar.marketItemGroup.Key);
                }
            }
        }

        [OnEventFire]
        public void OnClose(NodeRemoveEvent e, SingleNode<AvatarUIComponent> ui)
        {
            base.ScheduleEvent<ResetPreviewEvent>(ui);
        }

        [OnEventComplete]
        public void OnMountAvatar(NodeAddedEvent e, MountedAvatarNode mountedAvatar, [Context] SingleNode<AvatarUIComponent> avatarsUi)
        {
            avatarsUi.component.OnEquip(mountedAvatar.marketItemGroup.Key);
        }

        [OnEventComplete]
        public void OnMountAvatar(NodeAddedEvent e, MountedAvatarNode mountedAvatar, [JoinByUser, Context] SelfUserNode user, [Combine, Context] SingleNode<SelfUserAvatarComponent> selfAvatar)
        {
            selfAvatar.component.SetAvatar(mountedAvatar.avatarItem.Id);
        }

        [OnEventComplete]
        public void OnPreviewAvatar(ItemPreviewBaseSystem.PrewievEvent e, AvatarItemNode avatar, [Combine, JoinAll] SingleNode<SelfUserAvatarComponent> selfAvatar, [JoinAll] Optional<SingleNode<AvatarUIComponent>> avatarsUi)
        {
            selfAvatar.component.SetAvatar(avatar.avatarItem.Id);
            if (avatarsUi.IsPresent())
            {
                avatarsUi.Get().component.OnSelect(avatar.marketItemGroup.Key);
            }
        }

        [OnEventComplete]
        public void OnResetPreview(ResetPreviewEvent e, Node node, [JoinAll] MountedAvatarNode mountedAvatar, [JoinAll, Combine] SingleNode<SelfUserAvatarComponent> selfAvatar, [JoinAll] Optional<SingleNode<AvatarUIComponent>> avatarsUi)
        {
            selfAvatar.component.SetAvatar(mountedAvatar.avatarItem.Id);
            if (avatarsUi.IsPresent())
            {
                avatarsUi.Get().component.OnSelect(mountedAvatar.marketItemGroup.Key);
            }
        }

        [OnEventFire]
        public void OnShowGarageCategory(ShowGarageCategoryEvent e, Node any, [JoinAll] SingleNode<AvatarDialogComponent> ui)
        {
            ui.component.Hide();
        }

        [OnEventComplete]
        public void PriceChanged(PriceChangedEvent e, BuyableAvatarMarketItemNode item, [JoinAll] SingleNode<AvatarUIComponent> uiNode)
        {
            uiNode.component.UpdatePrice(item.marketItemGroup.Key);
        }

        [OnEventFire]
        public void RemoveAvatar(NodeRemoveEvent e, [Combine] AvatarMarketItemNode marketItem, [Context, JoinAll] SingleNode<AvatarUIComponent> uiNode)
        {
            uiNode.component.Remove(marketItem.marketItemGroup.Key);
        }

        [OnEventFire]
        public void ShowMenu(ShowMenuEvent e, Node stubNode, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<AvatarUIComponent>> avatarsUi)
        {
            if (avatarsUi.IsPresent())
            {
                avatarsUi.Get().component.SortAvatars();
            }
            else
            {
                dialogs.component.Get<AvatarDialogComponent>().Show(null);
            }
        }

        [OnEventComplete]
        public void UpdateRank(NodeAddedEvent e, [Context] Rank rank, [Context] SingleNode<AvatarUIComponent> avatarUi)
        {
            avatarUi.component.UpdateRank(rank.userRank.Rank);
        }

        [OnEventFire]
        public void UpdateRank(UpdateRankEvent e, Rank rank, [JoinAll] SingleNode<AvatarUIComponent> avatarUi)
        {
            avatarUi.component.UpdateRank(rank.userRank.Rank);
        }

        public class AvatarItemNode : Node
        {
            public AvatarItemComponent avatarItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class AvatarMarketItemNode : AvatarMenuSystem.AvatarItemNode
        {
            public MarketItemComponent marketItem;
            public ItemRarityComponent itemRarity;
        }

        public class AvatarUserItemNode : AvatarMenuSystem.AvatarItemNode
        {
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
        }

        public class BuyableAvatarMarketItemNode : AvatarMenuSystem.AvatarMarketItemNode
        {
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
        }

        public class MountedAvatarNode : AvatarMenuSystem.AvatarUserItemNode
        {
            public MountedItemComponent mountedItem;
        }

        public class Rank : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserRankComponent userRank;
        }

        public class ShowMenuEvent : Event
        {
        }
    }
}

