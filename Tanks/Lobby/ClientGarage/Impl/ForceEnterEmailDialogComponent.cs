namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class ForceEnterEmailDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TMP_InputField inputField;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.inputField.ActivateInputField();
        }
    }
}

