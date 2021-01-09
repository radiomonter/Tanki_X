namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WarmingUpTimerNotificationsComponent : BehaviourComponent
    {
        [SerializeField]
        private List<NotificationTime> warmingUpTimerNotifications;
        [SerializeField]
        private GameObject startBattleNotification;
        private bool notificationsInitialized;
        private Queue<NotificationTime> notifications = new Queue<NotificationTime>();
        [CompilerGenerated]
        private static Action<NotificationTime> <>f__am$cache0;

        public void DeactivateNotifications()
        {
            this.notificationsInitialized = false;
        }

        public float GetNextNotificationTime() => 
            !this.HasNotifications() ? -1f : this.notifications.Peek().remainingTime;

        public bool HasNotifications() => 
            this.notifications.Count > 0;

        public void Init(float remainingTime)
        {
            <Init>c__AnonStorey0 storey = new <Init>c__AnonStorey0 {
                remainingTime = remainingTime
            };
            this.SetNotificationsToInactiveState();
            this.notifications = new Queue<NotificationTime>(this.warmingUpTimerNotifications.Where<NotificationTime>(new Func<NotificationTime, bool>(storey.<>m__0)));
            this.NextNotificationTime = this.GetNextNotificationTime();
            this.notificationsInitialized = true;
        }

        private void OnDisable()
        {
            this.SetNotificationsToInactiveState();
        }

        private void SetNotificationsToInactiveState()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = notification => notification.notification.SetActive(false);
            }
            ((IList<NotificationTime>) this.warmingUpTimerNotifications).ForEach<NotificationTime>(<>f__am$cache0);
            this.startBattleNotification.SetActive(false);
        }

        public void ShowNextNotification()
        {
            if (this.notificationsInitialized)
            {
                this.notifications.Dequeue().notification.SetActive(true);
                this.NextNotificationTime = this.GetNextNotificationTime();
            }
        }

        public void ShowStartBattleNotification()
        {
            this.startBattleNotification.SetActive(true);
        }

        public float NextNotificationTime { get; set; }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey0
        {
            internal float remainingTime;

            internal bool <>m__0(WarmingUpTimerNotificationsComponent.NotificationTime notification) => 
                notification.remainingTime <= this.remainingTime;
        }

        [Serializable]
        public class NotificationTime
        {
            public float remainingTime;
            public GameObject notification;
        }
    }
}

