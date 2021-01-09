namespace Tanks.Lobby.ClientSettings.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class SelectLocaleScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI hint;
        [SerializeField]
        private Text currentLanguage;

        public string Hint
        {
            set => 
                this.hint.text = value;
        }

        public string CurrentLanguage
        {
            set => 
                this.currentLanguage.text = value;
        }
    }
}

