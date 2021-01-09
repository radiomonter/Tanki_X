namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class GraphicsSettingsBuilderSystem : ECSSystem
    {
        private int currentGrassLevelIndex;
        private int currentShadowQualityIndex;
        private int currentParticleQualityIndex;
        private int currentAnisotropicQualityIndex;
        private int currentTextureQualityIndex;
        private int currentVegetationLevelIndex;
        private int currentAntialiasingQualityIndex;
        private int currentRenderResolutionQualityIndex;
        private int currentCartridgeCaseAmountIndex;
        private int currentVSyncQualityIndex;
        private int defaultGrassLevelIndex;
        private int defaultShadowQualityIndex;
        private int defaultParticleQualityIndex;
        private int defaultAnisotropicQualityIndex;
        private int defaultTextureQualityIndex;
        private int defaultVegetationLevelIndex;
        private int defaultAntialiasingQualityIndex;
        private int defaultRenderResolutionQualityIndex;
        private int defaultCartridgeCaseAmountIndex;
        private int defaultVSyncQualityIndex;
        [CompilerGenerated]
        private static Func<SingleNode<SaturationLevelVariantComponent>, float> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<SingleNode<VegetationSettingsComponent>, int> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<SingleNode<GrassSettingsComponent>, int> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<SingleNode<AnisotropicQualityVariantComponent>, int> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<SingleNode<RenderResolutionQualityVariantComponent>, int> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<SingleNode<AntialiasingQualityVariantComponent>, int> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<SingleNode<ShadowQualityVariantComponent>, int> <>f__am$cache6;
        [CompilerGenerated]
        private static Func<SingleNode<ParticleQualityVariantComponent>, int> <>f__am$cache7;
        [CompilerGenerated]
        private static Func<SingleNode<TextureQualityVariantComponent>, int> <>f__am$cache8;
        [CompilerGenerated]
        private static Func<SingleNode<CartridgeCaseSettingVariantComponent>, int> <>f__am$cache9;
        [CompilerGenerated]
        private static Func<SingleNode<VSyncSettingVariantComponent>, int> <>f__am$cacheA;

        [OnEventFire]
        public void BuildAnisotropicGraphicsSettings(NodeAddedEvent e, AnisotropicQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<AnisotropicQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildAnisotropicQualitySettings(GraphicsSettingsBuilderReadyEvent e, AnisotropicQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<AnisotropicQualityVariantComponent>> anisotropicQuality, AnisotropicQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildAnisotropicQualitySettings>c__AnonStorey3 storey = new <BuildAnisotropicQualitySettings>c__AnonStorey3();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentAnisotropicQuality = GraphicsSettings.INSTANCE.CurrentAnisotropicQuality;
            SingleNode<AnisotropicQualityVariantComponent> node = anisotropicQuality.OrderBy<SingleNode<AnisotropicQualityVariantComponent>, float>(new Func<SingleNode<AnisotropicQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<AnisotropicQualityVariantComponent>>();
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultAnisotropicQuality));
            }
            SingleNode<AnisotropicQualityVariantComponent> node2 = anisotropicQuality.OrderBy<SingleNode<AnisotropicQualityVariantComponent>, int>(<>f__am$cache3).First<SingleNode<AnisotropicQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyAnisotropicQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentAnisotropicQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultAnisotropicQualityIndex = items.IndexOf(node2.Entity);
            this.defaultAnisotropicQualityIndex = graphicsSettingsIndexes.component.DefaultAnisotropicQualityIndex;
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering) node.component.AnisotropicFiltering;
        }

        [OnEventFire]
        public void BuildAntialiasingGraphicsSettings(NodeAddedEvent e, AntialiasingQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<AntialiasingQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildAntialiasingQualitySettings(GraphicsSettingsBuilderReadyEvent e, AntialiasingQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<AntialiasingQualityVariantComponent>> antialiasingQuality, AntialiasingQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildAntialiasingQualitySettings>c__AnonStorey5 storey = new <BuildAntialiasingQualitySettings>c__AnonStorey5();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentAntialiasingQuality = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality;
            SingleNode<AntialiasingQualityVariantComponent> node = antialiasingQuality.OrderBy<SingleNode<AntialiasingQualityVariantComponent>, float>(new Func<SingleNode<AntialiasingQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<AntialiasingQualityVariantComponent>>();
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultAntialiasingQuality));
            }
            SingleNode<AntialiasingQualityVariantComponent> node2 = antialiasingQuality.OrderBy<SingleNode<AntialiasingQualityVariantComponent>, int>(<>f__am$cache5).First<SingleNode<AntialiasingQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyAntialiasingQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentAntialiasingQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultAntialiasingQualityIndex = items.IndexOf(node2.Entity);
            this.defaultAntialiasingQualityIndex = graphicsSettingsIndexes.component.DefaultAntialiasingQualityIndex;
        }

        [OnEventFire]
        public void BuildCartridgeCaseAmountSettings(NodeAddedEvent e, CartridgeCaseAmountSettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<CartridgeCaseSettingVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildCartridgeCaseAmountSettings(GraphicsSettingsBuilderReadyEvent e, CartridgeCaseAmountSettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<CartridgeCaseSettingVariantComponent>> cartridgeAmount, CartridgeCaseAmountSettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildCartridgeCaseAmountSettings>c__AnonStorey9 storey = new <BuildCartridgeCaseAmountSettings>c__AnonStorey9();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentCartridgeCaseAmount = GraphicsSettings.INSTANCE.CurrentCartridgeCaseAmount;
            SingleNode<CartridgeCaseSettingVariantComponent> node = cartridgeAmount.OrderBy<SingleNode<CartridgeCaseSettingVariantComponent>, int>(new Func<SingleNode<CartridgeCaseSettingVariantComponent>, int>(storey.<>m__0)).First<SingleNode<CartridgeCaseSettingVariantComponent>>();
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality));
            }
            SingleNode<CartridgeCaseSettingVariantComponent> node2 = cartridgeAmount.OrderBy<SingleNode<CartridgeCaseSettingVariantComponent>, int>(<>f__am$cache9).First<SingleNode<CartridgeCaseSettingVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultCartridgeCaseAmountIndex = items.IndexOf(node2.Entity);
            this.defaultCartridgeCaseAmountIndex = graphicsSettingsIndexes.component.DefaultCartridgeCaseAmountIndex;
        }

        public void BuildDefaultSettings()
        {
            if (QualitySettings.GetQualityLevel() == 0)
            {
                this.BuildSettings(ShadowQuality.Disable, ShadowResolution.Low, ShadowProjection.StableFit, 0f, 2f, 0, 1, AnisotropicFiltering.Disable, 0, 0, 0, 0, 0, 0, 0, true, false, false, 0, 0f, 0f, 15f, 0f, false, 1, 0, 0, 1);
            }
            if (QualitySettings.GetQualityLevel() == 1)
            {
                this.BuildSettings(ShadowQuality.Disable, ShadowResolution.Low, ShadowProjection.StableFit, 0f, 2f, 0, 1, AnisotropicFiltering.Disable, 0, 0, 0, 0, 0, 0, 0, true, false, false, 0, 0f, 0f, 15f, 0f, false, 0, 1, 0, 1);
            }
            if (QualitySettings.GetQualityLevel() == 2)
            {
                this.BuildSettings(ShadowQuality.All, ShadowResolution.Medium, ShadowProjection.StableFit, 70f, 2f, 0, 0, AnisotropicFiltering.Disable, 1, 1, 0, 1, 1, 0, 1, true, true, true, 1, 0f, 0f, 15f, 0f, false, 0, 2, 0, 1);
            }
            if (QualitySettings.GetQualityLevel() == 3)
            {
                this.BuildSettings(ShadowQuality.All, ShadowResolution.High, ShadowProjection.StableFit, 100f, 2f, 0, 0, AnisotropicFiltering.Enable, 2, 2, 1, 1, 2, 0, 2, true, true, true, 2, 80f, 65f, 15f, 0.4f, false, 0, 3, 1, 1);
            }
            if (QualitySettings.GetQualityLevel() == 4)
            {
                this.BuildSettings(ShadowQuality.All, ShadowResolution.High, ShadowProjection.StableFit, 100f, 2f, 2, 0, AnisotropicFiltering.ForceEnable, 3, 3, 2, 1, 3, 1, 3, true, true, true, 3, 100f, 65f, 15f, 0.65f, true, 0, 4, 2, 1);
            }
            if (QualitySettings.GetQualityLevel() == 5)
            {
                this.BuildSettings(ShadowQuality.All, ShadowResolution.VeryHigh, ShadowProjection.StableFit, 500f, 2f, 4, 0, AnisotropicFiltering.ForceEnable, 4, 4, 2, 1, 4, 1, 4, true, true, true, 4, 200f, 90f, 15f, 0.8f, true, 0, 5, 3, 1);
            }
        }

        private void BuildGraphicsSettings<T>(GraphicsSettingsBuilderNode builder) where T: Template
        {
            <BuildGraphicsSettings>c__AnonStoreyB<T> yb = new <BuildGraphicsSettings>c__AnonStoreyB<T> {
                builder = builder,
                $this = this
            };
            yb.group = yb.builder.Entity.CreateGroup<GraphicsSettingsBuilderGroupComponent>();
            yb.builder.graphicsSettingsBuilder.Items = new List<Entity>();
            yb.builder.configPathCollection.Collection.ForEach(new Action<string>(yb.<>m__0));
            base.ScheduleEvent<GraphicsSettingsBuilderReadyEvent>(yb.builder);
        }

        [OnEventFire]
        public void BuildGrassGraphicsSettings(NodeAddedEvent e, GrassSettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<GrassSettingsTemplate>(builder);
        }

        [OnEventFire]
        public void BuildGrassQualitySettings(GraphicsSettingsBuilderReadyEvent e, GrassSettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<GrassSettingsComponent>> grassSettings, GrassSettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildGrassQualitySettings>c__AnonStorey2 storey = new <BuildGrassQualitySettings>c__AnonStorey2();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentGrassLevel = GraphicsSettings.INSTANCE.CurrentGrassLevel;
            SingleNode<GrassSettingsComponent> node = grassSettings.OrderBy<SingleNode<GrassSettingsComponent>, float>(new Func<SingleNode<GrassSettingsComponent>, float>(storey.<>m__0)).First<SingleNode<GrassSettingsComponent>>();
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultGrassLevel));
            }
            SingleNode<GrassSettingsComponent> node2 = grassSettings.OrderBy<SingleNode<GrassSettingsComponent>, int>(<>f__am$cache2).First<SingleNode<GrassSettingsComponent>>();
            GraphicsSettings.INSTANCE.ApplyGrassLevel(node.component.Value, node2.component.Value);
            GraphicsSettings.INSTANCE.ApplyGrassFarDrawDistance(node.component.GrassFarDrawDistance, node2.component.GrassFarDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassNearDrawDistance(node.component.GrassNearDrawDistance, node2.component.GrassNearDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassFadeRange(node.component.GrassFadeRange, node2.component.GrassFadeRange);
            GraphicsSettings.INSTANCE.ApplyGrassDensityMultiplier(node.component.GrassDensityMultiplier, node2.component.GrassDensityMultiplier);
            GraphicsSettings.INSTANCE.ApplyGrassCastsShadow(node.component.GrassCastsShadow, node2.component.GrassCastsShadow);
            graphicsSettingsIndexes.component.CurrentGrassQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultGrassQualityIndex = items.IndexOf(node2.Entity);
            this.defaultGrassLevelIndex = graphicsSettingsIndexes.component.DefaultGrassQualityIndex;
        }

        [OnEventFire]
        public void BuildParticleGraphicsSettings(NodeAddedEvent e, ParticleQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<ParticleQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildParticleQualitySettings(GraphicsSettingsBuilderReadyEvent e, ParticleQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<ParticleQualityVariantComponent>> particleQuality, ParticleQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildParticleQualitySettings>c__AnonStorey7 storey = new <BuildParticleQualitySettings>c__AnonStorey7();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentParticleQuality = GraphicsSettings.INSTANCE.CurrentParticleQuality;
            SingleNode<ParticleQualityVariantComponent> node = particleQuality.OrderBy<SingleNode<ParticleQualityVariantComponent>, float>(new Func<SingleNode<ParticleQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<ParticleQualityVariantComponent>>();
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultParticleQuality));
            }
            SingleNode<ParticleQualityVariantComponent> node2 = particleQuality.OrderBy<SingleNode<ParticleQualityVariantComponent>, int>(<>f__am$cache7).First<SingleNode<ParticleQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyParticleQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentParticleQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultParticleQualityIndex = items.IndexOf(node2.Entity);
            this.defaultParticleQualityIndex = graphicsSettingsIndexes.component.DefaultParticleQualityIndex;
        }

        [OnEventFire]
        public void BuildRenderResolutionGraphicsSettings(NodeAddedEvent e, RenderResolutionQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<RenderResolutionQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildRenderResolutionQualitySettings(GraphicsSettingsBuilderReadyEvent e, RenderResolutionQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<RenderResolutionQualityVariantComponent>> renderResolutionQuality, RenderResolutionQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildRenderResolutionQualitySettings>c__AnonStorey4 storey = new <BuildRenderResolutionQualitySettings>c__AnonStorey4();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentRenderResulutionQuality = GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality;
            SingleNode<RenderResolutionQualityVariantComponent> node = renderResolutionQuality.OrderBy<SingleNode<RenderResolutionQualityVariantComponent>, float>(new Func<SingleNode<RenderResolutionQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<RenderResolutionQualityVariantComponent>>();
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality));
            }
            SingleNode<RenderResolutionQualityVariantComponent> node2 = renderResolutionQuality.OrderBy<SingleNode<RenderResolutionQualityVariantComponent>, int>(<>f__am$cache4).First<SingleNode<RenderResolutionQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyRenderResolutionQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentRenderResolutionQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultRenderResolutionQualityIndex = items.IndexOf(node2.Entity);
            this.defaultRenderResolutionQualityIndex = graphicsSettingsIndexes.component.DefaultRenderResolutionQualityIndex;
        }

        [OnEventFire]
        public void BuildSaturationGraphicsSettings(NodeAddedEvent e, SaturationLevelSettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<SaturationLevelVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildSaturationLevelSettings(GraphicsSettingsBuilderReadyEvent e, SaturationLevelSettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<SaturationLevelVariantComponent>> saturationLevels, SaturationLevelSettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildSaturationLevelSettings>c__AnonStorey0 storey = new <BuildSaturationLevelSettings>c__AnonStorey0();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentSaturationLevel = GraphicsSettings.INSTANCE.CurrentSaturationLevel;
            SingleNode<SaturationLevelVariantComponent> node = saturationLevels.OrderBy<SingleNode<SaturationLevelVariantComponent>, float>(new Func<SingleNode<SaturationLevelVariantComponent>, float>(storey.<>m__0)).First<SingleNode<SaturationLevelVariantComponent>>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = i => Mathf.Abs((float) (i.component.Value - GraphicsSettings.INSTANCE.DefaultSaturationLevel));
            }
            SingleNode<SaturationLevelVariantComponent> node2 = saturationLevels.OrderBy<SingleNode<SaturationLevelVariantComponent>, float>(<>f__am$cache0).First<SingleNode<SaturationLevelVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplySaturationLevel(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultSaturationLevelIndex = items.IndexOf(node2.Entity);
        }

        [OnEventFire]
        public void BuildScreenResolutionSettings(NodeAddedEvent evt, SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            graphicsSettingsIndexes.component.CalculateScreenResolutionIndexes();
        }

        public void BuildSettings(ShadowQuality shadowQuality, ShadowResolution shadowResolution, ShadowProjection shadowProjection, float shadowDistance, float shadowNearPlane, int shadowCascades, int masterTexture, AnisotropicFiltering anisotropicFiltering, int grassLevelIndex, int shadowQualityIndex, int anisotropicQualityIndex, int textureQualityIndex, int vegetationLevelIndex, int antialiasingQuality, int vegetationLevel, bool farFoliageEnabled, bool treesShadowRecieving, bool billboardTreesShadowCasting, int grassLevel, float grassFarDrawDistance, float grassNearDrawDistance, float grassFadeRange, float grassDensityMultiplier, bool grassCastsShadow, int renderResolutionQuality, int particleQuality, int cartridgeCaseAmount, int vsyncQuality)
        {
            QualitySettings.shadows = shadowQuality;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.shadowProjection = shadowProjection;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowNearPlaneOffset = shadowNearPlane;
            QualitySettings.shadowCascades = shadowCascades;
            QualitySettings.masterTextureLimit = masterTexture;
            QualitySettings.anisotropicFiltering = anisotropicFiltering;
            QualitySettings.vSyncCount = vsyncQuality;
            this.currentGrassLevelIndex = grassLevelIndex;
            this.currentShadowQualityIndex = shadowQualityIndex;
            this.currentAnisotropicQualityIndex = anisotropicQualityIndex;
            this.currentTextureQualityIndex = textureQualityIndex;
            this.currentVegetationLevelIndex = vegetationLevelIndex;
            this.currentAntialiasingQualityIndex = antialiasingQuality;
            this.currentRenderResolutionQualityIndex = renderResolutionQuality;
            this.currentCartridgeCaseAmountIndex = cartridgeCaseAmount;
            this.currentVSyncQualityIndex = vsyncQuality;
            this.currentParticleQualityIndex = particleQuality;
            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(cartridgeCaseAmount, cartridgeCaseAmount);
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(vsyncQuality, vsyncQuality);
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(vegetationLevel, vegetationLevel);
            GraphicsSettings.INSTANCE.ApplyFarFoliageEnabled(farFoliageEnabled, farFoliageEnabled);
            GraphicsSettings.INSTANCE.ApplyTreesShadowRecieving(treesShadowRecieving, treesShadowRecieving);
            GraphicsSettings.INSTANCE.ApplyBillboardTreesShadowCasting(billboardTreesShadowCasting, billboardTreesShadowCasting);
            GraphicsSettings.INSTANCE.ApplyGrassLevel(grassLevel, grassLevel);
            GraphicsSettings.INSTANCE.ApplyGrassFarDrawDistance(grassFarDrawDistance, grassFarDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassNearDrawDistance(grassNearDrawDistance, grassNearDrawDistance);
            GraphicsSettings.INSTANCE.ApplyGrassFadeRange(grassFadeRange, grassFadeRange);
            GraphicsSettings.INSTANCE.ApplyGrassDensityMultiplier(grassDensityMultiplier, grassDensityMultiplier);
            GraphicsSettings.INSTANCE.ApplyGrassCastsShadow(grassCastsShadow, grassCastsShadow);
        }

        [OnEventFire]
        public void BuildShadowGraphicsSettings(NodeAddedEvent e, ShadowQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<ShadowQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildShadowQualitySettings(GraphicsSettingsBuilderReadyEvent e, ShadowQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<ShadowQualityVariantComponent>> shadowQuality, ShadowQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildShadowQualitySettings>c__AnonStorey6 storey = new <BuildShadowQualitySettings>c__AnonStorey6();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentShadowQuality = GraphicsSettings.INSTANCE.CurrentShadowQuality;
            SingleNode<ShadowQualityVariantComponent> node = shadowQuality.OrderBy<SingleNode<ShadowQualityVariantComponent>, float>(new Func<SingleNode<ShadowQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<ShadowQualityVariantComponent>>();
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultShadowQuality));
            }
            SingleNode<ShadowQualityVariantComponent> node2 = shadowQuality.OrderBy<SingleNode<ShadowQualityVariantComponent>, int>(<>f__am$cache6).First<SingleNode<ShadowQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyShadowQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentShadowQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultShadowQualityIndex = items.IndexOf(node2.Entity);
            this.defaultShadowQualityIndex = graphicsSettingsIndexes.component.DefaultShadowQualityIndex;
            ShadowResolution shadowResolution = (ShadowResolution) node.component.ShadowResolution;
            ShadowProjection shadowProjection = (ShadowProjection) node.component.ShadowProjection;
            QualitySettings.shadows = (ShadowQuality) node.component.ShadowQuality;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.shadowProjection = shadowProjection;
            QualitySettings.shadowDistance = node.component.ShadowDistance;
            QualitySettings.shadowNearPlaneOffset = node.component.ShadowNearPlaneOffset;
            QualitySettings.shadowCascades = node.component.ShadowCascades;
        }

        [OnEventFire]
        public void BuildTextureGraphicsSettings(NodeAddedEvent e, TextureQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<TextureQualityVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildTextureQualitySettings(GraphicsSettingsBuilderReadyEvent e, TextureQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<TextureQualityVariantComponent>> textureQuality, TextureQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildTextureQualitySettings>c__AnonStorey8 storey = new <BuildTextureQualitySettings>c__AnonStorey8();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentTextureQuality = GraphicsSettings.INSTANCE.CurrentTextureQuality;
            SingleNode<TextureQualityVariantComponent> node = textureQuality.OrderBy<SingleNode<TextureQualityVariantComponent>, float>(new Func<SingleNode<TextureQualityVariantComponent>, float>(storey.<>m__0)).First<SingleNode<TextureQualityVariantComponent>>();
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultTextureQuality));
            }
            SingleNode<TextureQualityVariantComponent> node2 = textureQuality.OrderBy<SingleNode<TextureQualityVariantComponent>, int>(<>f__am$cache8).First<SingleNode<TextureQualityVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyTextureQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentTextureQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultTextureQualityIndex = items.IndexOf(node2.Entity);
            this.defaultTextureQualityIndex = graphicsSettingsIndexes.component.DefaultTextureQualityIndex;
            QualitySettings.masterTextureLimit = node.component.MasterTextureLimit;
        }

        [OnEventFire]
        public void BuildVegetationGraphicsSettings(NodeAddedEvent e, VegetationSettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<VegetationSettingsTemplate>(builder);
        }

        [OnEventFire]
        public void BuildVegetationQualitySettings(GraphicsSettingsBuilderReadyEvent e, VegetationSettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<VegetationSettingsComponent>> vegetationSettings, VegetationSettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildVegetationQualitySettings>c__AnonStorey1 storey = new <BuildVegetationQualitySettings>c__AnonStorey1();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            storey.currentVegetationLevel = GraphicsSettings.INSTANCE.CurrentVegetationLevel;
            SingleNode<VegetationSettingsComponent> node = vegetationSettings.OrderBy<SingleNode<VegetationSettingsComponent>, float>(new Func<SingleNode<VegetationSettingsComponent>, float>(storey.<>m__0)).First<SingleNode<VegetationSettingsComponent>>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultVegetationLevel));
            }
            SingleNode<VegetationSettingsComponent> node2 = vegetationSettings.OrderBy<SingleNode<VegetationSettingsComponent>, int>(<>f__am$cache1).First<SingleNode<VegetationSettingsComponent>>();
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(node.component.Value, node2.component.Value);
            GraphicsSettings.INSTANCE.ApplyFarFoliageEnabled(node.component.FarFoliageEnabled, node2.component.FarFoliageEnabled);
            GraphicsSettings.INSTANCE.ApplyTreesShadowRecieving(node.component.BillboardTreesShadowReceiving, node2.component.BillboardTreesShadowReceiving);
            GraphicsSettings.INSTANCE.ApplyBillboardTreesShadowCasting(node.component.BillboardTreesShadowCasting, node2.component.BillboardTreesShadowCasting);
            graphicsSettingsIndexes.component.CurrentVegetationQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultVegetationQualityIndex = items.IndexOf(node2.Entity);
            this.defaultVegetationLevelIndex = graphicsSettingsIndexes.component.DefaultVegetationQualityIndex;
        }

        [OnEventFire]
        public void BuildVSyncQualitySettings(NodeAddedEvent e, VSyncQualitySettingsBuilderNode builder)
        {
            this.BuildGraphicsSettings<VSyncSettingVariantTemplate>(builder);
        }

        [OnEventFire]
        public void BuildVSyncQualitySettings(GraphicsSettingsBuilderReadyEvent e, VSyncQualitySettingsBuilderNode builder, [JoinByGraphicsSettingsBuilder] ICollection<SingleNode<VSyncSettingVariantComponent>> vSync, VSyncQualitySettingsBuilderNode builder1, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <BuildVSyncQualitySettings>c__AnonStoreyA ya = new <BuildVSyncQualitySettings>c__AnonStoreyA();
            List<Entity> items = builder.graphicsSettingsBuilder.Items;
            ya.currentVSyncQuality = GraphicsSettings.INSTANCE.CurrentVSyncQuality;
            SingleNode<VSyncSettingVariantComponent> node = vSync.OrderBy<SingleNode<VSyncSettingVariantComponent>, int>(new Func<SingleNode<VSyncSettingVariantComponent>, int>(ya.<>m__0)).First<SingleNode<VSyncSettingVariantComponent>>();
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = i => Mathf.Abs((int) (i.component.Value - GraphicsSettings.INSTANCE.DefaultRenderResolutionQuality));
            }
            SingleNode<VSyncSettingVariantComponent> node2 = vSync.OrderBy<SingleNode<VSyncSettingVariantComponent>, int>(<>f__am$cacheA).First<SingleNode<VSyncSettingVariantComponent>>();
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(node.component.Value, node2.component.Value);
            graphicsSettingsIndexes.component.CurrentVSyncQualityIndex = items.IndexOf(node.Entity);
            graphicsSettingsIndexes.component.DefaultVSyncQualityIndex = items.IndexOf(node2.Entity);
            this.defaultVSyncQualityIndex = graphicsSettingsIndexes.component.DefaultVSyncQualityIndex;
            QualitySettings.vSyncCount = node.component.Value;
        }

        [OnEventComplete]
        public void CleanBuilder(GraphicsSettingsBuilderReadyEvent e, GraphicsSettingsBuilderNode builder)
        {
            builder.graphicsSettingsBuilder.Items.ForEach(item => base.DeleteEntity(item));
            base.DeleteEntity(builder.Entity);
        }

        private Entity CreateWindowModeItem(string localizedCaption, bool windowed, CarouselGroupComponent carouselGroup)
        {
            Entity entity = base.CreateEntity("WindowModeItem");
            WindowModeVariantComponent component = new WindowModeVariantComponent {
                Windowed = windowed
            };
            entity.AddComponent(component);
            CarouselItemTextComponent component4 = new CarouselItemTextComponent {
                LocalizedCaption = localizedCaption
            };
            entity.AddComponent(component4);
            carouselGroup.Attach(entity);
            return entity;
        }

        [OnEventFire]
        public void InitQualitySettingsCarouselItems(NodeAddedEvent e, QualitySettingCarouselNode qualitySettingCarousel)
        {
            CarouselGroupComponent carouselGroup = qualitySettingCarousel.carouselGroup;
            long itemTemplateId = qualitySettingCarousel.carousel.ItemTemplateId;
            string baseConfigPath = qualitySettingCarousel.qualitySettingsCarousel.baseConfigPath;
            List<Entity> list = new List<Entity>();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                if ((i != Quality.ultra.Level) || GraphicsSettings.INSTANCE.UltraEnabled)
                {
                    Entity entity = base.CreateEntity(itemTemplateId, baseConfigPath + QualitySettings.names[i].ToLower());
                    carouselGroup.Attach(entity);
                    list.Add(entity);
                }
            }
            CarouselItemCollectionComponent component = new CarouselItemCollectionComponent {
                Items = list
            };
            qualitySettingCarousel.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void InitWindowModeSettingsCarouselItems(NodeAddedEvent e, WindowModeSettingCarouselNode windowModeSettingCarousel, SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            List<Entity> list2 = new List<Entity> {
                this.CreateWindowModeItem(windowModeSettingCarousel.windowModes.Fullscreen, false, windowModeSettingCarousel.carouselGroup),
                this.CreateWindowModeItem(windowModeSettingCarousel.windowModes.Windowed, true, windowModeSettingCarousel.carouselGroup)
            };
            graphicsSettingsIndexes.component.InitWindowModeIndexes(0, 1);
            CarouselItemCollectionComponent component = new CarouselItemCollectionComponent {
                Items = list2
            };
            windowModeSettingCarousel.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SetPostProcessing(NodeAddedEvent e, SingleNode<PostProcessingQualityVariantComponent> settings, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.customSettings = settings.component.CustomSettings;
            GraphicsSettings.INSTANCE.currentAmbientOcclusion = settings.component.AmbientOcclusion;
            GraphicsSettings.INSTANCE.currentBloom = settings.component.Bloom;
            GraphicsSettings.INSTANCE.currentChromaticAberration = settings.component.ChromaticAberration;
            GraphicsSettings.INSTANCE.currentGrain = settings.component.Grain;
            GraphicsSettings.INSTANCE.currentVignette = settings.component.Vignette;
            if (!GraphicsSettings.INSTANCE.customSettings)
            {
                GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
                this.BuildDefaultSettings();
                component.CurrentAnisotropicQualityIndex = this.currentAnisotropicQualityIndex;
                component.DefaultAnisotropicQualityIndex = this.defaultAnisotropicQualityIndex;
                component.CurrentAntialiasingQualityIndex = this.currentAntialiasingQualityIndex;
                component.DefaultAntialiasingQualityIndex = this.defaultAntialiasingQualityIndex;
                component.CurrentRenderResolutionQualityIndex = this.currentRenderResolutionQualityIndex;
                component.DefaultRenderResolutionQualityIndex = this.defaultRenderResolutionQualityIndex;
                component.CurrentGrassQualityIndex = this.currentGrassLevelIndex;
                component.DefaultGrassQualityIndex = this.defaultGrassLevelIndex;
                component.CurrentShadowQualityIndex = this.currentShadowQualityIndex;
                component.DefaultShadowQualityIndex = this.defaultShadowQualityIndex;
                component.CurrentParticleQualityIndex = this.currentParticleQualityIndex;
                component.DefaultParticleQualityIndex = this.defaultParticleQualityIndex;
                component.CurrentTextureQualityIndex = this.currentTextureQualityIndex;
                component.DefaultTextureQualityIndex = this.defaultTextureQualityIndex;
                component.CurrentVegetationQualityIndex = this.currentVegetationLevelIndex;
                component.DefaultVegetationQualityIndex = this.defaultVegetationLevelIndex;
                component.CurrentCartridgeCaseAmountIndex = this.currentCartridgeCaseAmountIndex;
                component.DefaultCartridgeCaseAmountIndex = this.defaultCartridgeCaseAmountIndex;
                component.CurrentVSyncQualityIndex = this.currentVSyncQualityIndex;
                component.DefaultVSyncQualityIndex = this.defaultVSyncQualityIndex;
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [CompilerGenerated]
        private sealed class <BuildAnisotropicQualitySettings>c__AnonStorey3
        {
            internal float currentAnisotropicQuality;

            internal float <>m__0(SingleNode<AnisotropicQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentAnisotropicQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildAntialiasingQualitySettings>c__AnonStorey5
        {
            internal float currentAntialiasingQuality;

            internal float <>m__0(SingleNode<AntialiasingQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentAntialiasingQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildCartridgeCaseAmountSettings>c__AnonStorey9
        {
            internal int currentCartridgeCaseAmount;

            internal int <>m__0(SingleNode<CartridgeCaseSettingVariantComponent> i) => 
                Mathf.Abs((int) (i.component.Value - this.currentCartridgeCaseAmount));
        }

        [CompilerGenerated]
        private sealed class <BuildGraphicsSettings>c__AnonStoreyB<T> where T: Template
        {
            internal GraphicsSettingsBuilderGroupComponent group;
            internal GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode builder;
            internal GraphicsSettingsBuilderSystem $this;

            internal void <>m__0(string s)
            {
                Entity entity = this.$this.CreateEntity<T>(s);
                this.group.Attach(entity);
                this.builder.graphicsSettingsBuilder.Items.Add(entity);
            }
        }

        [CompilerGenerated]
        private sealed class <BuildGrassQualitySettings>c__AnonStorey2
        {
            internal float currentGrassLevel;

            internal float <>m__0(SingleNode<GrassSettingsComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentGrassLevel));
        }

        [CompilerGenerated]
        private sealed class <BuildParticleQualitySettings>c__AnonStorey7
        {
            internal float currentParticleQuality;

            internal float <>m__0(SingleNode<ParticleQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentParticleQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildRenderResolutionQualitySettings>c__AnonStorey4
        {
            internal float currentRenderResulutionQuality;

            internal float <>m__0(SingleNode<RenderResolutionQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentRenderResulutionQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildSaturationLevelSettings>c__AnonStorey0
        {
            internal float currentSaturationLevel;

            internal float <>m__0(SingleNode<SaturationLevelVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentSaturationLevel));
        }

        [CompilerGenerated]
        private sealed class <BuildShadowQualitySettings>c__AnonStorey6
        {
            internal float currentShadowQuality;

            internal float <>m__0(SingleNode<ShadowQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentShadowQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildTextureQualitySettings>c__AnonStorey8
        {
            internal float currentTextureQuality;

            internal float <>m__0(SingleNode<TextureQualityVariantComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentTextureQuality));
        }

        [CompilerGenerated]
        private sealed class <BuildVegetationQualitySettings>c__AnonStorey1
        {
            internal float currentVegetationLevel;

            internal float <>m__0(SingleNode<VegetationSettingsComponent> i) => 
                Mathf.Abs((float) (i.component.Value - this.currentVegetationLevel));
        }

        [CompilerGenerated]
        private sealed class <BuildVSyncQualitySettings>c__AnonStoreyA
        {
            internal int currentVSyncQuality;

            internal int <>m__0(SingleNode<VSyncSettingVariantComponent> i) => 
                Mathf.Abs((int) (i.component.Value - this.currentVSyncQuality));
        }

        public class AnisotropicQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public AnisotropicQualitySettingsBuilderComponent anisotropicQualitySettingsBuilder;
        }

        public class AntialiasingQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public AntialiasingQualitySettingsBuilderComponent antialiasingQualitySettingsBuilder;
        }

        public class CartridgeCaseAmountSettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public CartridgeCaseAmountSettingBuilderComponent cartridgeCaseAmountSettingBuilder;
        }

        public class GraphicsSettingsBuilderNode : Node
        {
            public GraphicsSettingsBuilderComponent graphicsSettingsBuilder;
            public ConfigPathCollectionComponent configPathCollection;
        }

        public class GrassSettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public GrassSettingsBuilderComponent grassSettingsBuilder;
        }

        public class ParticleQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public ParticleQualitySettingsBuilderComponent particleQualitySettingsBuilder;
        }

        public class QualitySettingCarouselNode : Node
        {
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
            public CarouselComponent carousel;
            public CarouselGroupComponent carouselGroup;
        }

        public class RenderResolutionQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public RenderResolutionQualitySettingsBuilderComponent renderResolutionQualitySettingsBuilder;
        }

        public class SaturationLevelSettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public SaturationLevelSettingsBuilderComponent saturationLevelSettingsBuilder;
        }

        public class ShadowQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public ShadowQualitySettingsBuilderComponent shadowQualitySettingsBuilder;
        }

        public class TextureQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public TextureQualitySettingsBuilderComponent textureQualitySettingsBuilder;
        }

        public class VegetationSettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public VegetationSettingsBuilderComponent vegetationSettingsBuilder;
        }

        public class VSyncQualitySettingsBuilderNode : GraphicsSettingsBuilderSystem.GraphicsSettingsBuilderNode
        {
            public VSyncSettingBuilderComponent vSyncSettingBuilder;
        }

        public class WindowModeSettingCarouselNode : Node
        {
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
            public WindowModesComponent windowModes;
            public CarouselComponent carousel;
            public CarouselGroupComponent carouselGroup;
        }
    }
}

