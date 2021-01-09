namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.Events;

    public class UpgradeModulesTutorStep4Handler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        [SerializeField]
        private RectTransform popupPositionRect;
        private CollectionSlotView collectionSlot;

        private void OnComplete()
        {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            Destroy(this.collectionSlot.gameObject.GetComponent<CanvasGroup>());
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(this.modulesScreen);
            this.modulesScreen.Hide();
            ModulesTutorialSystem.tutorialActive = true;
            this.StepComplete();
        }

        private void OnSkipTutorial()
        {
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnSkipTutorial));
            Destroy(this.collectionSlot.gameObject.GetComponent<CanvasGroup>());
        }

        [DebuggerHidden]
        public IEnumerator OverrideOnClickHandler() => 
            new <OverrideOnClickHandler>c__Iterator0 { $this = this };

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            Debug.Log("Run 4");
            ModulesTutorialUtil.LockInteractable(this.modulesScreen);
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(base.tutorialData);
            this.collectionSlot = CollectionView.slots[moduleItem];
            this.collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSkipTutorial));
            data.ContinueOnClick = true;
            data.PopupPositionRect = this.popupPositionRect;
            TutorialCanvas.Instance.arrow.gameObject.SetActive(false);
            TutorialCanvas.Instance.Show(data, true, null, null);
            base.StartCoroutine(this.OverrideOnClickHandler());
        }

        [CompilerGenerated]
        private sealed class <OverrideOnClickHandler>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpgradeModulesTutorStep4Handler $this;
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
                        break;

                    case 1:
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        TutorialCanvas.Instance.dialog.PopupContinue = new TutorialPopupContinue(this.$this.OnComplete);
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

