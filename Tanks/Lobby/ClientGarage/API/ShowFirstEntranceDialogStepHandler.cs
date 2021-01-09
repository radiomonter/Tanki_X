namespace Tanks.Lobby.ClientGarage.API
{
    using Lobby.ClientGarage.Impl;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ShowFirstEntranceDialogStepHandler : TutorialStepHandler
    {
        private void Complete()
        {
            this.StepComplete();
        }

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            WelcomeScreenDialog dialog = FindObjectOfType<Dialogs60Component>().Get<WelcomeScreenDialog>();
            dialog.Show(null);
            dialog.dialogClosed += new Action(this.Complete);
        }
    }
}

