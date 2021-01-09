namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class InputFieldSubmitSoundEffectController : AbstractInputFieldSoundEffectController
    {
        private void OnGUI()
        {
            if ((base.Selected && (Event.current.type == EventType.KeyDown)) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                base.PlaySoundEffect();
            }
        }

        public override string HandlerName =>
            "Submit";
    }
}

