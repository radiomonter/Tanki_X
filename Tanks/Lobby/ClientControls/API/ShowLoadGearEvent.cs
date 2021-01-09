namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowLoadGearEvent : Event
    {
        public ShowLoadGearEvent()
        {
            this.ShowProgress = false;
        }

        public ShowLoadGearEvent(bool showProgress)
        {
            this.ShowProgress = showProgress;
        }

        public bool ShowProgress { get; set; }
    }
}

