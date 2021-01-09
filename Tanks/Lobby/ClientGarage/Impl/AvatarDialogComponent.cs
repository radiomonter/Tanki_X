namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class AvatarDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Button closeButton;

        private void Awake()
        {
            this.cancelButton.onClick.AddListener(new UnityAction(this.Hide));
            this.closeButton.onClick.AddListener(new UnityAction(this.Hide));
        }
    }
}

