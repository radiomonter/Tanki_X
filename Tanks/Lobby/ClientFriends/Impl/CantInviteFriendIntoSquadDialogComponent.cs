namespace Tanks.Lobby.ClientFriends.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class CantInviteFriendIntoSquadDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private LocalizedField messageLocalizedField;

        public void Show(string friendUid, List<Animator> animators = null)
        {
            this.message.text = string.Format(this.messageLocalizedField.Value, "<color=orange>" + friendUid + "</color>", "\n");
            base.Show(animators);
        }
    }
}

