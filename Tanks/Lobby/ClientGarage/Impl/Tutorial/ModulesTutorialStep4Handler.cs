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
    using UnityEngine.Events;

    public class ModulesTutorialStep4Handler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        [SerializeField]
        private RectTransform popupPositionRect;
        [SerializeField]
        private RectTransform arrowPositionRect;

        private void OnResearchClick()
        {
            this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnResearchClick));
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(this.modulesScreen);
            this.StepComplete();
        }

        public void OnSkipTutorial()
        {
            this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnResearchClick));
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(this.modulesScreen);
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            base.StartCoroutine(this.RunStepInNextFrame());
        }

        [DebuggerHidden]
        public IEnumerator RunStepInNextFrame() => 
            new <RunStepInNextFrame>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <RunStepInNextFrame>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal ModulesTutorialStep4Handler $this;
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
                    {
                        ModulesTutorialUtil.LockInteractable(this.$this.modulesScreen);
                        List<GameObject> objects = new List<GameObject> {
                            this.$this.modulesScreen.selectedModuleView.ResearchButton
                        };
                        ModulesTutorialUtil.SetOffset(objects);
                        TutorialCanvas.Instance.AddAdditionalMaskRect(this.$this.modulesScreen.selectedModuleView.ResearchButton);
                        this.$this.tutorialData.PopupPositionRect = this.$this.popupPositionRect;
                        this.$this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.$this.OnResearchClick));
                        TutorialCanvas.Instance.AddAllowSelectable(this.$this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>());
                        TutorialCanvas.Instance.Show(this.$this.tutorialData, true, null, this.$this.arrowPositionRect);
                        this.$PC = -1;
                        break;
                    }
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

