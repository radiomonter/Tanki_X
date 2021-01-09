namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class BattleShutdownNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void AddNotificationText(NodeAddedEvent e, NotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        [OnEventFire]
        public void UpdateTimeNotification(UpdateEvent e, NotificationMessageNode notification, [JoinAll] SelfBattleNode battle, [JoinAll, Combine] SingleNode<LocalizedTimerComponent> timer)
        {
            float num = (float) (Date.Now - battle.battleStartTime.RoundStartTime);
            float time = battle.timeLimit.TimeLimitSec - num;
            if (time < 0f)
            {
                time = 0f;
            }
            string str = timer.component.GenerateTimerString(time);
            notification.notificationMessage.Message = string.Format(notification.battleShutdownText.Text, str);
        }

        public class NotificationMessageNode : Node
        {
            public BattleShutdownNotificationComponent battleShutdownNotification;
            public BattleShutdownTextComponent battleShutdownText;
            public NotificationMessageComponent notificationMessage;
        }

        public class NotificationNode : Node
        {
            public BattleShutdownNotificationComponent battleShutdownNotification;
            public BattleShutdownTextComponent battleShutdownText;
        }

        public class SelfBattleNode : Node
        {
            public BattleComponent battle;
            public SelfComponent self;
            public BattleStartTimeComponent battleStartTime;
            public TimeLimitComponent timeLimit;
        }
    }
}

