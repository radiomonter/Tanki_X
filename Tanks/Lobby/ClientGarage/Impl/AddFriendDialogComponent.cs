namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class AddFriendDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TMP_InputField inputField;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ShowInput();
        }

        private void ShowInput()
        {
            this.inputField.ActivateInputField();
        }
    }
}

