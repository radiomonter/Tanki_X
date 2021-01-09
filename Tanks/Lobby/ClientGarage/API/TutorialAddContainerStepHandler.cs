namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialAddContainerStepHandler : TutorialStepHandler
    {
        public void Fail(long stepId)
        {
            base.tutorialData.ContinueOnClick = true;
            TutorialCanvas.Instance.SetupActivePopup(base.tutorialData);
            base.gameObject.SetActive(true);
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            base.gameObject.SetActive(true);
        }

        public void Success(long stepId)
        {
            base.tutorialData.ContinueOnClick = true;
            TutorialCanvas.Instance.SetupActivePopup(base.tutorialData);
            base.gameObject.SetActive(true);
        }
    }
}

