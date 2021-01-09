namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;

    public class SelectModuleForUpgradeTutorStepHandler : TutorialStepHandler
    {
        public NewModulesScreenUIComponent modulesScreen;

        private void RunStep()
        {
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(base.tutorialData);
            CollectionSlotView view = CollectionView.slots[moduleItem];
            List<GameObject> objects = new List<GameObject> {
                view.gameObject,
                this.modulesScreen.turretCollectionView.gameObject
            };
            ModulesTutorialUtil.SetOffset(objects);
            NewModulesScreenUIComponent.selection.Select(NewModulesScreenUIComponent.slotItems[moduleItem]);
            TutorialCanvas.Instance.AddAdditionalMaskRect(view.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(this.modulesScreen.turretCollectionView.gameObject);
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

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

