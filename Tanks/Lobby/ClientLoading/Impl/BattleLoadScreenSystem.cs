namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class BattleLoadScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckScreenReady(UpdateEvent e, NotReadeBattleLoadScreenNode screen)
        {
            if (screen.battleLoadScreen.isReadyToHide)
            {
                screen.Entity.AddComponent<BattleLoadScreenReadyToHideComponent>();
            }
        }

        [OnEventFire]
        public void HideNotificationOnBattleLoad(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> seftBattleUser, [JoinAll] SingleNode<AvatarDialogComponent> avatarDialog)
        {
            avatarDialog.component.Hide();
        }

        [OnEventFire]
        public void HideNotificationOnBattleLoad(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> seftBattleUser, [JoinAll] ClickableNotificationNode notification)
        {
            notification.activeNotification.Hide();
        }

        [OnEventFire]
        public void OnShowScreen(NodeAddedEvent e, BattleLoadScreenNode screen, [JoinAll] SingleNode<SelfBattleUserComponent> battleUser, [JoinByBattle, Context] BattleNode battle, [JoinByMap, Context] SingleNode<MapComponent> map)
        {
            screen.battleLoadScreen.InitView(battle.Entity, MapRegistry.GetMap(map.Entity));
        }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<ArcadeBattleComponent> battle)
        {
        }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<EnergyBattleComponent> battle)
        {
        }

        [OnEventFire]
        public void RegisterComponent(NodeAddedEvent e, SingleNode<RatingBattleComponent> battle)
        {
        }

        [OnEventFire]
        public void ShowScreen(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> battleUser, [JoinByBattle, Context] BattleNode battle, [JoinByMap, Context] MapNode map)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<BattleLoadScreenComponent>>(battleUser);
            base.ScheduleEvent(new ChangeScreenLogEvent(LogScreen.Battle), battleUser);
        }

        [Inject]
        public static Tanks.Lobby.ClientBattleSelect.API.MapRegistry MapRegistry { get; set; }

        public class BattleLoadScreenNode : Node
        {
            public BattleLoadScreenComponent battleLoadScreen;
        }

        public class BattleNode : Node
        {
            public BattleModeComponent battleMode;
            public BattleComponent battle;
        }

        [Not(typeof(NotClickableNotificationComponent))]
        public class ClickableNotificationNode : Node
        {
            public NotificationComponent notification;
            public NotificationMessageComponent notificationMessage;
            public NotificationConfigComponent notificationConfig;
            public ActiveNotificationComponent activeNotification;
            public EmailConfirmationNotificationComponent emailConfirmationNotification;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public DescriptionItemComponent descriptionItem;
        }

        [Not(typeof(BattleLoadScreenReadyToHideComponent))]
        public class NotReadeBattleLoadScreenNode : Node
        {
            public BattleLoadScreenComponent battleLoadScreen;
        }
    }
}

