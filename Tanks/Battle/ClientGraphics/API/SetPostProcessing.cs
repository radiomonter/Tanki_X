namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.PostProcessing;
    using UnityStandardAssets.ImageEffects;

    public class SetPostProcessing : MonoBehaviour
    {
        private PostProcessingProfile profile;
        [SerializeField]
        private MonoBehaviour Bloom;
        [SerializeField]
        private MonoBehaviour Fog;
        [SerializeField]
        private MonoBehaviour TargetBloom;
        public bool forcedFog;
        public bool lowRenderResolution;
        private ColorCorrectionCurves colorCorrection;
        private RenderTexture texture;
        private float renderCoeff = 0.7f;

        private void DisableAllEffects(PostProcessingProfile postProcessingProfile)
        {
            this.Bloom.enabled = false;
            this.Fog.enabled = false;
            this.TargetBloom.enabled = false;
            postProcessingProfile.ambientOcclusion.enabled = false;
            postProcessingProfile.antialiasing.enabled = false;
            postProcessingProfile.bloom.enabled = false;
            postProcessingProfile.chromaticAberration.enabled = false;
            postProcessingProfile.colorGrading.enabled = false;
            postProcessingProfile.debugViews.enabled = false;
            postProcessingProfile.depthOfField.enabled = false;
            postProcessingProfile.dithering.enabled = false;
            postProcessingProfile.eyeAdaptation.enabled = false;
            postProcessingProfile.fog.enabled = false;
            postProcessingProfile.grain.enabled = false;
            postProcessingProfile.motionBlur.enabled = false;
            postProcessingProfile.screenSpaceReflection.enabled = false;
            postProcessingProfile.userLut.enabled = false;
            postProcessingProfile.vignette.enabled = false;
        }

        private void OnEnable()
        {
            PostProcessingBehaviour component = base.GetComponent<PostProcessingBehaviour>();
            if (component.profile == null)
            {
                base.enabled = false;
            }
            else
            {
                this.profile = Instantiate<PostProcessingProfile>(component.profile);
                component.profile = this.profile;
                this.DisableAllEffects(this.profile);
                int qualityLevel = QualitySettings.GetQualityLevel();
                this.Fog.enabled = ((qualityLevel >= 2) && (GraphicsSettings.INSTANCE != null)) && (GraphicsSettings.INSTANCE.CurrentShadowQuality != 0);
                this.profile.userLut.enabled = qualityLevel >= 2;
                this.profile.colorGrading.enabled = qualityLevel >= 2;
                if (this.profile.colorGrading.enabled)
                {
                    ColorGradingModel.Settings settings = this.profile.colorGrading.settings;
                    settings.basic.saturation = (GraphicsSettings.INSTANCE != null) ? GraphicsSettings.INSTANCE.CurrentSaturationLevel : 1f;
                    this.profile.colorGrading.settings = settings;
                }
                if ((GraphicsSettings.INSTANCE != null) && GraphicsSettings.INSTANCE.customSettings)
                {
                    this.SetCustomSettings();
                }
                else
                {
                    this.SetDefaultSettings();
                }
            }
        }

        private void OnPostRender()
        {
            if (this.lowRenderResolution)
            {
                Camera.main.targetTexture = null;
            }
        }

        private void OnPreRender()
        {
            if (this.lowRenderResolution)
            {
                Camera.main.targetTexture = this.texture;
            }
        }

        private void SetCustomSettings()
        {
            this.lowRenderResolution = GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality == 1;
            if (this.lowRenderResolution)
            {
                this.texture = new RenderTexture((int) (Screen.width * this.renderCoeff), (int) (Screen.height * this.renderCoeff), 0);
                RenderTexture.active = this.texture;
            }
            this.Bloom.enabled = GraphicsSettings.INSTANCE.currentBloom;
            this.TargetBloom.enabled = GraphicsSettings.INSTANCE.currentBloom;
            if (GraphicsSettings.INSTANCE.currentBloom)
            {
                this.profile.colorGrading.enabled = true;
                this.profile.userLut.enabled = true;
            }
            this.profile.antialiasing.enabled = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality == 1;
            if (this.profile.antialiasing.enabled)
            {
                AntialiasingModel.Settings settings = this.profile.antialiasing.settings;
                settings.method = (QualitySettings.GetQualityLevel() <= 3) ? AntialiasingModel.Method.Fxaa : AntialiasingModel.Method.Taa;
                this.profile.antialiasing.settings = settings;
            }
            this.profile.ambientOcclusion.enabled = GraphicsSettings.INSTANCE.currentAmbientOcclusion;
            this.profile.chromaticAberration.enabled = GraphicsSettings.INSTANCE.currentChromaticAberration;
            this.profile.grain.enabled = GraphicsSettings.INSTANCE.currentGrain;
            this.profile.vignette.enabled = GraphicsSettings.INSTANCE.currentVignette;
        }

        private void SetDefaultSettings()
        {
            int qualityLevel = QualitySettings.GetQualityLevel();
            if (qualityLevel != 0)
            {
                this.lowRenderResolution = false;
            }
            else
            {
                this.lowRenderResolution = true;
                this.texture = new RenderTexture((int) (Screen.width * this.renderCoeff), (int) (Screen.height * this.renderCoeff), 0);
                RenderTexture.active = this.texture;
                QualitySettings.pixelLightCount = 0;
                base.GetComponent<Camera>().hdr = false;
            }
            this.TargetBloom.enabled = qualityLevel >= 3;
            this.Bloom.enabled = qualityLevel >= 3;
            this.profile.chromaticAberration.enabled = qualityLevel >= 2;
            this.profile.antialiasing.enabled = qualityLevel >= 3;
            if (this.profile.antialiasing.enabled)
            {
                AntialiasingModel.Settings settings = this.profile.antialiasing.settings;
                settings.method = (qualityLevel <= 3) ? AntialiasingModel.Method.Fxaa : AntialiasingModel.Method.Taa;
                this.profile.antialiasing.settings = settings;
            }
            this.profile.vignette.enabled = qualityLevel >= 3;
            this.profile.ambientOcclusion.enabled = qualityLevel >= 5;
        }
    }
}

