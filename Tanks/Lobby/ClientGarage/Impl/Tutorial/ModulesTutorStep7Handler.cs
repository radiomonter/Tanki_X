namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModulesTutorStep7Handler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        public List<GameObject> highlightedObjects;
        [SerializeField]
        private RectTransform popupPositionRect;

        private void OnComplete()
        {
            this.OnSkipTutorial();
            this.StepComplete();
        }

        public void OnSkipTutorial()
        {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(this.modulesScreen);
        }

        [DebuggerHidden]
        public IEnumerator OverrideOnClickHandler() => 
            new <OverrideOnClickHandler>c__Iterator0 { $this = this };

        public override void RunStep(TutorialData data)
        {
            Debug.Log("RunStep 7");
            base.RunStep(data);
            this.highlightedObjects.Add(this.modulesScreen.collectionView.turretCollectionView.activeTierCollectionList.transform.GetChild(0).gameObject);
            this.highlightedObjects.Add(this.modulesScreen.collectionView.turretCollectionView.passiveTierCollectionList.transform.GetChild(0).gameObject);
            foreach (GameObject obj2 in this.highlightedObjects)
            {
                TutorialCanvas.Instance.AddAdditionalMaskRect(obj2);
            }
            ModulesTutorialUtil.SetOffset(this.highlightedObjects);
            ModulesTutorialUtil.LockInteractable(this.modulesScreen);
            data.ContinueOnClick = true;
            data.PopupPositionRect = this.popupPositionRect;
            TutorialCanvas.Instance.arrow.gameObject.SetActive(false);
            TutorialCanvas.Instance.Show(data, true, null, null);
            base.StartCoroutine(this.OverrideOnClickHandler());
        }

        [CompilerGenerated]
        private sealed class <OverrideOnClickHandler>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal ModulesTutorStep7Handler $this;
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

