namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [SerialVersionUID(0x8d2e6e0dcfd812aL)]
    public class EntranceScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private TMP_InputField loginField;
        [SerializeField]
        private TMP_InputField passwordField;
        [SerializeField]
        private TMP_InputField captchaField;
        [SerializeField]
        private GameObject captcha;
        [SerializeField]
        private Toggle rememberMeCheckbox;
        [SerializeField]
        private TextMeshProUGUI serverStatusLabel;
        public GameObject loginText;
        public GameObject loginWaitIndicator;
        public GameObject locker;
        [SerializeField]
        private TextMeshProUGUI enterNameOrEmail;
        [SerializeField]
        private TextMeshProUGUI enterPassword;
        [SerializeField]
        private TextMeshProUGUI rememberMe;
        [SerializeField]
        private TextMeshProUGUI play;

        public virtual void ActivateCaptcha()
        {
            this.captchaField.gameObject.SetActive(true);
            this.captcha.SetActive(true);
            base.GetComponent<Animator>().SetBool("captcha", true);
        }

        public void LockScreen(bool value)
        {
            this.locker.SetActive(value);
        }

        protected void OnEnable()
        {
            this.LockScreen(false);
            this.captcha.SetActive(false);
            this.captchaField.gameObject.SetActive(false);
        }

        public void SetOfflineStatus()
        {
            this.SetServerStatus("Offline", "#E93A3AFF");
            Debug.Log("Set OFFLINE");
        }

        public void SetOnlineStatus()
        {
            this.SetServerStatus("Online", "#B6FF19FF");
        }

        private void SetServerStatus(string text, string color)
        {
            string str = LocalizationUtils.Localize("d2788af7-8f66-4445-8154-d1e9c04af353");
            string[] textArray1 = new string[] { str, ": <color=", color, ">", text, "</color>" };
            this.serverStatus = string.Concat(textArray1);
        }

        public void SetWaitIndicator(bool wait)
        {
            this.loginText.SetActive(!wait);
            this.loginWaitIndicator.SetActive(wait);
        }

        public virtual string Login
        {
            get => 
                this.loginField.text;
            set => 
                this.loginField.text = value;
        }

        public virtual string Password
        {
            get => 
                this.passwordField.text;
            set => 
                this.passwordField.text = value;
        }

        public virtual string Captcha
        {
            get => 
                this.captchaField.text;
            set => 
                this.captchaField.text = value;
        }

        public virtual bool RememberMe
        {
            get => 
                this.rememberMeCheckbox.isOn;
            set => 
                this.rememberMeCheckbox.isOn = value;
        }

        public virtual string serverStatus
        {
            get => 
                this.serverStatusLabel.text;
            set => 
                this.serverStatusLabel.text = value;
        }

        public string EnterNameOrEmail
        {
            set => 
                this.enterNameOrEmail.text = value;
        }

        public string EnterPassword
        {
            set => 
                this.enterPassword.text = value;
        }

        public string RememberMeCheckbox
        {
            set => 
                this.rememberMe.text = value;
        }

        public string Play
        {
            set => 
                this.play.text = value;
        }

        public string IncorrectPassword { get; set; }

        public string IncorrectCaptcha { get; set; }

        public string IncorrectLogin { get; set; }

        public string UnconfirmedEmail { get; set; }
    }
}

