namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialOpenContainerHandler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            OpenTutorialContainerEvent eventInstance = new OpenTutorialContainerEvent {
                StepId = data.TutorialStep.GetComponent<TutorialStepDataComponent>().StepId,
                ItemId = data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId
            };
            TutorialStepHandler.EngineService.Engine.ScheduleEvent(eventInstance, TutorialStepHandler.EngineService.EntityStub);
            this.StepComplete();
        }
    }
}

