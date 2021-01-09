namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialShowDialogContainerHandler : TutorialStepHandler
    {
        public int chestCount;

        private void Complete()
        {
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            OpenTutorialContainerDialogEvent eventInstance = new OpenTutorialContainerDialogEvent {
                StepId = base.tutorialData.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId,
                ItemId = base.tutorialData.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId,
                ItemsCount = this.chestCount
            };
            base.ScheduleEvent(eventInstance, TutorialStepHandler.EngineService.EntityStub);
            eventInstance.dialog.Message = base.tutorialData.Message;
            TutorialContainerDialog dialog = eventInstance.dialog;
            dialog.dialogClosed += new Action(this.Complete);
        }
    }
}

