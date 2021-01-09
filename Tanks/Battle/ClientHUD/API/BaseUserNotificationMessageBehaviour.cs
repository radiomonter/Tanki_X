namespace Tanks.Battle.ClientHUD.API
{
    using System;
    using UnityEngine;

    public abstract class BaseUserNotificationMessageBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        private float lifeTime = 3f;
        private float timer;
        private bool destroyTriggerSend;
        private bool isDestroying;

        protected BaseUserNotificationMessageBehaviour()
        {
        }

        private void OnEnable()
        {
            this.isDestroying = false;
            this.destroyTriggerSend = false;
            this.timer = this.lifeTime;
        }

        private void OnNotificationFadeIn()
        {
            this.isDestroying = true;
        }

        private void OnNotificationFadeOut()
        {
            base.SendMessageUpwards("OnUserNotificationFadeOut", SendMessageOptions.RequireReceiver);
            DestroyObject(base.gameObject);
        }

        public void Play()
        {
            base.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (this.isDestroying && !this.destroyTriggerSend)
            {
                if (this.timer > 0f)
                {
                    this.timer -= Time.deltaTime;
                }
                else
                {
                    this.animator.SetTrigger("FadeOut");
                    this.destroyTriggerSend = true;
                }
            }
        }
    }
}

