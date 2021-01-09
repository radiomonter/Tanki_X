namespace Tanks.Lobby.ClientProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class GameSettingsScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI cameraShakerEnabled;
        [SerializeField]
        private TextMeshProUGUI targetFocusEnabled;
        [SerializeField]
        private TextMeshProUGUI laserSightEnabled;
        [SerializeField]
        private TextMeshProUGUI damageInfo;
        [SerializeField]
        private TextMeshProUGUI healthFeedback;
        [SerializeField]
        private TextMeshProUGUI selfTargetHitFeedback;
        [SerializeField]
        private TextMeshProUGUI disableNotificationsText;

        public string CameraShakerEnabled
        {
            set => 
                this.cameraShakerEnabled.text = value;
        }

        public string TargetFocusEnabled
        {
            set => 
                this.targetFocusEnabled.text = value;
        }

        public string LaserSightEnabled
        {
            set => 
                this.laserSightEnabled.text = value;
        }

        public string DamageInfo
        {
            set => 
                this.damageInfo.text = value;
        }

        public string HealthFeedback
        {
            set => 
                this.healthFeedback.text = value;
        }

        public string SelfTargetHitFeedback
        {
            set => 
                this.selfTargetHitFeedback.text = value;
        }

        public string DisableNotificationsText
        {
            set => 
                this.disableNotificationsText.text = value;
        }
    }
}

