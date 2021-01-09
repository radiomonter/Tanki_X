namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class GraphicsSettingsIndexesComponent : Component
    {
        private int fullScreenIndex;
        private int windowedIndex;

        public void CalculateScreenResolutionIndexes()
        {
            <CalculateScreenResolutionIndexes>c__AnonStorey0 storey = new <CalculateScreenResolutionIndexes>c__AnonStorey0();
            List<Resolution> screenResolutions = GraphicsSettings.INSTANCE.ScreenResolutions;
            storey.defaultResolution = GraphicsSettings.INSTANCE.DefaultResolution;
            storey.currentResolution = GraphicsSettings.INSTANCE.CurrentResolution;
            this.DefaultScreenResolutionIndex = screenResolutions.FindIndex(new Predicate<Resolution>(storey.<>m__0));
            this.CurrentScreenResolutionIndex = screenResolutions.FindIndex(new Predicate<Resolution>(storey.<>m__1));
        }

        public void InitWindowModeIndexes(int fullScreenIndex, int windowedIndex)
        {
            this.fullScreenIndex = fullScreenIndex;
            this.windowedIndex = windowedIndex;
            this.DefaultWindowModeIndex = !GraphicsSettings.INSTANCE.WindowedByDefault ? fullScreenIndex : windowedIndex;
        }

        public int DefaultWindowModeIndex { get; set; }

        public int CurrentWindowModeIndex =>
            !Screen.fullScreen ? this.windowedIndex : this.fullScreenIndex;

        public int CurrentSaturationLevelIndex { get; set; }

        public int DefaultSaturationLevelIndex { get; set; }

        public int CurrentVegetationQualityIndex { get; set; }

        public int DefaultVegetationQualityIndex { get; set; }

        public int CurrentGrassQualityIndex { get; set; }

        public int DefaultGrassQualityIndex { get; set; }

        public int CurrentRenderResolutionQualityIndex { get; set; }

        public int DefaultRenderResolutionQualityIndex { get; set; }

        public int CurrentAntialiasingQualityIndex { get; set; }

        public int DefaultAntialiasingQualityIndex { get; set; }

        public int CurrentCartridgeCaseAmountIndex { get; set; }

        public int DefaultCartridgeCaseAmountIndex { get; set; }

        public int CurrentVSyncQualityIndex { get; set; }

        public int DefaultVSyncQualityIndex { get; set; }

        public int CurrentAnisotropicQualityIndex { get; set; }

        public int DefaultAnisotropicQualityIndex { get; set; }

        public int CurrentTextureQualityIndex { get; set; }

        public int DefaultTextureQualityIndex { get; set; }

        public int CurrentShadowQualityIndex { get; set; }

        public int DefaultShadowQualityIndex { get; set; }

        public int CurrentParticleQualityIndex { get; set; }

        public int DefaultParticleQualityIndex { get; set; }

        public int CurrentScreenResolutionIndex { get; set; }

        public int DefaultScreenResolutionIndex { get; set; }

        [CompilerGenerated]
        private sealed class <CalculateScreenResolutionIndexes>c__AnonStorey0
        {
            internal Resolution defaultResolution;
            internal Resolution currentResolution;

            internal bool <>m__0(Resolution r) => 
                (r.width == this.defaultResolution.width) && (r.height == this.defaultResolution.height);

            internal bool <>m__1(Resolution r) => 
                (r.width == this.currentResolution.width) && (r.height == this.currentResolution.height);
        }
    }
}

