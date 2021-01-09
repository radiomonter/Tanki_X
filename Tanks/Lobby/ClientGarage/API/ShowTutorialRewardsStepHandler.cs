namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class ShowTutorialRewardsStepHandler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            TutorialStepHandler.EngineService.Engine.ScheduleEvent<ShowTutorialRewardsEvent>(data.TutorialStep);
            this.StepComplete();
        }
    }
}

