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

    public class EulaDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private LocalizedField fileName;
        [SerializeField]
        private TextMeshProUGUI pageLabel;
        [SerializeField]
        private LocalizedField pageLocalizedField;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private GameObject prevPage;
        [SerializeField]
        private GameObject nextPage;
        [SerializeField]
        private GameObject pageButtons;
        [SerializeField]
        private GameObject agreeButton;
        [SerializeField]
        private GameObject loadingIndicator;

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

        public void NextPage()
        {
            this.text.pageToDisplay = Mathf.Min(this.text.pageToDisplay + 1, this.text.textInfo.pageCount);
            this.UpdatePageLabel();
        }

        public virtual void OnHide()
        {
            base.OnHide();
            this.text.text = string.Empty;
            this.pageButtons.SetActive(false);
            this.agreeButton.SetActive(false);
            this.loadingIndicator.SetActive(true);
        }

        public override void OnShow()
        {
            base.OnShow();
            base.StartCoroutine(this.LoadText());
        }

        public void PrevPage()
        {
            this.text.pageToDisplay = Mathf.Max(this.text.pageToDisplay - 1, 1);
            this.UpdatePageLabel();
        }

        private void UpdatePageButtons()
        {
            this.prevPage.gameObject.SetActive(this.text.pageToDisplay > 1);
            this.nextPage.gameObject.SetActive(this.text.pageToDisplay < this.text.textInfo.pageCount);
        }

        private void UpdatePageLabel()
        {
            this.pageLabel.text = $"{this.pageLocalizedField.Value} {this.text.pageToDisplay}/{this.text.textInfo.pageCount}";
            this.UpdatePageButtons();
        }

        [CompilerGenerated]
        private sealed class <LoadText>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal string <path>__0;
            internal WWW <www>__0;
            internal EulaDialog $this;
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
                        string[] textArray1 = new string[] { "file://", Application.dataPath, "/config/clientlocal/eula/", this.$this.fileName.Value, ".txt" };
                        this.<path>__0 = string.Concat(textArray1);
                        this.<www>__0 = new WWW(this.<path>__0);
                        this.$current = this.<www>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        break;
                    }
                    case 1:
                        this.$this.text.text = this.<www>__0.text;
                        this.$this.text.pageToDisplay = 1;
                        this.$this.loadingIndicator.SetActive(false);
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        this.$this.pageButtons.SetActive(true);
                        this.$this.agreeButton.SetActive(true);
                        this.$this.UpdatePageLabel();
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

