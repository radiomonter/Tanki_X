namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class TutorialShowQuestsHandler : TutorialStepHandler
    {
        [SerializeField]
        private CustomizationUIComponent customizationUI;

        public void OpenHullModules()
        {
            this.customizationUI.TurretModules();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            base.gameObject.SetActive(true);
            this.StepComplete();
        }
    }
}

