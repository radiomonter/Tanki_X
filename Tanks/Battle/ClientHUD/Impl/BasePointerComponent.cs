namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class BasePointerComponent : CTFPointerComponent
    {
        public Sprite FlagOnBaseImage;
        public Sprite FlagStolenImage;
        public LocalizedField BaseText;
        public UnityEngine.UI.Image Image;

        public void OnEnable()
        {
            base.text.text = this.BaseText.Value;
        }

        public void SetFlagAtHomeState()
        {
            this.Image.sprite = this.FlagOnBaseImage;
        }

        public void SetFlagStolenState()
        {
            this.Image.sprite = this.FlagStolenImage;
        }
    }
}

