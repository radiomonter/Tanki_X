namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Animator))]
    public class ConfirmButtonComponent : MonoBehaviour, Component
    {
        [SerializeField]
        public Button button;
        [SerializeField]
        private Text buttonText;
        [SerializeField]
        private Text confirmText;
        [SerializeField]
        private Text cancelText;
        [SerializeField]
        private Button defaultButton;
        private bool enableOutsideClicking;

        public void Confirm()
        {
            base.GetComponent<Animator>().SetTrigger("confirm");
        }

        [DebuggerHidden]
        private IEnumerator DelayActivation(Button button) => 
            new <DelayActivation>c__Iterator0 { button = button };

        private void DisableOutsideClickingOption()
        {
            this.enableOutsideClicking = false;
        }

        private void EnableOutsideClickingOption()
        {
            this.enableOutsideClicking = true;
        }

        public void FlipBack()
        {
            base.GetComponent<Animator>().SetBool("flip", true);
            base.StartCoroutine(this.DelayActivation(this.defaultButton));
        }

        public void FlipFront()
        {
            base.GetComponent<Animator>().SetBool("flip", false);
            base.StartCoroutine(this.DelayActivation(this.button));
        }

        public bool EnableOutsideClicking =>
            this.enableOutsideClicking;

        public string ButtonText
        {
            get => 
                this.buttonText.text;
            set => 
                this.buttonText.text = value;
        }

        public string ConfirmText
        {
            get => 
                this.confirmText.text;
            set => 
                this.confirmText.text = value;
        }

        public string CancelText
        {
            get => 
                this.cancelText.text;
            set => 
                this.cancelText.text = value;
        }

        [CompilerGenerated]
        private sealed class <DelayActivation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Button button;
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
                    case 1:
                        if (this.button.isActiveAndEnabled)
                        {
                            this.button.Select();
                            this.$PC = -1;
                            break;
                        }
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

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

