namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class UserNotificationHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateUpdateUserRankMessage(UpdateRankEffectFinishedEvent evt, SelfBattleUserNode battleUser, [JoinByUser] SelfUserNode user, [JoinAll] ScreenNode screen, [JoinByScreen] UserNotificatorHUDNode notificator, [JoinByScreen] UserNotificatorRanksNamesNode notificatorNames)
        {
            UserRankNotificationMessageBehaviour notification = this.InstantiateUserNotification<UserRankNotificationMessageBehaviour>(notificator.userNotificatorHUD, notificator.userNotificatorHUD.UserRankNotificationMessagePrefab);
            notification.Icon.SelectSprite(user.userRank.Rank.ToString());
            notification.IconImage.SetNativeSize();
            notification.Message.text = string.Format(notificator.userNotificatorHUDText.UserRankMessageFormat, notificatorNames.ranksNames.Names[user.userRank.Rank]);
            notificator.userNotificatorHUD.Push(notification);
        }

        private T InstantiateUserNotification<T>(UserNotificatorHUDComponent notificator, T notificationPrefab) where T: BaseUserNotificationMessageBehaviour
        {
            T local = Object.Instantiate<T>(notificationPrefab);
            Transform transform = local.transform;
            transform.SetParent(notificator.transform);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            return local;
        }

        public class ScreenNode : Node
        {
            public ScreenGroupComponent screenGroup;
            public BattleScreenComponent battleScreen;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public UserGroupComponent userGroup;
        }

        public class UserNotificatorHUDNode : Node
        {
            public UserNotificatorHUDComponent userNotificatorHUD;
            public UserNotificatorHUDTextComponent userNotificatorHUDText;
            public ScreenGroupComponent screenGroup;
        }

        public class UserNotificatorRanksNamesNode : Node
        {
            public UserNotificatorRankNamesComponent userNotificatorRankNames;
            public RanksNamesComponent ranksNames;
            public ScreenGroupComponent screenGroup;
        }
    }
}

