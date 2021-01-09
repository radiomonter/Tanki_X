namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class AddItemStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private bool completeOnResponse;

        public void Fail(long stepId)
        {
            if (this.completeOnResponse)
            {
                TutorialCanvas.Instance.UnblockInteractable();
                this.StepComplete();
            }
            else
            {
                base.tutorialData.ContinueOnClick = true;
                TutorialCanvas.Instance.SetupActivePopup(base.tutorialData);
            }
            base.gameObject.SetActive(false);
        }

        public override void RunStep(TutorialData data)
        {
            TutorialCanvas.Instance.BlockInteractable();
            base.RunStep(data);
            base.gameObject.SetActive(true);
        }

        public void Success(long stepId)
        {
            if (this.completeOnResponse)
            {
                TutorialCanvas.Instance.UnblockInteractable();
                this.StepComplete();
            }
            else
            {
                base.tutorialData.ContinueOnClick = true;
                TutorialCanvas.Instance.SetupActivePopup(base.tutorialData);
            }
            base.gameObject.SetActive(false);
        }

        public long stepId =>
            base.tutorialData.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId;
    }
}

