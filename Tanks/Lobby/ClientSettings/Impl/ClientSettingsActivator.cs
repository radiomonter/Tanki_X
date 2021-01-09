namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ClientSettingsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        [SerializeField]
        private string saturationLevelTemplatePath;
        [SerializeField]
        private string antialiasingQualityTemplatePath;
        [SerializeField]
        private string renderResolutionQualityTemplatePath;
        [SerializeField]
        private string shadowQualityTemplatePath;
        [SerializeField]
        private string particleQualityTemplatePath;
        [SerializeField]
        private string textureQualityTemplatePath;
        [SerializeField]
        private string anisotropicQualityTemplatePath;
        [SerializeField]
        private string vegetationSettingsTemplatePath;
        [SerializeField]
        private string grassSettingsTemplatePath;
        [SerializeField]
        private string cartridgeCaseAmountTemplatePath;
        [SerializeField]
        private string vsyncTemplatePath;

        protected override void Activate()
        {
            Engine engine = ECSBehaviour.EngineService.Engine;
            this.BuildGraphicsSettings(engine);
            engine.CreateEntity<SettingsTemplate>(string.Empty);
        }

        private void BuildGraphicsSettings(Engine engine)
        {
            engine.CreateEntity("GraphicsSettingsIndexes").AddComponent<GraphicsSettingsIndexesComponent>();
            engine.CreateEntity<SaturationLevelSettingsBuilderTemplate>(this.saturationLevelTemplatePath);
            engine.CreateEntity<AnisotropicQualitySettingsBuilderTemplate>(this.anisotropicQualityTemplatePath);
            engine.CreateEntity<AntialiasingQualitySettingsBuilderTemplate>(this.antialiasingQualityTemplatePath);
            engine.CreateEntity<RenderResolutionQualitySettingsBuilderTemplate>(this.renderResolutionQualityTemplatePath);
            engine.CreateEntity<TextureQualitySettingsBuilderTemplate>(this.textureQualityTemplatePath);
            engine.CreateEntity<ShadowQualitySettingsBuilderTemplate>(this.shadowQualityTemplatePath);
            engine.CreateEntity<ParticleQualitySettingsBuilderTemplate>(this.particleQualityTemplatePath);
            engine.CreateEntity<VegetationSettingsBuilderTemplate>(this.vegetationSettingsTemplatePath);
            engine.CreateEntity<GrassSettingsBuilderTemplate>(this.grassSettingsTemplatePath);
            engine.CreateEntity<CartridgeCaseAmountSettingBuilderTemplate>(this.cartridgeCaseAmountTemplatePath);
            engine.CreateEntity<VSyncSettingBuilderTemplate>(this.vsyncTemplatePath);
        }

        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new SelectLocaleScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScreenResolutionSettingsCarouselBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GraphicsSettingsScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GraphicsSettingsBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PostProcessingQualitySystem());
            TemplateRegistry.Register<SettingsTemplate>();
            TemplateRegistry.Register<QualitySettingsVariantTemplate>();
            TemplateRegistry.Register<ScreenResolutionVariantTemplate>();
            TemplateRegistry.Register<WindowModesTemplate>();
            TemplateRegistry.Register<SaturationLevelVariantTemplate>();
            TemplateRegistry.Register<AnisotropicQualityVariantTemplate>();
            TemplateRegistry.Register<AntialiasingQualityVariantTemplate>();
            TemplateRegistry.Register<RenderResolutionQualityVariantTemplate>();
            TemplateRegistry.Register<ShadowQualityVariantTemplate>();
            TemplateRegistry.Register<ParticleQualityVariantTemplate>();
            TemplateRegistry.Register<TextureQualityVariantTemplate>();
            TemplateRegistry.Register<GraphicsSettingsBuilderTemplate>();
            TemplateRegistry.Register<SaturationLevelSettingsBuilderTemplate>();
            TemplateRegistry.Register<AnisotropicQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<AntialiasingQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<RenderResolutionQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<CartridgeCaseSettingVariantTemplate>();
            TemplateRegistry.Register<CartridgeCaseAmountSettingBuilderTemplate>();
            TemplateRegistry.Register<VSyncSettingVariantTemplate>();
            TemplateRegistry.Register<VSyncSettingBuilderTemplate>();
            TemplateRegistry.Register<ShadowQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<ParticleQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<TextureQualitySettingsBuilderTemplate>();
            TemplateRegistry.Register<GrassSettingsTemplate>();
            TemplateRegistry.Register<VegetationSettingsTemplate>();
            TemplateRegistry.Register<VegetationSettingsBuilderTemplate>();
            TemplateRegistry.Register<GrassSettingsBuilderTemplate>();
            TemplateRegistry.RegisterPart<CartridgeCaseSettingTemplatePart>();
            TemplateRegistry.RegisterPart<VSyncSettingTemplatePart>();
            TemplateRegistry.RegisterPart<PostProcessingQualityVariantTemplatePart>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

