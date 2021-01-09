namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class GoToChangeUserEmailScreenButtonLocalization : LocalizedControl
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        public override string YamlKey =>
            "changeEmailButton";

        public string Text
        {
            set => 
                this.text.text = value.ToUpper();
        }
    }
}

