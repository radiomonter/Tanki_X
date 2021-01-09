namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialRemoveItemHandler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            this.StepComplete();
        }
    }
}

