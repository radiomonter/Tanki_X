namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientHUDActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        private void RegisterSystems()
        {
            ECSBehaviour.EngineService.RegisterSystem(new HUDScoreTableSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TeamBattleScoreTableSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HUDBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NameplateBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NameplatePositioningSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NameplateOpacitySystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemBundleLimitBundleEffectsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemButtonAmmunitionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemButtonCooldownStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemButtonEnabledStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemButtonPassiveStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InventorySlotActivationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TabSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RoundFinishSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UIElementsVisibilityControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CombatEventLogSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DMCombatEventLogSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TDMCombatEventLogSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFCombatEventLogSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VisualScoreSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ServiceMessageSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ServiceMessagesInteroperabilitySystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelfDestructionHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PauseAndIdleKickHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UpsideDownHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpectatorHUDModesSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RoundSpecificVisibilityControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleChatSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleHUDStateControllerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleInputContextSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSpectatorInputContextSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleChatChannelSwitchSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ReceiveBattleMessageSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ResizeBattleChatScrollViewSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleChatVisibilitySystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleChatInputSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserNotificationHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MainHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InventoryHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HealthBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EffectsHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EffectHUDCooldownSpeedSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LockSlotByEMPSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnergyInjectionEffectHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RageHUDEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DMScoreHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TeamScoreHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFFlagsHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFPointersSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WarmingUpTimerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WarmingUpTimerNotificationsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DiscreteEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TwinsEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanEnergyBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MultikillSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DisbalanceInfoSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TutorialHUDSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DamageInfoSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SelfTargetHitFeedbackHUDSystem());
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterSystems();
            TemplateRegistry.Register<BattleChatHUDTemplate>();
            TemplateRegistry.Register<CombatLogMessagesTemplate>();
            TemplateRegistry.Register<HUDWorldSpaceCanvasTemplate>();
            TemplateRegistry.Register<DMBattleScreenTemplate>();
            TemplateRegistry.Register<PauseServiceMessageTemplate>();
            TemplateRegistry.Register<SelfDestructionServiceMessageTemplate>();
            TemplateRegistry.Register<UpsideDownServiceMessageTemplate>();
            TemplateRegistry.Register<AutokickServiceMessageTemplate>();
            TemplateRegistry.Register<UserNotificatorHUDTemplate>();
            TemplateRegistry.Register<UserNotificatorRankNamesTemplate>();
            TemplateRegistry.RegisterPart<EffectHUDTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

