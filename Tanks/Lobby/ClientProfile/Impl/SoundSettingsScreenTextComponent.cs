namespace Tanks.Lobby.ClientProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class SoundSettingsScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI sFXVolume;
        [SerializeField]
        private TextMeshProUGUI musicVolume;
        [SerializeField]
        private TextMeshProUGUI uIVolume;

        public string SFXVolume
        {
            set => 
                this.sFXVolume.text = value;
        }

        public string MusicVolume
        {
            set => 
                this.musicVolume.text = value;
        }

        public string UIVolume
        {
            set => 
                this.uIVolume.text = value;
        }
    }
}

