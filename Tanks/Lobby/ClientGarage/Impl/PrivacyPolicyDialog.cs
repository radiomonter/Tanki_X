namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PrivacyPolicyDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private LocalizedField fileName;
        [SerializeField]
        private TextMeshProUGUI text;

        public override void Hide()
        {
        }

        public void HideByAcceptButton()
        {
            if (base.show)
            {
                MainScreenComponent.Instance.ClearOnBackOverride();
                base.show = false;
                if (this != null)
                {
                    base.GetComponent<Animator>().SetBool("show", false);
                }
                base.ShowHiddenScreenParts();
            }
        }

        [DebuggerHidden]
        private IEnumerator LoadText() => 
            new <LoadText>c__Iterator0 { $this = this };

        public virtual void OnHide()
        {
            base.OnHide();
            this.text.text = string.Empty;
        }

        public override void OnShow()
        {
            base.OnShow();
            base.StartCoroutine(this.LoadText());
        }

        [CompilerGenerated]
        private sealed class <LoadText>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal string <path>__0;
            internal WWW <www>__0;
            internal PrivacyPolicyDialog $this;
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
                    {
                        string[] textArray1 = new string[] { "file://", Application.dataPath, "/config/clientlocal/privacypolicy/", this.$this.fileName.Value, ".txt" };
                        this.<path>__0 = string.Concat(textArray1);
                        this.<www>__0 = new WWW(this.<path>__0);
                        this.$current = this.<www>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    case 1:
                        this.$this.text.text = this.<www>__0.text;
                        this.$this.text.gameObject.AddComponent<TMPLink>();
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

