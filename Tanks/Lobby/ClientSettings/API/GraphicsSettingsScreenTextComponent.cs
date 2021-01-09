namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class GraphicsSettingsScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI reloadText;
        [SerializeField]
        private TextMeshProUGUI perfomanceChangeText;
        [SerializeField]
        private TextMeshProUGUI currentPerfomanceText;
        [SerializeField]
        private TextMeshProUGUI windowModeText;
        [SerializeField]
        private TextMeshProUGUI resolutionText;
        [SerializeField]
        private TextMeshProUGUI qualityText;
        [SerializeField]
        private TextMeshProUGUI saturationLevelText;
        [SerializeField]
        private TextMeshProUGUI renderResolutionQualityText;
        [SerializeField]
        private TextMeshProUGUI antialiasingQualityText;
        [SerializeField]
        private TextMeshProUGUI textureQualityText;
        [SerializeField]
        private TextMeshProUGUI shadowQualityText;
        [SerializeField]
        private TextMeshProUGUI particleQualityText;
        [SerializeField]
        private TextMeshProUGUI anisotropicQualityText;
        [SerializeField]
        private TextMeshProUGUI customSettingsModeText;
        [SerializeField]
        private TextMeshProUGUI ambientOccluisonModeText;
        [SerializeField]
        private TextMeshProUGUI bloomModeText;
        [SerializeField]
        private TextMeshProUGUI chromaticAberrationModeText;
        [SerializeField]
        private TextMeshProUGUI grainModeText;
        [SerializeField]
        private TextMeshProUGUI vignetteModeText;
        [SerializeField]
        private TextMeshProUGUI vegetationQualityText;
        [SerializeField]
        private TextMeshProUGUI grassQualityText;
        [SerializeField]
        private TextMeshProUGUI cartridgeCaseAmountText;
        [SerializeField]
        private TextMeshProUGUI vSyncQualityText;

        public string ReloadText
        {
            set => 
                this.reloadText.text = value;
        }

        public string PerfomanceChangeText
        {
            set => 
                this.perfomanceChangeText.text = value;
        }

        public string CurrentPerfomanceText
        {
            set => 
                this.currentPerfomanceText.text = value;
        }

        public string WindowModeText
        {
            set => 
                this.windowModeText.text = value;
        }

        public string ScreenResolutionText
        {
            set => 
                this.resolutionText.text = value;
        }

        public string QualityLevelText
        {
            set => 
                this.qualityText.text = value;
        }

        public string SaturationLevelText
        {
            set => 
                this.saturationLevelText.text = value;
        }

        public string RenderResolutionQualityText
        {
            set => 
                this.renderResolutionQualityText.text = value;
        }

        public string AntialiasingQualityText
        {
            set => 
                this.antialiasingQualityText.text = value;
        }

        public string TextureQualityText
        {
            set => 
                this.textureQualityText.text = value;
        }

        public string ShadowQualityText
        {
            set => 
                this.shadowQualityText.text = value;
        }

        public string ParticleQualityText
        {
            set => 
                this.particleQualityText.text = value;
        }

        public string AnisotropicQualityText
        {
            set => 
                this.anisotropicQualityText.text = value;
        }

        public string CustomSettingsModeText
        {
            set => 
                this.customSettingsModeText.text = value;
        }

        public string AmbientOccluisonModeText
        {
            set => 
                this.ambientOccluisonModeText.text = value;
        }

        public string BloomModeText
        {
            set => 
                this.bloomModeText.text = value;
        }

        public string ChromaticAberrationModeText
        {
            set => 
                this.chromaticAberrationModeText.text = value;
        }

        public string GrainModeText
        {
            set => 
                this.grainModeText.text = value;
        }

        public string VignetteModeText
        {
            set => 
                this.vignetteModeText.text = value;
        }

        public string VegetationQualityText
        {
            set => 
                this.vegetationQualityText.text = value;
        }

        public string GrassQualityText
        {
            set => 
                this.grassQualityText.text = value;
        }

        public string CartridgeCaseAmountText
        {
            set => 
                this.cartridgeCaseAmountText.text = value;
        }

        public string VSyncQualityText
        {
            set => 
                this.vSyncQualityText.text = value;
        }
    }
}

