namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class CheckedCheckboxEvent : CheckboxEvent
    {
        public CheckedCheckboxEvent()
        {
            base.isChecked = true;
        }
    }
}

