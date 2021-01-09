namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class OpenStatStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private Button statButton;

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            this.statButton.onClick.Invoke();
            this.StepComplete();
        }
    }
}

