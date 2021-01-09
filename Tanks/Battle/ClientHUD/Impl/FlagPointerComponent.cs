namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class FlagPointerComponent : CTFPointerComponent
    {
        public LocalizedField FlagOnTheGroundText;
        public LocalizedField FlagCapturedText;
        public GameObject pointer;

        public void SetFlagCapturedState()
        {
            base.text.text = this.FlagCapturedText.Value;
        }

        public void SetFlagOnTheGroundState()
        {
            base.text.text = this.FlagOnTheGroundText.Value;
        }
    }
}

