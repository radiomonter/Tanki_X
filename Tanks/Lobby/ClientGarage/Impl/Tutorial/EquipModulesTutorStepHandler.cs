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
    using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop;
    using UnityEngine;
    using UnityEngine.Events;

    public class EquipModulesTutorStepHandler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        [SerializeField]
        private RectTransform popupPositionRect;
        [SerializeField]
        private RectTransform arrowPositionRect;
        private CollectionSlotView collectionSlot;
        private SlotItemView moduleSlotItem;
        private bool tryToRunStep;

        [DebuggerHidden]
        private IEnumerator Complete() => 
            new <Complete>c__Iterator0 { $this = this };

        private void LockInteractable()
        {
            Debug.Log("LockInteractable equip");
            this.modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = false;
            this.modulesScreen.turretCollectionView.GetComponent<CanvasGroup>().alpha = 1f;
            this.modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this.modulesScreen.GetComponent<Animator>().enabled = false;
            this.modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this.modulesScreen.backButton.interactable = false;
            this.modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        private void OnDrop(DropDescriptor dropDescriptor, DropDescriptor backDescriptor)
        {
            if (dropDescriptor.sourceCell.gameObject != this.collectionSlot.gameObject)
            {
                Debug.Log("dropDescriptor.sourceCell.gameObject != collectionSlot.gameObject");
            }
            else if (dropDescriptor.item.gameObject != this.moduleSlotItem.gameObject)
            {
                Debug.Log("dropDescriptor.item.gameObject != moduleSlotItem.gameObject");
            }
            else
            {
                base.StartCoroutine(this.Complete());
            }
        }

        public void OnSkipTutorial()
        {
            ModulesTutorialUtil.ResetOffset();
            this.UnlockInteractable();
            ModulesTutorialUtil.TUTORIAL_MODE = false;
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            ModulesTutorialUtil.TUTORIAL_MODE = true;
            this.tryToRunStep = true;
            this.LockInteractable();
            this.tryToRunStep = true;
        }

        private void UnlockInteractable()
        {
            Debug.Log("UnlockInteractable equip");
            this.modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = true;
            this.modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.modulesScreen.backButton.interactable = true;
            this.modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.UnlockInteractable));
        }

        public void Update()
        {
            if (this.tryToRunStep)
            {
                ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(base.tutorialData);
                if ((moduleItem != null) && NewModulesScreenUIComponent.slotItems.ContainsKey(moduleItem))
                {
                    this.tryToRunStep = false;
                    this.collectionSlot = CollectionView.slots[moduleItem];
                    List<GameObject> objects = new List<GameObject> {
                        this.modulesScreen.turretCollectionView.gameObject,
                        this.collectionSlot.gameObject
                    };
                    ModulesTutorialUtil.SetOffset(objects);
                    this.moduleSlotItem = NewModulesScreenUIComponent.slotItems[moduleItem];
                    NewModulesScreenUIComponent.selection.Select(this.moduleSlotItem);
                    this.modulesScreen.dragAndDropController.onDrop += new Action<DropDescriptor, DropDescriptor>(this.OnDrop);
                    TutorialCanvas.Instance.AddAdditionalMaskRect(this.modulesScreen.turretCollectionView.gameObject);
                    TutorialCanvas.Instance.AddAdditionalMaskRect(this.collectionSlot.gameObject.transform.parent.gameObject);
                    base.tutorialData.PopupPositionRect = this.popupPositionRect;
                    TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.UnlockInteractable));
                    TutorialCanvas.Instance.Show(base.tutorialData, true, null, this.arrowPositionRect);
                }
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <Complete>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal EquipModulesTutorStepHandler $this;
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
                        this.$this.UnlockInteractable();
                        ModulesTutorialUtil.TUTORIAL_MODE = false;
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

