namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NotificationsContainerComponent : BehaviourComponent
    {
        [SerializeField]
        private List<GameObject> rows;
        [SerializeField]
        private GameObject fullSceenNotificationContainer;
        [SerializeField]
        private int columnsCount;
        public int openedCards;
        public int hidenCards;
        [SerializeField]
        private GameObject OpenAllCardsButton;
        [SerializeField]
        private GameObject CloseAllCardsButton;
        [SerializeField]
        private GameObject cardsCamera;
        [SerializeField]
        private GameObject outlineBlurCamera;
        private bool isHiden;

        [DebuggerHidden]
        private IEnumerator CloseHidenCards() => 
            new <CloseHidenCards>c__Iterator0 { $this = this };

        public Transform GetFullSceenNotificationContainer() => 
            this.fullSceenNotificationContainer.transform;

        public Transform GetParenTransform(int index, int count) => 
            this.rows[this.GetRowIndex(index, count)].transform;

        private int GetRowIndex(int index, int count)
        {
            int columnsCount = this.columnsCount;
            if (count == 4)
            {
                columnsCount = 2;
            }
            int num2 = index / columnsCount;
            if (num2 >= this.rows.Count)
            {
                num2 = this.rows.Count - 1;
            }
            return num2;
        }

        public void Update()
        {
            int num = 0;
            int num2 = 0;
            while (num2 < this.rows.Count)
            {
                int index = 0;
                while (true)
                {
                    if (index >= this.rows[num2].transform.childCount)
                    {
                        num2++;
                        break;
                    }
                    Transform child = this.rows[num2].transform.GetChild(index);
                    if (child.GetComponentInChildren<NewCardNotificationComponent>() != null)
                    {
                        num++;
                    }
                    index++;
                }
            }
            if (num == 0)
            {
                this.OpenAllCardsButton.SetActive(false);
                this.CloseAllCardsButton.SetActive(false);
                this.cardsCamera.SetActive(false);
                this.outlineBlurCamera.SetActive(false);
            }
            if ((num > 0) && (this.openedCards != num))
            {
                this.OpenAllCardsButton.SetActive(true);
                this.CloseAllCardsButton.SetActive(false);
                this.cardsCamera.SetActive(true);
                this.outlineBlurCamera.SetActive(true);
            }
            if ((num > 0) && (this.openedCards == num))
            {
                this.OpenAllCardsButton.SetActive(false);
                this.CloseAllCardsButton.SetActive(true);
                this.cardsCamera.SetActive(true);
                this.outlineBlurCamera.SetActive(true);
            }
            if ((num == this.hidenCards) && ((num > 0) && !this.isHiden))
            {
                this.isHiden = true;
                base.StartCoroutine(this.CloseHidenCards());
            }
        }

        public int MaxItemsPerScreen =>
            this.rows.Count * this.columnsCount;

        [CompilerGenerated]
        private sealed class <CloseHidenCards>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal PointerEventData <pointer>__0;
            internal NotificationsContainerComponent $this;
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
                        this.$current = new WaitForSeconds(0.3f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        break;

                    case 1:
                        this.<pointer>__0 = new PointerEventData(EventSystem.current);
                        ExecuteEvents.Execute<ISubmitHandler>(this.$this.CloseAllCardsButton.GetComponent<Button>().gameObject, this.<pointer>__0, ExecuteEvents.submitHandler);
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        this.$this.isHiden = false;
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                return true;
            TR_0000:
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

