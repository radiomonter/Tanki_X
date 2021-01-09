namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class UIElementHomeSoundEffectController : UISoundEffectController
    {
        private void OnDisable()
        {
            if (base.alive)
            {
                this.PlayHomeSoundEffectIfNeeded();
            }
        }

        private void PlayHomeSoundEffectIfNeeded()
        {
            if (Input.GetKey(KeyCode.Home))
            {
                base.PlaySoundEffect();
            }
        }

        public override string HandlerName =>
            "Home";
    }
}

