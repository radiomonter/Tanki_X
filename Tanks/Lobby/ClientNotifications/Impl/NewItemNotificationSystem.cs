namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class NewItemNotificationSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<NotificationWithGroupNode> <>f__am$cache0;

        [OnEventFire]
        public void CreateMessage(NodeAddedEvent e, NotificationWithoutGroupNode notification)
        {
            NotificationMessageComponent component = new NotificationMessageComponent {
                Message = notification.newItemNotificationText.HeaderText
            };
            notification.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void CreateMessages(ShowNotificationGroupEvent e, SingleNode<NotificationGroupComponent> userItem, [JoinBy(typeof(NotificationGroupComponent))] ICollection<NotificationWithGroupNode> notificatios)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (NotificationWithGroupNode notification) {
                    NotificationMessageComponent component = new NotificationMessageComponent {
                        Message = notification.newItemNotificationText.HeaderText
                    };
                    notification.Entity.AddComponent(component);
                };
            }
            notificatios.ForEach<NotificationWithGroupNode>(<>f__am$cache0);
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, ActiveNotificationNode notification)
        {
            base.NewEvent<UpdateNotificationEvent>().Attach(notification).Attach(notification.newItemNotification.Item).Schedule();
        }

        [OnEventFire]
        public void HideProfile(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<ProfileScreenComponent> profileScreen)
        {
            profileScreen.component.HideOnNewItemNotificationShow();
        }

        [OnEventFire]
        public void NewCardNotificationsClosed(CloseNotificationEvent e, ActiveCardNotificationNode cards, [JoinAll] SingleNode<MainScreenComponent> mainScreeen)
        {
            mainScreeen.component.CardsCount--;
            mainScreeen.component.NotificationsCount--;
            mainScreeen.component.HideNewItemNotification();
            TutorialCanvas.Instance.CardsNotificationsEnabled(false);
        }

        [OnEventFire]
        public void NewCardsNotificationsClosed(CloseNotificationEvent e, SingleNode<NewCardItemNotificationComponent> cardsNotification, [JoinAll, Combine] SingleNode<NewCardsNotificationClosedTriggerComponent> tutorialTriggerNode)
        {
            tutorialTriggerNode.component.GetComponent<TutorialShowTriggerComponent>().Triggered();
        }

        [OnEventFire]
        public void NewItemNotificationsClosed(CloseNotificationEvent e, ActiveItemNotificationNode cards, [JoinAll] SingleNode<MainScreenComponent> mainScreeen)
        {
            mainScreeen.component.NotificationsCount--;
            mainScreeen.component.CardsCount--;
            mainScreeen.component.HideNewItemNotification();
            TutorialCanvas.Instance.CardsNotificationsEnabled(false);
        }

        [OnEventFire]
        public void NewNotificationsOpened(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<MainScreenComponent> mainScreeen)
        {
            mainScreeen.component.CardsCount++;
            mainScreeen.component.NotificationsCount++;
            mainScreeen.component.ShowCardsNotification(true);
            TutorialCanvas.Instance.CardsNotificationsEnabled(true);
        }

        [OnEventFire]
        public void RegisterNode(NodeAddedEvent e, MountedChildWithImageNode notification)
        {
        }

        private void SetImageOrIcon(ActiveNotificationNode notification, ItemNode item)
        {
            bool flag = false;
            string spriteUid = null;
            if (item.Entity.HasComponent<ImageItemComponent>())
            {
                spriteUid = item.Entity.GetComponent<ImageItemComponent>().SpriteUid;
            }
            if (spriteUid == null)
            {
                MountedChildWithImageNode node = base.Select<MountedChildWithImageNode>(item.Entity, typeof(ParentGroupComponent)).FirstOrDefault<MountedChildWithImageNode>();
                if (node != null)
                {
                    spriteUid = node.imageItem.SpriteUid;
                }
            }
            if ((spriteUid == null) && item.Entity.HasComponent<ItemIconComponent>())
            {
                flag = true;
                spriteUid = item.Entity.GetComponent<ItemIconComponent>().SpriteUid;
            }
            if (!string.IsNullOrEmpty(spriteUid))
            {
                if (!flag)
                {
                    notification.newItemNotificationUnity.SetItemImage(spriteUid);
                }
                else
                {
                    notification.newItemNotificationUnity.SetItemIcon(spriteUid);
                }
            }
        }

        [OnEventFire]
        public void ShowProfile(CloseNotificationEvent e, ActiveItemNotificationNode notification, [JoinAll] SingleNode<ProfileScreenComponent> profileScreen)
        {
            profileScreen.component.ShowOnNewItemNotificationCLose();
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationEvent e, ActiveItemNotificationNode notification, ItemNode item)
        {
            Node[] nodes = new Node[] { notification, item };
            base.NewEvent<UpdateNotificationItemNameEvent>().AttachAll(nodes).Schedule();
            notification.newItemNotificationUnity.HeaderElement.text = notification.newItemNotificationText.HeaderText;
            this.SetImageOrIcon(notification, item);
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationItemNameEvent e, ActiveItemNotificationNode notification, NotPaintItemNode item)
        {
            string singleItemText = notification.newItemNotificationText.SingleItemText;
            if (notification.newItemNotification.Amount > 1)
            {
                singleItemText = notification.newItemNotificationText.ItemText;
            }
            notification.newItemNotificationUnity.ItemNameElement.text = string.Format(singleItemText, item.descriptionItem.Name, notification.newItemNotification.Amount);
            notification.newItemNotificationUnity.SetItemRarity(GarageItemsRegistry.GetItem<GarageItem>(item.Entity));
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationItemNameEvent e, ActiveItemNotificationNode notification, PaintItemNode item)
        {
            string str = !item.Entity.HasComponent<TankPaintItemComponent>() ? notification.newPaintItemNotificationText.CoverText : notification.newPaintItemNotificationText.PaintText;
            string singleItemText = notification.newItemNotificationText.SingleItemText;
            notification.newItemNotificationUnity.ItemNameElement.text = string.Format(singleItemText, item.descriptionItem.Name + $" ({str})", notification.newItemNotification.Amount);
            notification.newItemNotificationUnity.SetItemRarity(GarageItemsRegistry.GetItem<GarageItem>(item.Entity));
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationEvent e, ActiveCardNotificationNode notification, ItemNode item, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard, [JoinByParentGroup] Optional<NotUserModuleNode> notUserModule, [JoinByParentGroup, Combine] Optional<UserModuleNode> userModule, [JoinAll] SelfUserNode selfUser)
        {
            int num = 0;
            int cards = 0;
            notification.newItemNotificationUnity.ContainerContent = false;
            if (!moduleCard.IsPresent())
            {
                notification.newItemNotificationUnity.view.UpdateViewForCrystal(item.Entity.GetComponent<CardImageItemComponent>().SpriteUid, item.descriptionItem.Name);
                notification.newItemNotificationUnity.upgradeAnimator.Maximum = num;
                notification.newItemNotificationUnity.upgradeAnimator.StartValue = num - notification.newItemNotification.Amount;
                notification.newItemNotificationUnity.upgradeAnimator.Price = cards;
                notification.newItemNotificationUnity.count = notification.newItemNotification.Amount;
            }
            else
            {
                num = !moduleCard.IsPresent() ? 0 : ((int) moduleCard.Get().userItemCounter.Count);
                notification.newItemNotificationUnity.view.UpdateView(notUserModule.Get().Entity.Id, -1L, true, true);
                if (notUserModule.IsPresent() && !userModule.IsPresent())
                {
                    cards = notUserModule.Get().moduleCardsComposition.CraftPrice.Cards;
                }
                if (userModule.IsPresent())
                {
                    long num3 = userModule.Get().moduleUpgradeLevel.Level + 1L;
                    cards = (num3 > userModule.Get().moduleCardsComposition.UpgradePrices.Count) ? 0 : userModule.Get().moduleCardsComposition.UpgradePrices[(int) (num3 - 1L)].Cards;
                }
                notification.newItemNotificationUnity.upgradeAnimator.Maximum = num;
                notification.newItemNotificationUnity.upgradeAnimator.StartValue = num - notification.newItemNotification.Amount;
                notification.newItemNotificationUnity.upgradeAnimator.Price = cards;
                notification.newItemNotificationUnity.count = notification.newItemNotification.Amount;
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class ActiveCardNotificationNode : NewItemNotificationSystem.ActiveNotificationNode
        {
            public NewCardItemNotificationComponent newCardItemNotification;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class ActiveItemNotificationNode : NewItemNotificationSystem.ActiveNotificationNode
        {
        }

        public class ActiveNotificationNode : NewItemNotificationSystem.NotificationNode
        {
            public ActiveNotificationComponent activeNotification;
            public NewItemNotificationUnityComponent newItemNotificationUnity;
        }

        public class InstantiatedNewItemNotification : Node
        {
            public NewItemNotificationTextComponent newItemNotificationText;
            public NotificationInstanceComponent notificationInstance;
        }

        public class ItemNode : Node
        {
            public GarageItemComponent garageItem;
            public DescriptionItemComponent descriptionItem;
        }

        public class ModuleCardNode : Node
        {
            public ModuleCardItemComponent moduleCardItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class ModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public MarketItemGroupComponent marketItemGroup;
            public ModuleBehaviourTypeComponent moduleBehaviourType;
            public ModuleTierComponent moduleTier;
            public ModuleTankPartComponent moduleTankPart;
            public DescriptionItemComponent descriptionItem;
            public ItemIconComponent itemIcon;
            public ItemBigIconComponent itemBigIcon;
            public OrderItemComponent orderItem;
            public ModuleCardsCompositionComponent moduleCardsComposition;
        }

        public class MountedChildWithImageNode : Node
        {
            public ImageItemComponent imageItem;
            public MountedItemComponent mountedItem;
            public SkinItemComponent skinItem;
        }

        public class NotificationNode : Node
        {
            public NewItemNotificationComponent newItemNotification;
            public NewItemNotificationTextComponent newItemNotificationText;
            public NewPaintItemNotificationTextComponent newPaintItemNotificationText;
        }

        public class NotificationWithGroupNode : NewItemNotificationSystem.NotificationNode
        {
            public NotificationGroupComponent notificationGroup;
        }

        [Not(typeof(NotificationGroupComponent))]
        public class NotificationWithoutGroupNode : NewItemNotificationSystem.NotificationNode
        {
        }

        [Not(typeof(PaintItemComponent))]
        public class NotPaintItemNode : NewItemNotificationSystem.ItemNode
        {
        }

        [Not(typeof(UserItemComponent))]
        public class NotUserModuleNode : NewItemNotificationSystem.ModuleNode
        {
        }

        public class PaintItemNode : NewItemNotificationSystem.ItemNode
        {
            public PaintItemComponent paintItem;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class UpdateNotificationEvent : Event
        {
        }

        public class UpdateNotificationItemNameEvent : Event
        {
        }

        public class UserModuleNode : NewItemNotificationSystem.ModuleNode
        {
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
            public ModuleGroupComponent moduleGroup;
            public UserGroupComponent userGroup;
        }
    }
}

