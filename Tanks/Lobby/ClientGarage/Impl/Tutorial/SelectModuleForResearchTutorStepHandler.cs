namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.Events;

    public class SelectModuleForResearchTutorStepHandler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;
        private CollectionSlotView collectionSlot;

        private void OnResearchClick()
        {
            this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnResearchClick));
            Destroy(this.collectionSlot.gameObject.GetComponent<CanvasGroup>());
        }

        public void OnSkipTutorial()
        {
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(new UnityAction(this.OnSkipTutorial));
            if (this.collectionSlot != null)
            {
                Destroy(this.collectionSlot.gameObject.GetComponent<CanvasGroup>());
            }
        }

        private void RunStep()
        {
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(base.tutorialData);
            this.collectionSlot = CollectionView.slots[moduleItem];
            List<GameObject> objects = new List<GameObject> {
                this.collectionSlot.gameObject
            };
            ModulesTutorialUtil.SetOffset(objects);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.collectionSlot.gameObject);
            NewModulesScreenUIComponent.selection.Select(this.collectionSlot);
            this.collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSkipTutorial));
            this.modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnResearchClick));
            this.StepComplete();
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
    }
}

