namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class OpenGarageStepHandler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            MainScreenComponent.Instance.ShowParts();
            this.StepComplete();
        }
    }
}

