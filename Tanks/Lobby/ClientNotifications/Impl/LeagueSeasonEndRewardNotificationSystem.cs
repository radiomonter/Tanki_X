namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class LeagueSeasonEndRewardNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, [Combine] LeagueSeasonEndRewardNotificationNode notification, EndSeasonDialogNode popup, [JoinAll] UserWithLeagueNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            PopupDialogComponent popupDialog = popup.popupDialog;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(notification.leagueSeasonEndRewardNotification.LeagueId);
            popupDialog.itemsContainer.DestroyChildren();
            popupDialog.leagueIcon.SpriteUid = entity.GetComponent<LeagueIconComponent>().SpriteUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            int num = 0;
            foreach (KeyValuePair<long, int> pair in notification.leagueSeasonEndRewardNotification.Reward)
            {
                popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent component2 = Object.Instantiate<LeagueEntranceItemComponent>(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entity2 = Flow.Current.EntityRegistry.GetEntity(pair.Key);
                int num2 = pair.Value;
                component2.preview.SpriteUid = entity2.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = num2 > 1;
                component2.text.text = entity2.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
                component2.gameObject.SetActive(true);
                if (flag)
                {
                    component2.count.Value = num2;
                }
                else
                {
                    component2.count.gameObject.SetActive(false);
                }
                num++;
            }
            popupDialog.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<LeagueEntrancePopupCloseButtonCompoent> button, [JoinAll, Combine] LeagueSeasonEndRewardNotificationNode notification, [JoinAll] EndSeasonDialogNode popup)
        {
            popup.popupDialog.Hide();
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class EndSeasonDialogNode : Node
        {
            public EndSeasonPopupDialogComponent endSeasonPopupDialog;
            public PopupDialogComponent popupDialog;
        }

        public class LeagueSeasonEndRewardNotificationNode : Node
        {
            public LeagueSeasonEndRewardNotificationComponent leagueSeasonEndRewardNotification;
            public ResourceDataComponent resourceData;
        }

        public class UserWithLeagueNode : Node
        {
            public SelfUserComponent selfUser;
            public UserReputationComponent userReputation;
            public LeagueGroupComponent leagueGroup;
        }
    }
}

