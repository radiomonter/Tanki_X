namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LobbyUserListItemComponent : BehaviourComponent
    {
        public Entity userEntity;
        public bool selfUser;
        public GameObject userInfo;
        public GameObject userSearchingText;
        public GameObject userLabelPrefab;
        private GameObject userLabelInstance;
        [SerializeField]
        private TextMeshProUGUI turretName;
        [SerializeField]
        private TextMeshProUGUI hullName;
        [SerializeField]
        private TextMeshProUGUI turretIcon;
        [SerializeField]
        private TextMeshProUGUI hullIcon;
        [SerializeField]
        private GameObject readyButton;
        [SerializeField]
        private TextMeshProUGUI statusLabel;
        [SerializeField]
        private LocalizedField notReadyText;
        [SerializeField]
        private LocalizedField readyText;
        [SerializeField]
        private LocalizedField inBattleText;
        [SerializeField]
        private Color notReadyColor;
        [SerializeField]
        private Color readyColor;
        [SerializeField]
        private GameObject lobbyMasterIcon;
        [SerializeField]
        private Button buttonScript;
        private TeamListUserData currentUserData;
        private bool showSearchingText = true;

        private void Init()
        {
            this.SetNotReady();
            if (this.Empty)
            {
                this.userSearchingText.SetActive(this.showSearchingText);
                this.userInfo.SetActive(false);
            }
            else
            {
                if (!this.userEntity.HasComponent<LobbyUserListItemComponent>())
                {
                    this.userEntity.AddComponent(this);
                }
                if (!this.userEntity.HasComponent<UserSquadColorComponent>())
                {
                    UserSquadColorComponent component = base.GetComponent<UserSquadColorComponent>();
                    this.userEntity.AddComponent(component);
                }
                if (this.userLabelInstance != null)
                {
                    Destroy(this.userLabelInstance);
                }
                this.userLabelInstance = Instantiate<GameObject>(this.userLabelPrefab);
                bool premium = this.userEntity.HasComponent<PremiumAccountBoostComponent>();
                UserLabelBuilder builder = new UserLabelBuilder(this.userEntity.Id, this.userLabelInstance, this.userEntity.GetComponent<UserAvatarComponent>().Id, premium);
                this.userLabelInstance = builder.SkipLoadUserFromServer().Build();
                this.userLabelInstance.transform.SetParent(this.turretName.transform.parent, false);
                this.userLabelInstance.transform.SetSiblingIndex(0);
            }
            bool flag2 = !this.Empty && !this.selfUser;
            if (this.buttonScript != null)
            {
                this.buttonScript.interactable = flag2;
            }
            RightMouseButtonClickSender sender = base.GetComponent<RightMouseButtonClickSender>();
            if ((sender != null) && !flag2)
            {
                sender.enabled = false;
            }
        }

        private void OnDisable()
        {
            if (!this.Empty && ClientUnityIntegrationUtils.HasEngine())
            {
                if (this.userEntity.HasComponent<LobbyUserListItemComponent>())
                {
                    this.userEntity.RemoveComponent<LobbyUserListItemComponent>();
                }
                if (this.userEntity.HasComponent<UserSquadColorComponent>())
                {
                    this.userEntity.RemoveComponent<UserSquadColorComponent>();
                }
            }
        }

        private void OnEnable()
        {
            this.Init();
        }

        public void Select()
        {
        }

        public void SetInBattle()
        {
            this.readyButton.SetActive(false);
            this.statusLabel.gameObject.SetActive(true);
            this.statusLabel.text = (string) this.inBattleText;
            this.statusLabel.color = this.readyColor;
        }

        public void SetNotReady()
        {
            if (this.selfUser)
            {
                this.ShowReadyButton();
            }
            else
            {
                this.readyButton.SetActive(false);
                this.statusLabel.gameObject.SetActive(true);
                this.statusLabel.text = (string) this.notReadyText;
                this.statusLabel.color = this.notReadyColor;
            }
        }

        public void SetReady()
        {
            this.readyButton.SetActive(false);
            this.statusLabel.gameObject.SetActive(true);
            this.statusLabel.text = (string) this.readyText;
            this.statusLabel.color = this.readyColor;
        }

        public void ShowReadyButton()
        {
            this.statusLabel.gameObject.SetActive(false);
            this.readyButton.SetActive(true);
            this.statusLabel.text = string.Empty;
            this.statusLabel.color = this.notReadyColor;
        }

        public void UpdateEquipment(string hullName, long hullIconId, string turretName, long turretIconId)
        {
            this.turretName.text = turretName;
            this.turretIcon.text = "<sprite name=\"" + turretIconId + "\">";
            this.hullName.text = hullName;
            this.hullIcon.text = "<sprite name=\"" + hullIconId + "\">";
        }

        public bool Empty =>
            ReferenceEquals(this.userEntity, null);

        public bool ShowSearchingText
        {
            get => 
                this.showSearchingText;
            set
            {
                this.showSearchingText = value;
                this.userSearchingText.SetActive(this.Empty && this.showSearchingText);
            }
        }

        public bool Master
        {
            set => 
                this.lobbyMasterIcon.SetActive(value);
        }
    }
}

