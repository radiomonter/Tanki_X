namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class ShowIntroDialogHandler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            CheckForSkipTutorial eventInstance = new CheckForSkipTutorial();
            TutorialStepHandler.EngineService.Engine.ScheduleEvent(eventInstance, data.TutorialStep);
            TutorialCanvas.Instance.ShowIntroDialog(data, eventInstance.canSkipTutorial);
        }
    }
}

