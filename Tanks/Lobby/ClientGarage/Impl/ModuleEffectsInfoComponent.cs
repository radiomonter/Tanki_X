namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ModuleEffectsInfoComponent : UIBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI effectText;
        [SerializeField]
        private ImageSkin effectIcon;
        [SerializeField]
        private PaletteColorField exceptionalColor;
        [SerializeField]
        private PaletteColorField epicColor;
        [SerializeField]
        private Image staticIcon;

        public ImageSkin EffectIcon =>
            this.effectIcon;

        public string EffectValue
        {
            set => 
                this.effectText.text = value;
        }

        public Color ExceptionalColor =>
            (Color) this.exceptionalColor;

        public Color EpicColor =>
            (Color) this.epicColor;

        public Image StaticIcon =>
            this.staticIcon;

        public TextMeshProUGUI EffectText =>
            this.effectText;
    }
}

