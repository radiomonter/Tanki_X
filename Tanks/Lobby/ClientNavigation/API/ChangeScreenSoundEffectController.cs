namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class ChangeScreenSoundEffectController : UISoundEffectController
    {
        private const string HANDLER_NAME = "ChangeScreen";

        private void OnChangeScreen()
        {
            base.PlaySoundEffect();
        }

        public override string HandlerName =>
            "ChangeScreen";
    }
}

