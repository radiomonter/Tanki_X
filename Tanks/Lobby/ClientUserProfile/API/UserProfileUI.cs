namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserProfileUI : BehaviourComponent
    {
        [SerializeField]
        private Slider expProgress;
        [SerializeField]
        private TextMeshProUGUI level;
        [SerializeField]
        private TextMeshProUGUI nextLevel;
        [SerializeField]
        private TextMeshProUGUI expValue;
        [SerializeField]
        private TextMeshProUGUI nickname;
        [SerializeField]
        private GameObject createSquadButton;
        [SerializeField]
        private GameObject cancelButton;
        [SerializeField]
        private LocalizedField expValueLocalizedField;

        private void OnEnable()
        {
            if (SelfUserComponent.SelfUser != null)
            {
                this.UpdateNickname();
                LevelInfo info = this.SendEvent<GetUserLevelInfoEvent>(SelfUserComponent.SelfUser).Info;
                this.level.text = (info.Level + 1).ToString();
                this.nextLevel.text = (info.Experience >= info.MaxExperience) ? string.Empty : (info.Level + 2).ToString();
                this.expProgress.value = info.Progress;
                this.expValue.text = string.Format(this.expValueLocalizedField.Value, info.Experience, info.MaxExperience);
            }
        }

        public void SwitchButtons(bool showCreateSquadButton)
        {
            this.createSquadButton.SetActive(showCreateSquadButton);
            this.cancelButton.SetActive(!showCreateSquadButton);
        }

        public void UpdateNickname()
        {
            this.nickname.text = SelfUserComponent.SelfUser.GetComponent<UserUidComponent>().Uid;
        }

        public bool CanCreateSquad
        {
            set => 
                this.createSquadButton.GetComponent<Button>().interactable = value;
        }
    }
}

