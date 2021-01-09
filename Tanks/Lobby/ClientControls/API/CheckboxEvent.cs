namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class CheckboxEvent : Event
    {
        protected bool isChecked;

        public bool IsChecked =>
            this.isChecked;
    }
}

