namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class UpgradeModuleTutorStep10Handler : TutorialStepHandler
    {
        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            this.StepComplete();
        }
    }
}

