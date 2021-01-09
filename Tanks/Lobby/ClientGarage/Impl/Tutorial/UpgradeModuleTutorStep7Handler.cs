namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using Platform.Kernel.OSGi.ClientCore.API;
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

    public class UpgradeModuleTutorStep7Handler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        [SerializeField]
        private RectTransform popupPositionRect;
        [SerializeField]
        private RectTransform arrowPositionRect;
        private CollectionSlotView collectionSlot;

        [DebuggerHidden]
        private IEnumerator Complete() => 
            new <Complete>c__Iterator0 { $this = this };

        public void OnSkipTutorial()
        {
            if (this.collectionSlot != null)
            {
                ModulesTutorialUtil.ResetOffset();
                ModulesTutorialUtil.UnlockInteractable(this.modulesScreen);
                CanvasGroup component = this.collectionSlot.gameObject.GetComponent<CanvasGroup>();
                if (component != null)
                {
                    Destroy(component);
                }
            }
        }

        private void OnUpgradeClick()
        {
            this.modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnUpgradeClick));
            base.StartCoroutine(this.Complete());
        }

        private void RunStep()
        {
            ModulesTutorialUtil.LockInteractable(this.modulesScreen);
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(base.tutorialData);
            this.collectionSlot = CollectionView.slots[moduleItem];
            this.collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            List<GameObject> objects = new List<GameObject> {
                this.collectionSlot.gameObject,
                this.modulesScreen.turretCollectionView.gameObject,
                this.modulesScreen.selectedModuleView.UpgradeCRYButton
            };
            ModulesTutorialUtil.SetOffset(objects);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.collectionSlot.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.modulesScreen.turretCollectionView.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.modulesScreen.selectedModuleView.UpgradeCRYButton);
            TutorialCanvas.Instance.AddAllowSelectable(this.modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>());
            NewModulesScreenUIComponent.selection.Select(NewModulesScreenUIComponent.slotItems[moduleItem]);
            this.modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnUpgradeClick));
            base.tutorialData.PopupPositionRect = this.popupPositionRect;
            TutorialCanvas.Instance.Show(base.tutorialData, true, null, this.arrowPositionRect);
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            if (!this.modulesScreen.showAnimationFinished)
            {
                this.modulesScreen.OnShowAnimationFinishedAction += new Action(this.RunStepDelayed);
            }
            else
            {
                this.RunStep();
            }
        }

        public void RunStepDelayed()
        {
            Debug.Log("RunStepDelayed");
            this.modulesScreen.OnShowAnimationFinishedAction -= new Action(this.RunStepDelayed);
            this.RunStep();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <Complete>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal UpgradeModuleTutorStep7Handler $this;
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
                        ModulesTutorialUtil.ResetOffset();
                        ModulesTutorialUtil.UnlockInteractable(this.$this.modulesScreen);
                        Object.Destroy(this.$this.collectionSlot.GetComponent<CanvasGroup>());
                        ModulesTutorialSystem.tutorialActive = false;
                        this.$this.StepComplete();
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

