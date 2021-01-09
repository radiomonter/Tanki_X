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

    public class LeagueFirstEntranceRewardNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, LeagueFirstEntranceRewardNotificationNode notification, LeagueDialogNode popup, [JoinAll] UserWithLeagueNode user, [JoinByLeague] LeagueNode league, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.itemsContainer.DestroyChildren();
            popupDialog.leagueIcon.SpriteUid = league.leagueIcon.SpriteUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.headerText.text = league.leagueEnterNotificationTexts.HeaderText;
            popupDialog.text.text = league.leagueEnterNotificationTexts.Text;
            int num = 0;
            foreach (KeyValuePair<long, int> pair in notification.leagueFirstEntranceRewardNotification.Reward)
            {
                popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent component2 = Object.Instantiate<LeagueEntranceItemComponent>(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(pair.Key);
                int num2 = pair.Value;
                component2.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = num2 > 1;
                component2.text.text = entity.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
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
        public void HidePopup(ButtonClickEvent e, SingleNode<LeagueEntrancePopupCloseButtonCompoent> button, [JoinAll, Combine] LeagueFirstEntranceRewardNotificationNode notification, [JoinAll] LeagueDialogNode popup)
        {
            popup.popupDialog.Hide();
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, LeagueFirstEntranceRewardNotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class LeagueDialogNode : Node
        {
            public LeaguePopupDialogComponent leaguePopupDialog;
            public PopupDialogComponent popupDialog;
        }

        public class LeagueFirstEntranceRewardNotificationNode : Node
        {
            public LeagueFirstEntranceRewardNotificationComponent leagueFirstEntranceRewardNotification;
            public ResourceDataComponent resourceData;
        }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueNameComponent leagueName;
            public LeagueIconComponent leagueIcon;
            public LeagueConfigComponent leagueConfig;
            public ChestBattleRewardComponent chestBattleReward;
            public CurrentSeasonRewardForClientComponent currentSeasonRewardForClient;
            public LeagueEnterNotificationTextsComponent leagueEnterNotificationTexts;
        }

        public class UserWithLeagueNode : Node
        {
            public SelfUserComponent selfUser;
            public UserReputationComponent userReputation;
            public LeagueGroupComponent leagueGroup;
        }
    }
}

