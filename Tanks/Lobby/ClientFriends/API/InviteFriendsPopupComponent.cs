namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientFriends.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class InviteFriendsPopupComponent : UIBehaviour, Component, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private UITableViewCell inviteToLobbyCell;
        [SerializeField]
        private UITableViewCell inviteToSquadCell;
        [SerializeField]
        private Tanks.Lobby.ClientFriends.API.InviteMode currentInviteMode;
        [SerializeField]
        private InviteFriendsUIComponent inviteFriends;
        private bool pointerIn;
        [SerializeField]
        private TextMeshProUGUI inviteHeader;
        [SerializeField]
        private LocalizedField inviteToLobbyLocalizationFiled;
        [SerializeField]
        private LocalizedField inviteToSquadLocalizationFiled;

        public void Close()
        {
            this.inviteFriends.Hide();
        }

        private void OnDisable()
        {
            this.pointerIn = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.pointerIn = false;
        }

        public void ShowInvite(Vector3 popupPosition, Vector2 pivot, Tanks.Lobby.ClientFriends.API.InviteMode inviteMode)
        {
            this.InviteMode = inviteMode;
            this.inviteHeader.text = (inviteMode != Tanks.Lobby.ClientFriends.API.InviteMode.Lobby) ? this.inviteToSquadLocalizationFiled.Value : this.inviteToLobbyLocalizationFiled.Value;
            base.GetComponent<RectTransform>().pivot = pivot;
            base.GetComponent<RectTransform>().position = popupPosition;
            this.inviteFriends.GetComponent<RectTransform>().pivot = pivot;
            this.inviteFriends.GetComponent<RectTransform>().position = popupPosition;
            this.inviteFriends.Show(null);
        }

        private void Update()
        {
            if (!this.pointerIn && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                this.Close();
            }
        }

        public Tanks.Lobby.ClientFriends.API.InviteMode InviteMode
        {
            set
            {
                if (value == Tanks.Lobby.ClientFriends.API.InviteMode.Lobby)
                {
                    this.inviteFriends.tableView.CellPrefab = this.inviteToLobbyCell;
                }
                else if (value == Tanks.Lobby.ClientFriends.API.InviteMode.Squad)
                {
                    this.inviteFriends.tableView.CellPrefab = this.inviteToSquadCell;
                }
            }
        }
    }
}

