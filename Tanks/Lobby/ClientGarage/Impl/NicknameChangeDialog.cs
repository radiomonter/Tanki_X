namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NicknameChangeDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private TMP_InputField inputField;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.button.interactable = false;
            this.inputField.ActivateInputField();
        }
    }
}

