namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ClientGraphicsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public static GUIStyle guiStyle;
        public static readonly string QUALITY_SETTINGS_ROOT_CONFIG_PATH = "clientlocal/graphics/settings/quality/";

        protected override void Activate()
        {
            this.CreateGUIStyle();
            this.CreateQualitySettingsEntity();
            this.AdjustTargetMaxTextureSize();
        }

        private void AdjustTargetMaxTextureSize()
        {
            if (SystemInfo.graphicsMemorySize <= 0x80)
            {
                QualitySettings.masterTextureLimit = 3;
            }
            else if (SystemInfo.graphicsMemorySize <= 0x100)
            {
                QualitySettings.masterTextureLimit = 2;
            }
            else if (SystemInfo.graphicsMemorySize <= 0x200)
            {
                QualitySettings.masterTextureLimit = 2;
            }
        }

        private Texture2D CreateColoredTexture(float r, float g, float b, float a)
        {
            Texture2D textured = new Texture2D(1, 1);
            Color color = new Color(r, g, b, a);
            textured.SetPixel(0, 0, color);
            textured.Apply();
            return textured;
        }

        private void CreateGUIStyle()
        {
            guiStyle = new GUIStyle();
            Texture2D textured = this.CreateColoredTexture(0f, 0f, 0f, 0.4f);
            guiStyle.normal.background = textured;
            guiStyle.normal.textColor = new Color(255f, 255f, 255f, 0.7f);
            guiStyle.alignment = TextAnchor.UpperLeft;
            guiStyle.fontSize = 30;
            guiStyle.padding = new RectOffset(20, 5, 5, 5);
            guiStyle.font = (Font) Resources.Load("SEGUISYM");
        }

        public void CreateQualitySettingsEntity()
        {
            string str = QualitySettings.names[QualitySettings.GetQualityLevel()].Replace(" ", string.Empty).ToLower();
            string configPath = QUALITY_SETTINGS_ROOT_CONFIG_PATH + str;
            ECSBehaviour.EngineService.Engine.CreateEntity<QualitySettingsTemplate>(configPath);
        }

        private void RegisterSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new MapEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NitroEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DoubleDamageEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShotAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AcceleratedGearsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ChassisAnimationInitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ChassisAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CommonDecalHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftDecalHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamingDecalHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerDecalHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GraffitiDecalSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TrackMarksSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TrackDustSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CartridgeCaseLifeSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagTransparencySystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagVisualDropSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagVisualPickupSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TransformTimeSmoothingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ClientTankSnapSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RemoteTankSmootherSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RemoteWeaponSmootherSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankVisualAssemblySystem());
            ECSBehaviour.EngineService.RegisterSystem(new FollowCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FreeCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleCameraBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ApplyCameraTransformSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CameraFOVUpdateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MouseOrbitCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TransitionCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EffectArrangementSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HullBuilderGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PaintGraphicsBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankCommonBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BrokenTankEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MuzzlePointGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DoubleArmorEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CommonWeaponVisualBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankRendererSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankActiveStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankDeadStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankSemiActiveStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TransparencyTransitionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MuzzleFlashSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunChargingEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunTrailSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BulletGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ModuleEffectGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MineCommonGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MineGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpiderMineVisualSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankExplosionGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HitExplosionGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CriticalDamageGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SoleTracerGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PelletThrowingGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisWorkingAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamWeaponParticleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponStreamMuzzleFlashSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponStreamTracerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanTurbineAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanBandAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponStreamHitGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RicochetBulletBounceGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankVisualTemperatureControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingMapEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingReticleEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingRendererSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RemoteShaftAimingLaserSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingColorEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingForceFieldEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftShotAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MapInitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PoolContainerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CameraInitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankPartIntersectionWithCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FreezeMotionAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlamethrowerMotionAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TwinsAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CartridgeCaseEjectionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerShotAnimationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MapHidingGeometryCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusGraphicsBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusBoxAppearingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusBoxDisappearingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusParachuteAppearingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusParachuteDisappearingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusRegionShowSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusRegionVisualBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoldRegionShowSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SupplyRegionShowSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SaveLoadCameraSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpectatorCameraManagerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UpdateUserRankEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UpdateUserRankSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankInvisibilityEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SonarEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankShaderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HealingGraphicsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CameraShakerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HealthFeedbackGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EmergencyProtectionGraphicsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EMPGraphicsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LifestealGraphicsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HolyshieldGraphicsEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ForceFieldEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ForceFieldSlotActivationValidatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DroneGraphicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CustomTankBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ParticleSystemEffectsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ModuleVisualEffectSystem());
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterTemplates();
            this.RegisterSystems();
        }

        private void RegisterTemplates()
        {
            TemplateRegistry.Register(typeof(CameraTemplate));
            TemplateRegistry.Register(typeof(QualitySettingsTemplate));
            TemplateRegistry.RegisterPart<DiscreteWeaponCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<ShaftCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<VulcanCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<SpiderMineCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<MineCameraShakerTemplatePart>();
            TemplateRegistry.RegisterPart<HammerHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<RailgunHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<RicochetHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<ShaftHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<SmokyHitFeedbackSoundsTemplatePart>();
            TemplateRegistry.RegisterPart<ThunderHitFeedbackSoundsTemplatePart>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

