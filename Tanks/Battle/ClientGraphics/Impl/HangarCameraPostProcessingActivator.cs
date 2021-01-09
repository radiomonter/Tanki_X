namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;
    using UnityEngine.PostProcessing;

    public class HangarCameraPostProcessingActivator : MonoBehaviour
    {
        public PostProcessingProfile profile;
        private float saturation;
        [SerializeField]
        private MonoBehaviour Bloom;
        [SerializeField]
        private MonoBehaviour Fog;
        [SerializeField]
        private MonoBehaviour TargetBloom;
        public static GameObject ActivePanel;
        public float FocusDistance;
        public Animator blurAnimator;
        private bool lowRenderResolution;
        private int screenWidth;
        private int screenHeight;
        private RenderTexture texture;
        private float renderCoeff = 0.7f;

        private void CreateRenderTexture()
        {
            this.screenWidth = Screen.width;
            this.screenHeight = Screen.height;
            this.texture = new RenderTexture((int) (Screen.width * this.renderCoeff), (int) (Screen.height * this.renderCoeff), 0);
            RenderTexture.active = this.texture;
            QualitySettings.pixelLightCount = 0;
        }

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
                this.profile.depthOfField.enabled = qualityLevel >= 2;
                this.Fog.enabled = qualityLevel >= 2;
                this.profile.colorGrading.enabled = true;
                ColorGradingModel.Settings settings = this.profile.colorGrading.settings;
                this.saturation = (GraphicsSettings.INSTANCE != null) ? GraphicsSettings.INSTANCE.CurrentSaturationLevel : 1f;
                settings.basic.saturation = this.saturation;
                this.profile.colorGrading.settings = settings;
                if ((GraphicsSettings.INSTANCE != null) && GraphicsSettings.INSTANCE.customSettings)
                {
                    this.profile.antialiasing.enabled = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality != 0;
                    if (this.profile.antialiasing.enabled)
                    {
                        AntialiasingModel.Settings settings2 = this.profile.antialiasing.settings;
                        settings2.method = (qualityLevel <= 3) ? AntialiasingModel.Method.Fxaa : AntialiasingModel.Method.Taa;
                        this.profile.antialiasing.settings = settings2;
                    }
                    this.profile.ambientOcclusion.enabled = GraphicsSettings.INSTANCE.currentAmbientOcclusion;
                    this.profile.bloom.enabled = GraphicsSettings.INSTANCE.currentBloom;
                    this.TargetBloom.enabled = GraphicsSettings.INSTANCE.currentBloom;
                    this.profile.chromaticAberration.enabled = GraphicsSettings.INSTANCE.currentChromaticAberration;
                    this.profile.grain.enabled = GraphicsSettings.INSTANCE.currentGrain;
                    this.profile.vignette.enabled = GraphicsSettings.INSTANCE.currentVignette;
                }
                else
                {
                    this.profile.antialiasing.enabled = qualityLevel >= 2;
                    if (this.profile.antialiasing.enabled)
                    {
                        AntialiasingModel.Settings settings3 = this.profile.antialiasing.settings;
                        settings3.method = (qualityLevel <= 3) ? AntialiasingModel.Method.Fxaa : AntialiasingModel.Method.Taa;
                        this.profile.antialiasing.settings = settings3;
                    }
                    this.profile.chromaticAberration.enabled = qualityLevel >= 2;
                    this.profile.bloom.enabled = qualityLevel >= 3;
                    this.TargetBloom.enabled = qualityLevel >= 3;
                    this.profile.grain.enabled = qualityLevel >= 3;
                    this.profile.vignette.enabled = qualityLevel >= 3;
                    this.profile.ambientOcclusion.enabled = qualityLevel >= 4;
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
                if ((this.screenWidth != Screen.width) || (this.screenHeight != Screen.height))
                {
                    this.CreateRenderTexture();
                }
            }
        }

        private void Start()
        {
            this.lowRenderResolution = ((GraphicsSettings.INSTANCE == null) || !GraphicsSettings.INSTANCE.customSettings) ? (QualitySettings.GetQualityLevel() == 0) : (GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality == 1);
            if (this.lowRenderResolution)
            {
                this.CreateRenderTexture();
            }
        }

        private void Update()
        {
            if ((GraphicsSettings.INSTANCE != null) && (this.saturation != GraphicsSettings.INSTANCE.CurrentSaturationLevel))
            {
                ColorGradingModel.Settings settings = this.profile.colorGrading.settings;
                this.saturation = GraphicsSettings.INSTANCE.CurrentSaturationLevel;
                settings.basic.saturation = this.saturation;
                this.profile.colorGrading.settings = settings;
            }
            if ((ActivePanel != null) && ActivePanel.activeInHierarchy)
            {
                this.blurAnimator.SetBool("Blur", true);
            }
            else
            {
                this.blurAnimator.SetBool("Blur", false);
            }
        }
    }
}

