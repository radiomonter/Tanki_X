namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class GraphicsSettingsActivator : UnityAwareActivator<AutoCompleting>
    {
        [SerializeField]
        private float defaultSaturationLevel = 0.6f;
        [SerializeField]
        private int defaultVegetationLevel;
        [SerializeField]
        private int defaultGrassLevel;
        [SerializeField]
        private int defaultAntialiasingQuality;
        [SerializeField]
        private int defaultRenderResolutionQuality;
        [SerializeField]
        private int defaultAnisotropicQuality;
        [SerializeField]
        private int defaultTextureQuality;
        [SerializeField]
        private int defaultShadowQuality;
        [SerializeField]
        private int defaultParticleQuality;
        [SerializeField]
        private int defaultCartridgeCaseAmount;
        [SerializeField]
        private int defaultVsyncQuality = 1;
        [SerializeField]
        private bool isWindowedByDefault;
        [SerializeField]
        private int minHeight = 0x2b8;
        [SerializeField]
        private int minWidth = 0x400;
        [SerializeField]
        private string configPath;
        [SerializeField]
        private GraphicsSettingsAnalyzer graphicsSettingsAnalyzer;

        protected override void Activate()
        {
            Quality.ValidateQualities();
            this.graphicsSettingsAnalyzer.Init();
            GraphicsSettings graphicsSettings = new GraphicsSettings();
            GraphicsSettings.INSTANCE = graphicsSettings;
            graphicsSettings.InitWindowModeSettings(this.isWindowedByDefault);
            graphicsSettings.InitQualitySettings(this.graphicsSettingsAnalyzer.GetDefaultQuality(), this.UltraEnabled());
            Quality defaultQuality = this.graphicsSettingsAnalyzer.GetDefaultQuality();
            if (defaultQuality.Level == 0)
            {
                this.defaultGrassLevel = 0;
                this.defaultShadowQuality = 0;
                this.defaultParticleQuality = 0;
                this.defaultAnisotropicQuality = 0;
                this.defaultTextureQuality = 0;
                this.defaultVegetationLevel = 0;
                this.defaultAntialiasingQuality = 0;
                this.defaultRenderResolutionQuality = 1;
                this.defaultCartridgeCaseAmount = 0;
                this.defaultVsyncQuality = 1;
            }
            if (defaultQuality.Level == 1)
            {
                this.defaultGrassLevel = 0;
                this.defaultShadowQuality = 0;
                this.defaultParticleQuality = 1;
                this.defaultAnisotropicQuality = 0;
                this.defaultTextureQuality = 0;
                this.defaultVegetationLevel = 0;
                this.defaultAntialiasingQuality = 0;
                this.defaultRenderResolutionQuality = 0;
                this.defaultCartridgeCaseAmount = 0;
                this.defaultVsyncQuality = 1;
            }
            if (defaultQuality.Level == 2)
            {
                this.defaultGrassLevel = 1;
                this.defaultShadowQuality = 1;
                this.defaultParticleQuality = 2;
                this.defaultAnisotropicQuality = 0;
                this.defaultTextureQuality = 1;
                this.defaultVegetationLevel = 1;
                this.defaultAntialiasingQuality = 0;
                this.defaultRenderResolutionQuality = 0;
                this.defaultCartridgeCaseAmount = 0;
                this.defaultVsyncQuality = 1;
            }
            if (defaultQuality.Level == 3)
            {
                this.defaultGrassLevel = 2;
                this.defaultShadowQuality = 2;
                this.defaultParticleQuality = 3;
                this.defaultAnisotropicQuality = 1;
                this.defaultTextureQuality = 1;
                this.defaultVegetationLevel = 2;
                this.defaultAntialiasingQuality = 0;
                this.defaultRenderResolutionQuality = 0;
                this.defaultCartridgeCaseAmount = 1;
                this.defaultVsyncQuality = 1;
            }
            if (defaultQuality.Level == 4)
            {
                this.defaultGrassLevel = 3;
                this.defaultShadowQuality = 3;
                this.defaultParticleQuality = 4;
                this.defaultAnisotropicQuality = 2;
                this.defaultTextureQuality = 1;
                this.defaultVegetationLevel = 3;
                this.defaultAntialiasingQuality = 1;
                this.defaultRenderResolutionQuality = 0;
                this.defaultCartridgeCaseAmount = 2;
                this.defaultVsyncQuality = 1;
            }
            if (defaultQuality.Level == 5)
            {
                this.defaultGrassLevel = 4;
                this.defaultShadowQuality = 4;
                this.defaultParticleQuality = 5;
                this.defaultAnisotropicQuality = 2;
                this.defaultTextureQuality = 1;
                this.defaultVegetationLevel = 4;
                this.defaultAntialiasingQuality = 1;
                this.defaultRenderResolutionQuality = 0;
                this.defaultCartridgeCaseAmount = 3;
                this.defaultVsyncQuality = 1;
            }
            this.DefineScreenResolutionData(graphicsSettings);
            graphicsSettings.InitSaturationLevelSettings(this.defaultSaturationLevel);
            graphicsSettings.InitAnisotropicQualitySettings(this.defaultAnisotropicQuality);
            graphicsSettings.InitRenderResolutionQualitySettings(this.defaultRenderResolutionQuality);
            graphicsSettings.InitAntialiasingQualitySettings(this.defaultAntialiasingQuality);
            graphicsSettings.InitShadowQualitySettings(this.defaultShadowQuality);
            graphicsSettings.InitParticleQualitySettings(this.defaultParticleQuality);
            graphicsSettings.InitTextureQualitySettings(this.defaultTextureQuality);
            graphicsSettings.InitVegetationLevelSettings(this.defaultVegetationLevel);
            graphicsSettings.InitGrassLevelSettings(this.defaultGrassLevel);
            graphicsSettings.InitCartridgeCaseAmount(this.defaultCartridgeCaseAmount);
            graphicsSettings.InitVSyncQualitySettings(this.defaultVsyncQuality);
            if (!graphicsSettings.NeedCompactWindow())
            {
                graphicsSettings.ApplyInitialScreenResolutionData();
            }
            else
            {
                graphicsSettings.EnableCompactScreen(base.gameObject.AddComponent<CompactScreenBehaviour>());
            }
        }

        private void DefineScreenResolutionData(GraphicsSettings graphicsSettings)
        {
            List<Resolution> avaiableResolutions = this.FilterSmallResolutions();
            graphicsSettings.InitScreenResolutionSettings(avaiableResolutions, this.graphicsSettingsAnalyzer.GetDefaultResolution(avaiableResolutions));
        }

        private List<Resolution> FilterSmallResolutions()
        {
            List<Resolution> list = new List<Resolution>();
            Resolution[] resolutions = Screen.resolutions;
            int length = resolutions.Length;
            for (int i = 0; i < length; i++)
            {
                Resolution item = resolutions[i];
                if ((item.width >= this.minWidth) && (item.height >= this.minHeight))
                {
                    list.Add(item);
                }
            }
            if (list.Count == 0)
            {
                Resolution item = new Resolution {
                    height = this.minHeight,
                    width = this.minWidth
                };
                list.Add(item);
            }
            return list;
        }

        private bool UltraEnabled() => 
            bool.Parse(ConfigurationService.GetConfig(this.configPath).GetStringValue("ultraenabled"));

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

