namespace Tanks.Lobby.ClientProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class SettingsScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI header;
        [SerializeField]
        private TextMeshProUGUI gameSettings;
        [SerializeField]
        private TextMeshProUGUI soundSettings;
        [SerializeField]
        private TextMeshProUGUI languageSettings;
        [SerializeField]
        private TextMeshProUGUI graphicsSettings;
        [SerializeField]
        private TextMeshProUGUI keyboardSettings;

        public virtual string Header
        {
            set => 
                this.header.text = value;
        }

        public virtual string GameSettings
        {
            set => 
                this.gameSettings.text = value;
        }

        public virtual string SoundSettings
        {
            set => 
                this.soundSettings.text = value;
        }

        public virtual string LanguageSettings
        {
            set => 
                this.languageSettings.text = value;
        }

        public virtual string GraphicsSettings
        {
            set => 
                this.graphicsSettings.text = value;
        }

        public virtual string KeyboardSettings
        {
            set => 
                this.keyboardSettings.text = value;
        }
    }
}

