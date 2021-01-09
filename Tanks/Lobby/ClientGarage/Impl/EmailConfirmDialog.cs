namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class EmailConfirmDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI confirmationHintLabel;
        [SerializeField]
        private LocalizedField confirmationHint;
        [SerializeField]
        private PaletteColorField emailColor;

        public void ShowEmailConfirm(string email)
        {
            string[] textArray1 = new string[] { "<color=#", this.emailColor.Color.ToHexString(), ">", email, "</color>" };
            this.confirmationHintLabel.text = this.confirmationHint.Value.Replace("%EMAIL%", string.Concat(textArray1));
        }
    }
}

