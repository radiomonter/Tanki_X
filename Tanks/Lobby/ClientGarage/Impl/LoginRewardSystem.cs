namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNotifications.API;
    using UnityEngine;

    public class LoginRewardSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, SingleNode<LoginRewardDialog> popup, [Combine] LoginRewardsNotificationNode notification, [JoinAll] UserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            int currentDay = notification.loginRewardsNotification.CurrentDay;
            LoginRewardDialog dialog = popup.component;
            if (dialog.allItems.currentDay <= currentDay)
            {
                dialog.allItems.Clear();
                int num2 = 0;
                foreach (KeyValuePair<long, int> pair in notification.loginRewardsNotification.Reward)
                {
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(pair.Key);
                    if (!entity.HasComponent<PremiumQuestItemComponent>())
                    {
                        dialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num2 + 1) * dialog.itemsShowDelay;
                        ReleaseGiftItemComponent component = Object.Instantiate<ReleaseGiftItemComponent>(dialog.itemPrefab, dialog.itemsContainer, false);
                        dialog.marketItems.Add(entity);
                        int num3 = pair.Value;
                        component.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                        bool flag = num3 > 1;
                        component.text.text = dialog.GetRewardItemName(entity);
                        if (entity.HasComponent<PremiumBoostItemComponent>())
                        {
                            component.text.text = string.Format(component.text.text, num3);
                            flag = false;
                        }
                        component.gameObject.SetActive(true);
                        if (flag)
                        {
                            component.count.Value = num3;
                        }
                        else
                        {
                            component.count.gameObject.SetActive(false);
                        }
                        num2++;
                    }
                }
                Dictionary<int, List<LoginRewardItem>> allRewards = new Dictionary<int, List<LoginRewardItem>>();
                foreach (LoginRewardItem item in notification.loginRewardsNotification.AllReward)
                {
                    if (allRewards.ContainsKey(item.Day))
                    {
                        allRewards[item.Day].Add(item);
                        continue;
                    }
                    List<LoginRewardItem> list = new List<LoginRewardItem> {
                        item
                    };
                    allRewards[item.Day] = list;
                }
                popup.component.allItems.InitItems(allRewards, currentDay);
                dialog.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
            }
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<LoginRewardAcceptButton> button, [JoinAll] ICollection<LoginRewardsNotificationNode> notifications, [JoinAll] SingleNode<LoginRewardDialog> popup)
        {
            popup.component.Hide();
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent();
            foreach (Entity entity in popup.component.marketItems)
            {
                if (entity.HasComponent<ContainerMarkerComponent>())
                {
                    eventInstance.Category = !entity.HasComponent<GameplayChestItemComponent>() ? GarageCategory.CONTAINERS : GarageCategory.BLUEPRINTS;
                    eventInstance.SelectedItem = entity;
                    base.ScheduleEvent(eventInstance, entity);
                }
                else if (entity.HasComponent<WeaponPaintItemComponent>())
                {
                    eventInstance.Category = GarageCategory.PAINTS;
                    eventInstance.SelectedItem = entity;
                    base.ScheduleEvent(eventInstance, entity);
                }
                else
                {
                    if (!entity.HasComponent<PaintItemComponent>())
                    {
                        continue;
                    }
                    eventInstance.Category = GarageCategory.PAINTS;
                    eventInstance.SelectedItem = entity;
                    base.ScheduleEvent(eventInstance, entity);
                }
                break;
            }
            foreach (LoginRewardsNotificationNode node in notifications)
            {
                base.ScheduleEvent<NotificationShownEvent>(node);
            }
        }

        public class LoginRewardsNotificationNode : Node
        {
            public LoginRewardsNotificationComponent loginRewardsNotification;
            public ResourceDataComponent resourceData;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

