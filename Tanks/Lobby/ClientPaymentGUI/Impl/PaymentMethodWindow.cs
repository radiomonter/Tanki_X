namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class PaymentMethodWindow : MonoBehaviour
    {
        [SerializeField]
        private RectTransform methodsRoot;
        [SerializeField]
        private TextMeshProUGUI info;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private PaymentMethodContent methodPrefab;
        private Action onBack;
        private Action<Entity> onForward;
        private static readonly int CancelHash = Animator.StringToHash("cancel");

        public void Cancel()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger(CancelHash);
            this.onBack();
        }

        private void OnDisable()
        {
            IEnumerator enumerator = this.methodsRoot.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void Show(Entity item, List<Entity> methods, Action onBack, Action<Entity> onForward, string desc = "")
        {
            <Show>c__AnonStorey0 storey = new <Show>c__AnonStorey0 {
                onForward = onForward,
                $this = this
            };
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            this.onBack = onBack;
            this.onForward = storey.onForward;
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, null);
            if (!string.IsNullOrEmpty(desc))
            {
                this.description.text = "\n" + desc;
                this.description.gameObject.SetActive(true);
            }
            else
            {
                this.description.text = desc;
                this.description.gameObject.SetActive(false);
            }
            foreach (Entity entity in methods)
            {
                <Show>c__AnonStorey1 storey2 = new <Show>c__AnonStorey1 {
                    <>f__ref$0 = storey
                };
                PaymentMethodContent content = Instantiate<PaymentMethodContent>(this.methodPrefab, this.methodsRoot, false);
                content.SetDataProvider(entity);
                storey2.target = entity;
                content.GetComponent<Button>().onClick.AddListener(new UnityAction(storey2.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <Show>c__AnonStorey0
        {
            internal Action<Entity> onForward;
            internal PaymentMethodWindow $this;
        }

        [CompilerGenerated]
        private sealed class <Show>c__AnonStorey1
        {
            internal Entity target;
            internal PaymentMethodWindow.<Show>c__AnonStorey0 <>f__ref$0;

            internal void <>m__0()
            {
                MainScreenComponent.Instance.ClearOnBackOverride();
                this.<>f__ref$0.$this.GetComponent<Animator>().SetTrigger(PaymentMethodWindow.CancelHash);
                this.<>f__ref$0.onForward(this.target);
            }
        }
    }
}

