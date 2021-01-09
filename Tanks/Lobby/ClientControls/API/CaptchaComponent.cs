namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class CaptchaComponent : BehaviourComponent
    {
        [SerializeField]
        private Image captchaImage;
        [SerializeField]
        private UnityEngine.Animator animator;

        public Sprite CaptchaSprite
        {
            get => 
                this.captchaImage.sprite;
            set => 
                this.captchaImage.sprite = value;
        }

        public UnityEngine.Animator Animator =>
            this.animator;
    }
}

