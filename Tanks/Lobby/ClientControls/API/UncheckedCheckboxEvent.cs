namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class UncheckedCheckboxEvent : CheckboxEvent
    {
        public UncheckedCheckboxEvent()
        {
            base.isChecked = false;
        }
    }
}

