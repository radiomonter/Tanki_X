namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialShowArmsRaceCompensasionHandler : TutorialStepHandler
    {
        private void DialogClosed()
        {
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            ShowArmsRaceCompensationEvent eventInstance = new ShowArmsRaceCompensationEvent();
            base.ScheduleEvent(eventInstance, TutorialStepHandler.EngineService.EntityStub);
            if (eventInstance.Dialog == null)
            {
                this.StepComplete();
            }
            else
            {
                ConfirmDialogComponent dialog = eventInstance.Dialog;
                dialog.dialogClosed += new Action(this.DialogClosed);
            }
        }
    }
}

