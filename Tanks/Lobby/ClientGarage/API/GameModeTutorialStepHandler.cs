namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class GameModeTutorialStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private TutorialShowTriggerComponent nextStepTrigger;
        [SerializeField]
        private Transform buttonContainer;

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            Button componentInChildren = this.buttonContainer.GetComponentInChildren<Button>();
            if (componentInChildren != null)
            {
                this.nextStepTrigger.SetSeleectable(componentInChildren);
            }
            this.StepComplete();
        }
    }
}

