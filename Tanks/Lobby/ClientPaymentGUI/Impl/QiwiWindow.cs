namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class QiwiWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI info;
        [SerializeField]
        private QiwiAccountFormatterComponent account;
        [SerializeField]
        private Animator continueButton;
        private Action onBack;
        private Action onForward;

        private void Awake()
        {
            this.account.GetComponent<TMP_InputField>().onValueChanged.AddListener(new UnityAction<string>(this.ValidateInput));
        }

        public void Cancel()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onBack();
        }

        [DebuggerHidden]
        private IEnumerator DelaySet(string acc) => 
            new <DelaySet>c__Iterator0 { 
                acc = acc,
                $this = this
            };

        public void Proceed()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onForward();
        }

        public void Show(Entity item, Entity method, string acc, Action onBack, Action onForward)
        {
            base.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(acc))
            {
                base.StartCoroutine(this.DelaySet(acc));
            }
            this.account.GetComponent<TMP_InputField>().Select();
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            this.onBack = onBack;
            this.onForward = onForward;
            this.info.text = ShopDialogs.FormatItem(item, method);
        }

        private void ValidateInput(string value)
        {
            this.continueButton.SetBool("Visible", this.account.IsValidPhoneNumber);
        }

        public string Account =>
            this.account.Account;

        [CompilerGenerated]
        private sealed class <DelaySet>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal TMP_InputField <input>__0;
            internal string acc;
            internal QiwiWindow $this;
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
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<input>__0 = this.$this.account.GetComponent<TMP_InputField>();
                        this.<input>__0.text = this.<input>__0.text + this.acc;
                        this.<input>__0.MoveTextEnd(false);
                        this.$this.continueButton.SetBool("Visible", false);
                        this.$this.account.GetComponent<Animator>().SetBool("HasError", true);
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

