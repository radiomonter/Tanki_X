namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class UserRankRewardNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateUserRankRewardNotification(NodeAddedEvent e, NotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        [OnEventFire]
        public void CreateUserRankRewardNotification(NodeAddedEvent e, RanksNamesNode ranksNames, [Combine] InstantiatedNotificationNode notification, [Context, JoinBy(typeof(NotificationsGroupComponent))] UserNode user)
        {
            UserRankRewardNotificationTextComponent userRankRewardNotificationText = notification.userRankRewardNotificationText;
            UserRankRewardNotificationInfoComponent userRankRewardNotificationInfo = notification.userRankRewardNotificationInfo;
            UserRankRewardNotificationUnityComponent userRankRewardNotificationUnity = notification.userRankRewardNotificationUnity;
            string format = "{0} {1}";
            string rankHeaderText = userRankRewardNotificationText.RankHeaderText;
            string rewardLabelText = userRankRewardNotificationText.RewardLabelText;
            long redCrystals = userRankRewardNotificationInfo.RedCrystals;
            long blueCrystals = userRankRewardNotificationInfo.BlueCrystals;
            userRankRewardNotificationUnity.RankHeaderElement.text = rankHeaderText;
            userRankRewardNotificationUnity.RankNameElement.text = ranksNames.ranksNames.Names[(int) ((IntPtr) userRankRewardNotificationInfo.Rank)];
            userRankRewardNotificationUnity.RankImageSkin.SelectSprite(userRankRewardNotificationInfo.Rank.ToString());
            if (redCrystals > 0L)
            {
                userRankRewardNotificationUnity.XCrystalsBlock.MoneyRewardField.text = string.Format(format, rewardLabelText, redCrystals);
                userRankRewardNotificationUnity.XCrystalsBlock.gameObject.SetActive(true);
            }
            if (blueCrystals > 0L)
            {
                userRankRewardNotificationUnity.CrystalsBlock.MoneyRewardField.text = string.Format(format, rewardLabelText, blueCrystals);
                userRankRewardNotificationUnity.CrystalsBlock.gameObject.SetActive(true);
            }
        }

        public class InstantiatedNotificationNode : UserRankRewardNotificationSystem.NotificationNode
        {
            public ActiveNotificationComponent activeNotification;
            public UserRankRewardNotificationUnityComponent userRankRewardNotificationUnity;
            public NotificationsGroupComponent notificationsGroup;
        }

        public class NotificationNode : Node
        {
            public UserRankRewardNotificationInfoComponent userRankRewardNotificationInfo;
            public UserRankRewardNotificationTextComponent userRankRewardNotificationText;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RanksNamesNode : Node
        {
            public RanksNamesComponent ranksNames;
        }

        public class UserNode : Node
        {
            public NotificationsGroupComponent notificationsGroup;
            public SelfUserComponent selfUser;
        }
    }
}

