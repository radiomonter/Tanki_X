namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ShowArmsRaceCompensationEvent : Event
    {
        private ConfirmDialogComponent dialog;

        public ConfirmDialogComponent Dialog
        {
            get => 
                this.dialog;
            set => 
                this.dialog = value;
        }
    }
}

