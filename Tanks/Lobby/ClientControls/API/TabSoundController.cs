namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class TabSoundController : UISoundEffectController
    {
        private string _handlerName;

        public override string HandlerName =>
            this._handlerName;
    }
}

