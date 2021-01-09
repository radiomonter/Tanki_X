namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class VolumeChangedNotifierComponent : BehaviourComponent, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        [SerializeField]
        private UnityEngine.UI.Slider slider;
        [SerializeField]
        private UnityEngine.AudioSource audioSource;

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!this.slider.minValue.Equals(this.slider.value))
            {
                this.audioSource.outputAudioMixerGroup.audioMixer.SetFloat(SoundSettingsUtils.VOLUME_PARAM_KEY, this.slider.value);
                this.audioSource.Play();
            }
        }

        public UnityEngine.UI.Slider Slider
        {
            get => 
                this.slider;
            set => 
                this.slider = value;
        }

        public UnityEngine.AudioSource AudioSource
        {
            get => 
                this.audioSource;
            set => 
                this.audioSource = value;
        }
    }
}

