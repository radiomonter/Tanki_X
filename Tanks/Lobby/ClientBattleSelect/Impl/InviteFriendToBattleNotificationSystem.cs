namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class InviteFriendToBattleNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void GoToBattle(NotificationClickEvent e, NotificationNode notification, [JoinAll] Optional<SingleNode<SelfBattleUserComponent>> userInBattle)
        {
            base.ScheduleEvent(new ShowBattleEvent(notification.battleGroup.Key), EngineService.EntityStub);
        }

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification, [Context, JoinByUser] SingleNode<UserUidComponent> fromUser, NotificationNode notificationToMap, [Context, JoinByMap] MapNode map)
        {
            string message = string.Format(notification.inviteFriendToBattleNotification.MessageTemplate, fromUser.component.Uid, map.descriptionItem.Name, notification.battleMode.BattleMode);
            notification.Entity.AddComponent(new NotificationMessageComponent(message));
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class MapNode : Node
        {
            public MapComponent map;
            public DescriptionItemComponent descriptionItem;
        }

        public class NotificationNode : Node
        {
            public InviteFriendToBattleNotificationComponent inviteFriendToBattleNotification;
            public UserGroupComponent userGroup;
            public BattleModeComponent battleMode;
            public BattleGroupComponent battleGroup;
            public MapGroupComponent mapGroup;
        }
    }
}

