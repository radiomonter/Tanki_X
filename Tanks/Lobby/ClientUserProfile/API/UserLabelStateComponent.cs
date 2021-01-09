namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserLabelStateComponent : BehaviourComponent
    {
        [SerializeField]
        private Image[] images;
        [SerializeField]
        private CanvasGroup textGroup;
        [SerializeField]
        private TextMeshProUGUI stateText;
        [SerializeField]
        private LocalizedField online;
        [SerializeField]
        private LocalizedField offline;
        [SerializeField]
        private LocalizedField inBattle;
        [SerializeField]
        private Color onlineColor;
        [SerializeField]
        private Color offlineColor;
        [SerializeField]
        private float alpha = 0.6f;
        [SerializeField]
        private bool userInBattle;
        [SerializeField]
        private GameObject userInSquadLabel;
        [SerializeField]
        private Button inviteButton;
        [SerializeField]
        private bool disableInviteOnlyForSquadState;

        private void Awake()
        {
            this.UserOffline();
        }

        public void SetAlpha(float alpha)
        {
            foreach (Image image in this.images)
            {
                Color color = image.color;
                Color color2 = image.color;
                Color color3 = image.color;
                image.color = new Color(color.r, color2.g, color3.b, alpha);
                this.textGroup.alpha = alpha;
            }
        }

        public void SetBattleDescription(string mode, string map)
        {
            string[] textArray1 = new string[] { this.inBattle.Value, " (", map, ", ", mode, ")" };
            this.stateText.text = string.Concat(textArray1);
            this.stateText.color = this.onlineColor;
        }

        public void UserInBattle()
        {
            this.userInBattle = true;
            this.stateText.text = this.inBattle.Value;
            this.stateText.color = this.onlineColor;
        }

        public void UserOffline()
        {
            this.SetAlpha(this.alpha);
            this.stateText.text = this.offline.Value;
            this.stateText.color = this.offlineColor;
        }

        public void UserOnline()
        {
            this.SetAlpha(1f);
            if (!this.userInBattle)
            {
                this.stateText.text = this.online.Value;
                this.stateText.color = this.onlineColor;
            }
        }

        public void UserOutBattle(bool userOnline)
        {
            this.userInBattle = false;
            if (userOnline)
            {
                this.UserOnline();
            }
            else
            {
                this.UserOffline();
            }
        }

        public bool CanBeInvited
        {
            set
            {
                if (this.inviteButton != null)
                {
                    this.inviteButton.GetComponent<Button>().interactable = value;
                }
            }
        }

        public bool UserInSquad
        {
            set
            {
                if (this.userInSquadLabel != null)
                {
                    this.userInSquadLabel.gameObject.SetActive(value);
                }
            }
        }

        public bool DisableInviteOnlyForSquadState =>
            this.disableInviteOnlyForSquadState;
    }
}

