namespace Tanks.Lobby.ClientProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class KeyboardSettingsScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI mouseSensivity;
        [SerializeField]
        private TextMeshProUGUI invertBackwardMovingControl;
        [SerializeField]
        private TextMeshProUGUI mouseControlAllowed;
        [SerializeField]
        private TextMeshProUGUI mouseVerticalInverted;
        [SerializeField]
        private TextMeshProUGUI keyboardHeader;
        [SerializeField]
        private TextMeshProUGUI tip;
        [SerializeField]
        private TextMeshProUGUI fewActionsError;
        [SerializeField]
        private TextMeshProUGUI modules;
        [SerializeField]
        private Text turretLeft;
        [SerializeField]
        private Text turretRight;
        [SerializeField]
        private Text centerTurret;
        [SerializeField]
        private Text graffiti;
        [SerializeField]
        private Text help;
        [SerializeField]
        private Text changeHud;
        [SerializeField]
        private Text dropFlag;
        [SerializeField]
        private Text cameraUp;
        [SerializeField]
        private Text cameraDown;
        [SerializeField]
        private Text scoreBoard;
        [SerializeField]
        private Text selfDestruction;
        [SerializeField]
        private Text pause;

        public string MouseSensivity
        {
            set => 
                this.mouseSensivity.text = value;
        }

        public string InvertBackwardMovingControl
        {
            set => 
                this.invertBackwardMovingControl.text = value;
        }

        public string MouseControlAllowed
        {
            set => 
                this.mouseControlAllowed.text = value;
        }

        public string MouseVerticalInverted
        {
            set => 
                this.mouseVerticalInverted.text = value;
        }

        public string Keyboard
        {
            set => 
                this.keyboardHeader.text = value;
        }

        public string Tip
        {
            set => 
                this.tip.text = value;
        }

        public string FewActionsError
        {
            set => 
                this.fewActionsError.text = value;
        }

        public string Modules
        {
            set => 
                this.modules.text = value;
        }

        public virtual string TurretLeft
        {
            set => 
                this.turretLeft.text = value;
        }

        public virtual string TurretRight
        {
            set => 
                this.turretRight.text = value;
        }

        public virtual string CenterTurret
        {
            set => 
                this.centerTurret.text = value;
        }

        public virtual string Graffiti
        {
            set => 
                this.graffiti.text = value;
        }

        public virtual string Help
        {
            set => 
                this.help.text = value;
        }

        public virtual string ChangeHud
        {
            set => 
                this.changeHud.text = value;
        }

        public virtual string DropFlag
        {
            set => 
                this.dropFlag.text = value;
        }

        public virtual string CameraUp
        {
            set => 
                this.cameraUp.text = value;
        }

        public virtual string CameraDown
        {
            set => 
                this.cameraDown.text = value;
        }

        public virtual string ScoreBoard
        {
            set => 
                this.scoreBoard.text = value;
        }

        public virtual string SelfDestruction
        {
            set => 
                this.selfDestruction.text = value;
        }

        public virtual string Pause
        {
            set => 
                this.pause.text = value;
        }
    }
}

