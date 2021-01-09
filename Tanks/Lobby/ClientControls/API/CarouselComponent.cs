namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class CarouselComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private CarouselButtonComponent backButton;
        [SerializeField]
        private CarouselButtonComponent frontButton;
        [SerializeField, HideInInspector]
        private int templateIdLow;
        [SerializeField, HideInInspector]
        private int templateIdHigh;

        public long ItemTemplateId
        {
            get => 
                (this.templateIdHigh << 0x20) | ((ulong) this.templateIdLow);
            set
            {
                this.templateIdLow = (int) (((ulong) value) & 0xffffffffUL);
                this.templateIdHigh = (int) (value >> 0x20);
            }
        }

        public TextMeshProUGUI Text =>
            this.text;

        public CarouselButtonComponent BackButton =>
            this.backButton;

        public CarouselButtonComponent FrontButton =>
            this.frontButton;
    }
}

