namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NewCardNotificationComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, AttachToEntityListener, DetachFromEntityListener, IEventSystemHandler
    {
        [SerializeField]
        private bool clickAnywhere;
        private bool isClicked;
        private bool isHiden;
        private Entity entity;
        [SerializeField]
        private Transform container;

        private void Awake()
        {
            base.GetComponent<Animator>().SetFloat("multiple", Random.Range((float) 0.9f, (float) 1.1f));
        }

        public void CloseCardsButtonClicked()
        {
            base.GetComponent<Animator>().SetTrigger("end");
            base.StartCoroutine(this.NotificationClickEvent());
        }

        private void DestroyHidenCards()
        {
            base.StartCoroutine(this.NotificationClickEvent());
        }

        private void MouseClicked()
        {
            if (!this.isClicked)
            {
                base.GetComponent<Animator>().SetTrigger("click");
                NotificationsContainerComponent componentInParent = base.transform.GetComponentInParent<NotificationsContainerComponent>();
                componentInParent.openedCards++;
            }
        }

        [DebuggerHidden]
        private IEnumerator NotificationClickEvent() => 
            new <NotificationClickEvent>c__Iterator0 { $this = this };

        private void OnDestroy()
        {
            if ((base.transform.GetComponentInParent<NotificationsContainerComponent>() != null) && this.isClicked)
            {
                NotificationsContainerComponent componentInParent = base.transform.GetComponentInParent<NotificationsContainerComponent>();
                componentInParent.openedCards--;
                if (this.isHiden)
                {
                    NotificationsContainerComponent local2 = base.transform.GetComponentInParent<NotificationsContainerComponent>();
                    local2.hidenCards--;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.isClicked || this.isHiden)
            {
                this.MouseClicked();
                this.isClicked = true;
            }
            else
            {
                base.GetComponent<Animator>().SetTrigger("end");
                NotificationsContainerComponent componentInParent = base.transform.GetComponentInParent<NotificationsContainerComponent>();
                componentInParent.hidenCards++;
                this.isHiden = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            base.GetComponent<Animator>().SetBool("selected", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            base.GetComponent<Animator>().SetBool("selected", false);
        }

        public void OpenCardsButtonClicked()
        {
            base.GetComponent<Animator>().SetTrigger("Button");
            this.MouseClicked();
            this.isClicked = true;
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        [CompilerGenerated]
        private sealed class <NotificationClickEvent>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal NewCardNotificationComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        ECSBehaviour.EngineService.Engine.ScheduleEvent<NotificationClickEvent>(this.$this.entity);
                        this.$this.enabled = false;
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

