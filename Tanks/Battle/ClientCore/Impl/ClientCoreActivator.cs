namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore;
    using Tanks.Battle.ClientCore.API;
    using tanks.modules.battle.ClientCore.Scripts.Impl.Tank.Effect.Kamikadze;
    using UnityEngine;

    public class ClientCoreActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            PhysicsSetup.CheckCollisionMatrix();
            ServiceRegistry.Current.RegisterService<InputManager>(new InputManagerImpl());
        }

        private void AddBonusSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new BonusBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusClientConfigLoaderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusSpawnSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusFallingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusAlignmentToGroundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusTakingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusHidingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusRegionLogicalBuilderSystem());
        }

        private void AddMapSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new MapLoaderSystem());
        }

        private void AddMineSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new MineSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CommonMineSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MineActivateValidationSystem());
        }

        private void AddModuleSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new ModuleEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UnitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UnitMoveSystem());
            TemplateRegistry.Register(typeof(SpiderEffectTemplate));
            ECSBehaviour.EngineService.RegisterSystem(new SpiderMineSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpiderDirectionCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InventorySlotTemporaryBlockSystem());
            TemplateRegistry.Register(typeof(DroneEffectTemplate));
            TemplateRegistry.Register(typeof(DroneWeaponTemplate));
            ECSBehaviour.EngineService.RegisterSystem(new DroneEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DroneWeaponSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DroneMovementSystem());
            ECSBehaviour.EngineService.RegisterSystem(new JumpImpactSystem());
        }

        private void AddWeaponSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new SmokyBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlamethrowerBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RicochetBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TwinsBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ThunderBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MuzzlePointSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MuzzlePointSwitchSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CommonWeaponBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponGyroscopeRotationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponRotationInputSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponRotationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponCooldownSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponShotSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShotValidateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SplashHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FreezeBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerTargetSectorCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LocalHammerMagazineSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DiscreteWeaponEnergySystem());
            ECSBehaviour.EngineService.RegisterSystem(new DiscreteWeaponControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamWeaponControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamWeaponCooldownSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamWeaponEnergySystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamHitLoadCheckerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BulletTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VerticalTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VerticalSectorTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ConicTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TargetSectorCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VerticalDirectionsCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SectorDirectionsCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ConicDirectionsCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TargetCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerTargetCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DistanceAndAngleTargetEvaluatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TeamTargetEvaluatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFTargetEvaluatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DirectionEvaluatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DiscreteImpactSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ImpactWeakeningByTargetSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SplashImpactSystem());
            ECSBehaviour.EngineService.RegisterSystem(new KickbackSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponBulletShotSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BulletSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RicochetBulletSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TwinsBulletSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunChargingWeaponControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanWeaponControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanGyroscopeSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanImpactSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanKickbackSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FreezeTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlamethrowerTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisTargetEvaluatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftStateControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftEnergySystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingTargetPointSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftHitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingShotSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingStraightTargetingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingStraightDirectionCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftImpactSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingInputContextSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingVerticalTargetingControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingCooldownSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingTargetCollectorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftShotValidateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponEnergySystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShotToHitSystem());
        }

        private void RegisterEffects()
        {
            TemplateRegistry.Register<ArmorEffectTemplate>();
            TemplateRegistry.Register<DamageEffectTemplate>();
            TemplateRegistry.Register<TemperatureEffectTemplate>();
            TemplateRegistry.Register<HealingEffectTemplate>();
            TemplateRegistry.Register<ForceFieldEffectTemplate>();
            TemplateRegistry.Register<OverhaulEffectTemplate>();
            TemplateRegistry.Register<HolyshieldEffectTemplate>();
            TemplateRegistry.Register<InvulnerabilityEffectTemplate>();
            TemplateRegistry.Register<SonarEffectTemplate>();
            TemplateRegistry.Register<InvisibilityEffectTemplate>();
            TemplateRegistry.Register<LifestealEffectTemplate>();
            TemplateRegistry.Register<RageEffectTemplate>();
            TemplateRegistry.Register<TurboSpeedEffectTemplate>();
            TemplateRegistry.Register<BurningEffectTemplate>();
            TemplateRegistry.Register<MineEffectTemplate>();
            TemplateRegistry.Register<IcetrapEffectTemplate>();
            TemplateRegistry.Register<AcceleratedGearsEffectTemplate>();
            TemplateRegistry.Register<JumpEffectTemplate>();
            TemplateRegistry.Register<SapperEffectTemplate>();
            TemplateRegistry.Register<EmergencyProtectionHealingPartEffectTemplate>();
            TemplateRegistry.Register<EmergencyProtectionEffectTemplate>();
            TemplateRegistry.Register<EMPEffectTemplate>();
            TemplateRegistry.Register<BackhitDefenceEffectTemplate>();
            TemplateRegistry.Register<BackhitIncreaseEffectTemplate>();
            TemplateRegistry.Register<EnergyInjectionEffectTemplate>();
            TemplateRegistry.Register<NormalizeTemperatureEffectTemplate>();
            TemplateRegistry.Register<TargetFocusEffectTemplate>();
            TemplateRegistry.Register<EngineerEffectTemplate>();
            TemplateRegistry.Register<AdrenalineEffectTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new TargetFocusEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EMPEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnergyInjectionEffectSystem());
            TemplateRegistry.Register<ExternalImpactEffectTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new ExternalImpactEffectSystem());
            TemplateRegistry.Register<FireRingEffectTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new FireRingEffectSystem());
            TemplateRegistry.Register<ExplosiveMassEffectTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new ExplosiveMassEffectSystem());
            TemplateRegistry.Register<KamikadzeEffectTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new KamikadzeEffectSystem());
        }

        private void RegisterGraffitiTemplates()
        {
            TemplateRegistry.Register<GraffitiBattleItemTemplate>();
        }

        private void RegisterPaintTemplates()
        {
            TemplateRegistry.Register<TankPaintBattleItemTemplate>();
            TemplateRegistry.Register<WeaponPaintBattleItemTemplate>();
        }

        private void RegisterReticleTemplates()
        {
            TemplateRegistry.Register<ReticleTemplate>();
        }

        private void RegisterShellTemplates()
        {
            TemplateRegistry.Register<ShellBattleItemTemplate>();
        }

        private void RegisterSkinTemplates()
        {
            TemplateRegistry.Register<WeaponSkinBattleItemTemplate>();
            TemplateRegistry.Register<HullSkinBattleItemTemplate>();
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterTemplates();
            this.RegisterWeaponTemplates();
            this.RegisterReticleTemplates();
            this.RegisterPaintTemplates();
            this.RegisterGraffitiTemplates();
            this.RegisterShellTemplates();
            this.RegisterSkinTemplates();
            ECSBehaviour.EngineService.RegisterSystem(new MapPhysicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagCollisionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FlagDropSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelfMarkerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankPhysicsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankExplosionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ChassisSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankCollidersSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InputManagerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MouseControlSettingsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankJumpingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NanFixerSystem());
            if (!Application.isEditor)
            {
                ECSBehaviour.EngineService.RegisterSystem(new CursorStateSystem());
            }
            ECSBehaviour.EngineService.RegisterSystem(new PauseSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InstanceDestructionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankIncarnationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankAssemblySystem());
            ECSBehaviour.EngineService.RegisterSystem(new ClientHullBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PaintBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankFriendAndEnemySystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankCollisionDetectionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelfDestructionControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankUpsideDownSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IdleKickSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankEngineSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankFallingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HitFeedbackSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HealthSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TutorialHitTriggersSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InBattlesTutorialHandlersSystem());
            this.AddMapSystems();
            this.AddWeaponSystems();
            this.AddBonusSystems();
            this.AddMineSystems();
            this.AddModuleSystems();
            ECSBehaviour.EngineService.RegisterSystem(new LocalDurationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PerformanceStatisticsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleStatisticsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleLabelSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleLabelLoadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ServerShutdownNotificationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleShutdownNotificationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShutdownScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattlePingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new KeyboardSettingsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AdminUserSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TesterUserSystem());
            this.RegisterEffects();
            ServiceRegistry.Current.RegisterService<BattleFlowInstancesCache>(new BattleFlowInstancesCache());
            ECSBehaviour.EngineService.RegisterSystem(new ColorInBattleSystem());
        }

        private void RegisterTemplates()
        {
            TemplateRegistry.Register(typeof(BattleUserTemplate));
            TemplateRegistry.Register(typeof(RoundTemplate));
            TemplateRegistry.Register(typeof(RoundUserTemplate));
            TemplateRegistry.Register(typeof(TankTemplate));
            TemplateRegistry.Register(typeof(TankIncarnationTemplate));
            TemplateRegistry.Register(typeof(BonusTemplate));
            TemplateRegistry.Register(typeof(SupplyBonusTemplate));
            TemplateRegistry.Register(typeof(GoldBonusTemplate));
            TemplateRegistry.Register(typeof(GoldBonusWithCrystalsTemplate));
            TemplateRegistry.Register(typeof(GoldBonusWithTeleportTemplate));
            TemplateRegistry.Register(typeof(BonusRegionTemplate));
            TemplateRegistry.Register(typeof(BonusRegionAssetsTemplate));
            TemplateRegistry.Register(typeof(MapTemplate));
            TemplateRegistry.Register(typeof(PerformanceStatisticsTemplate));
            TemplateRegistry.Register(typeof(GeneralBattleChatTemplate));
            TemplateRegistry.Register(typeof(TeamBattleChatTemplate));
        }

        private void RegisterWeaponTemplates()
        {
            TemplateRegistry.Register(typeof(WeaponTemplate));
            TemplateRegistry.Register(typeof(StreamWeaponTemplate));
            TemplateRegistry.Register(typeof(DiscreteWeaponEnergyTemplate));
            TemplateRegistry.Register(typeof(SmokyBattleItemTemplate));
            TemplateRegistry.Register(typeof(ShaftBattleItemTemplate));
            TemplateRegistry.Register(typeof(FlamethrowerBattleItemTemplate));
            TemplateRegistry.Register(typeof(FreezeBattleItemTemplate));
            TemplateRegistry.Register(typeof(TwinsBattleItemTemplate));
            TemplateRegistry.Register(typeof(RicochetBattleItemTemplate));
            TemplateRegistry.Register(typeof(TwinsBulletTemplate));
            TemplateRegistry.Register(typeof(RicochetBulletTemplate));
            TemplateRegistry.Register(typeof(ThunderBattleItemTemplate));
            TemplateRegistry.Register(typeof(HammerBattleItemTemplate));
            TemplateRegistry.Register(typeof(RailgunBattleItemTemplate));
            TemplateRegistry.Register(typeof(VulcanBattleItemTemplate));
            TemplateRegistry.Register(typeof(IsisBattleItemTemplate));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

