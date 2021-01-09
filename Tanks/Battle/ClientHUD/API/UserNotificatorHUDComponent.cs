namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UserNotificatorHUDComponent : BehaviourComponent
    {
        [SerializeField]
        private UserRankNotificationMessageBehaviour userRankNotificationMessagePrefab;
        [SerializeField]
        private CanvasGroup serviseMessagesCanvasGroup;
        [SerializeField]
        private float serviceMessagesFadeTime = 0.5f;
        private ServiceMessageHUDState serviceMessageState;
        private float fadeSpeed;
        private Queue<BaseUserNotificationMessageBehaviour> messagesQueue;
        private BaseUserNotificationMessageBehaviour activeNotification;
        [CompilerGenerated]
        private static Action<BaseUserNotificationMessageBehaviour> <>f__am$cache0;

        private void OnEnable()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = m => DestroyObject(m.gameObject);
            }
            base.GetComponentsInChildren<BaseUserNotificationMessageBehaviour>(true).ForEach<BaseUserNotificationMessageBehaviour>(<>f__am$cache0);
            this.serviseMessagesCanvasGroup.alpha = 1f;
            this.serviceMessageState = ServiceMessageHUDState.IDLE;
            this.fadeSpeed = 1f / this.serviceMessagesFadeTime;
            this.messagesQueue = new Queue<BaseUserNotificationMessageBehaviour>();
        }

        private void OnUserNotificationFadeOut()
        {
            this.activeNotification = null;
            if (this.messagesQueue.Count <= 0)
            {
                this.serviceMessageState = ServiceMessageHUDState.FADE_IN;
            }
            else
            {
                this.activeNotification = this.messagesQueue.Dequeue();
                this.PlayNextNotification();
            }
        }

        private void PlayNextNotification()
        {
            this.activeNotification.Play();
        }

        public void Push(BaseUserNotificationMessageBehaviour notification)
        {
            this.serviceMessageState = ServiceMessageHUDState.FADE_OUT;
            if (this.activeNotification == null)
            {
                this.activeNotification = notification;
            }
            else
            {
                this.messagesQueue.Enqueue(notification);
            }
        }

        private void Update()
        {
            if (this.serviceMessageState != ServiceMessageHUDState.IDLE)
            {
                if (this.serviceMessageState == ServiceMessageHUDState.FADE_IN)
                {
                    if (this.serviseMessagesCanvasGroup.alpha < 1f)
                    {
                        this.serviseMessagesCanvasGroup.alpha += this.fadeSpeed * Time.deltaTime;
                    }
                    else
                    {
                        this.serviseMessagesCanvasGroup.alpha = 1f;
                        this.serviceMessageState = ServiceMessageHUDState.IDLE;
                    }
                }
                else if (this.serviceMessageState == ServiceMessageHUDState.FADE_OUT)
                {
                    if (this.serviseMessagesCanvasGroup.alpha <= 0f)
                    {
                        this.serviseMessagesCanvasGroup.alpha = 0f;
                        this.serviceMessageState = ServiceMessageHUDState.IDLE;
                        this.PlayNextNotification();
                    }
                    else
                    {
                        this.serviseMessagesCanvasGroup.alpha -= this.fadeSpeed * Time.deltaTime;
                    }
                }
            }
        }

        public UserRankNotificationMessageBehaviour UserRankNotificationMessagePrefab =>
            this.userRankNotificationMessagePrefab;

        private enum ServiceMessageHUDState
        {
            IDLE,
            FADE_IN,
            FADE_OUT
        }
    }
}

