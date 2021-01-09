namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class SelectModuleStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private ModulesScreenUIComponent modulesScreen;
        private ModuleCardItemUIComponent selectedModule;

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            this.selectedModule = this.modulesScreen.GetCard(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);
            if (this.selectedModule != null)
            {
                TutorialCanvas.Instance.AddAdditionalMaskRect(this.selectedModule.gameObject);
                TutorialCanvas.Instance.AddAllowSelectable(this.selectedModule.GetComponent<Toggle>());
                this.selectedModule.GetComponent<Toggle>().isOn = true;
            }
            this.StepComplete();
        }
    }
}

