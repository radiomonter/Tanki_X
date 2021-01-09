namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class SettingsRadioButton : RadioButton
    {
        [SerializeField]
        private Color defaultColor;
        [SerializeField]
        private Color activatedColor;

        public override void Activate()
        {
            base.Activate();
            base.GetComponentInChildren<TextMeshProUGUI>().color = this.activatedColor;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            base.GetComponentInChildren<TextMeshProUGUI>().color = this.defaultColor;
        }
    }
}

