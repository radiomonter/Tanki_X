namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeEmailDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private GameObject input;
        [SerializeField]
        private GameObject confirm;
        [SerializeField]
        private Button button;
        [SerializeField]
        private TextMeshProUGUI hintLabel;
        [SerializeField]
        private TextMeshProUGUI confirmationHintLabel;
        [SerializeField]
        private LocalizedField hint;
        [SerializeField]
        private LocalizedField confirmationHint;
        [SerializeField]
        private PaletteColorField emailColor;
        [SerializeField]
        private TMP_InputField inputField;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ShowInput();
            this.button.interactable = false;
        }

        public void SetActiveHint(bool value)
        {
            this.hintLabel.text = !value ? string.Empty : this.hint.Value;
            this.hintLabel.rectTransform.sizeDelta = new Vector2(this.hintLabel.rectTransform.sizeDelta.x, !value ? ((float) 30) : ((float) 80));
        }

        public void ShowEmailConfirm(string email)
        {
            string[] textArray1 = new string[] { "<color=#", this.emailColor.Color.ToHexString(), ">", email, "</color>" };
            this.confirmationHintLabel.text = this.confirmationHint.Value.Replace("%EMAIL%", string.Concat(textArray1));
            this.input.SetActive(false);
            this.confirm.SetActive(true);
        }

        private void ShowInput()
        {
            this.confirm.SetActive(false);
            this.input.SetActive(true);
            this.inputField.ActivateInputField();
        }
    }
}

