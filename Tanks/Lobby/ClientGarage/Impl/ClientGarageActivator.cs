namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientGarage.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl.Tutorial;

    public class ClientGarageActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            Engine engine = EngineService.Engine;
            UpgradablePropertiesUtils.MAX_LEVEL = engine.CreateEntity<GarageTemplate>("garage").GetComponent<ItemUpgradeExperiencesConfigComponent>().LevelsExperiences.Length + 1;
            engine.CreateEntity<SlotConfigTemplate>("garage/module/slot");
            engine.CreateEntity<ModuleTierConfigTemplate>("garage/module/tier");
            engine.CreateEntity<TutorialsConfigurationTemplate>("garage/tutorial/config");
        }

        private void RegisterAvatarSystems()
        {
            EngineService.RegisterSystem(new AvatarMenuSystem());
            EngineService.RegisterSystem(new AvatarScreenSystem());
        }

        private void RegisterAvatarTemplates()
        {
            TemplateRegistry.Register<AvatarMarketItemTemplate>();
            TemplateRegistry.Register<AvatarUserItemTemplate>();
        }

        private void RegisterButtonWithPriceTemplates()
        {
            TemplateRegistry.Register<BuyButtonWithPriceTemplate>();
        }

        private void RegisterContainerTemplates()
        {
            TemplateRegistry.Register<ContainerMarketItemTemplate>();
            TemplateRegistry.Register<ContainerPackPriceMarketItemTemplate>();
            TemplateRegistry.Register<ContainerUserItemTemplate>();
            TemplateRegistry.Register<GameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<DonutChestMarketItemTemplate>();
            TemplateRegistry.Register<GameplayChestUserItemTemplate>();
            TemplateRegistry.Register<SimpleChestUserItemTemplate>();
            TemplateRegistry.Register<CheatGameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<CheatGameplayChestUserItemTemplate>();
            TemplateRegistry.Register<TutorialGameplayChestMarketItemTemplate>();
            TemplateRegistry.Register<TutorialGameplayChestUserItemTemplate>();
        }

        private void RegisterCrystalTemplates()
        {
            TemplateRegistry.Register<CrystalMarketItemTemplate>();
            TemplateRegistry.Register<CrystalUserItemTemplate>();
            TemplateRegistry.Register<XCrystalMarketItemTemplate>();
            TemplateRegistry.Register<XCrystalUserItemTemplate>();
        }

        private void RegisterDetailTemplates()
        {
            TemplateRegistry.Register<DetailItemTemplate>();
            TemplateRegistry.Register<DetailMarketItemTemplate>();
            TemplateRegistry.Register<DetailUserItemTemplate>();
        }

        private void RegisterEnergyTemplates()
        {
            TemplateRegistry.Register<EnergyMarketItemTemplate>();
            TemplateRegistry.Register<EnergyUserItemTemplate>();
            TemplateRegistry.Register<QuantumMarketItemTemplate>();
        }

        private void RegisterGoldBonusTemplates()
        {
            TemplateRegistry.Register<GoldBonusMarketItemTemplate>();
            TemplateRegistry.Register<GoldBonusUserItemTemplate>();
        }

        private void RegisterGraffitiTemplates()
        {
            TemplateRegistry.Register<GraffitiMarketItemTemplate>();
            TemplateRegistry.Register<ChildGraffitiMarketItemTemplate>();
            TemplateRegistry.Register<GraffitiUserItemTemplate>();
        }

        private void RegisterModulesTemplates()
        {
            TemplateRegistry.Register<SlotMarketItemTemplate>();
            TemplateRegistry.Register<SlotUserItemTemplate>();
            TemplateRegistry.Register<SlotConfigTemplate>();
            TemplateRegistry.Register<ModuleTierConfigTemplate>();
            TemplateRegistry.Register<ModuleMarketItemTemplate>();
            TemplateRegistry.Register<ModuleUserItemTemplate>();
            TemplateRegistry.Register<PassiveModuleUserItemTemplate>();
            TemplateRegistry.Register<TriggerModuleUserItemTemplate>();
            TemplateRegistry.Register<GoldBonusModuleUserItemTemplate>();
            TemplateRegistry.Register<GarageModulesScreenTemplate>();
            TemplateRegistry.Register<ModuleCardItemTemplate>();
            TemplateRegistry.Register<ModuleCardMarketItemTemplate>();
            TemplateRegistry.Register<ModuleCardUserItemTemplate>();
            TemplateRegistry.Register<ModuleUpgradeInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEffectWithDurationTemplate>();
            TemplateRegistry.Register<ModuleUpgradeArmorEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeTurbospeedEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeTempblockEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeDamageEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeHealingEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeAcceleratedGearsEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEngineerEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeLifestealEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeBackhitModificatorEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEmergencyProtectionEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeRageEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeCommonMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeIcetrapEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeSpiderMineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeDroneEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEnergyInjectionEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeAdrenalineEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeEMPEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeForcefieldEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeGoldBonusActionInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeJumpEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeExternalImpactEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeFireRingEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeExplosiveMassEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeSapperEffectInfoTemplate>();
            TemplateRegistry.Register<ModuleUpgradeKamikadzeEffectInfoTemplate>();
        }

        private void RegisterPaintTemplates()
        {
            TemplateRegistry.Register<TankPaintMarketItemTemplate>();
            TemplateRegistry.Register<TankPaintUserItemTemplate>();
            TemplateRegistry.Register<WeaponPaintMarketItemTemplate>();
            TemplateRegistry.Register<WeaponPaintUserItemTemplate>();
        }

        private void RegisterPayableSystemsAndTemplates()
        {
            TemplateRegistry.Register<ChangeUIDTemplate>();
            EngineService.RegisterSystem(new ChangeUIDSystem());
        }

        private void RegisterPremiumSystemsAndTemplates()
        {
            TemplateRegistry.Register<PremiumBoostMarketItemTemplate>();
            TemplateRegistry.Register<PremiumBoostUserItemTemplate>();
            TemplateRegistry.Register<PremiumQuestMarketItemTemplate>();
            TemplateRegistry.Register<PremiumQuestUserItemTemplate>();
            EngineService.RegisterSystem(new PremiumItemSystem());
        }

        private void RegisterPresetTemplates()
        {
            TemplateRegistry.Register<PresetUserItemTemplate>();
            TemplateRegistry.Register<PresetMarketItemTemplate>();
        }

        private void RegisterPropertiesSystems()
        {
            EngineService.RegisterSystem(new ContainerSystem());
            EngineService.RegisterSystem(new OpenContainerSystem());
        }

        private void RegisterShellTemplates()
        {
            TemplateRegistry.Register<ShellMarketItemTemplate>();
            TemplateRegistry.Register<ShellUserItemTemplate>();
        }

        private void RegisterSkinTemplates()
        {
            TemplateRegistry.Register<HullSkinMarketItemTemplate>();
            TemplateRegistry.Register<HullSkinUserItemTemplate>();
            TemplateRegistry.Register<WeaponSkinMarketItemTemplate>();
            TemplateRegistry.Register<WeaponSkinUserItemTemplate>();
        }

        private void RegistersTemplates()
        {
            TemplateRegistry.Register<GarageItemTemplate>();
            TemplateRegistry.Register<ItemImagedTemplate>();
            TemplateRegistry.Register<CardTierTemplate>();
            TemplateRegistry.Register<GarageItemImagedTemplate>();
            TemplateRegistry.Register<MarketItemTemplate>();
            TemplateRegistry.Register<UserItemTemplate>();
            TemplateRegistry.Register<UpgradableUserItemTemplate>();
            TemplateRegistry.Register<GarageTemplate>();
            TemplateRegistry.Register<MountItemButtonTemplate>();
            TemplateRegistry.Register<GarageItemsTemplate>();
            TemplateRegistry.Register<DurationItemTemplate>();
            this.RegisterPresetTemplates();
            this.RegisterWeaponTemplates();
            this.RegisterTankTemplates();
            this.RegisterPaintTemplates();
            this.RegisterGraffitiTemplates();
            this.RegisterAvatarTemplates();
            this.RegisterButtonWithPriceTemplates();
            this.RegisterShellTemplates();
            this.RegisterSkinTemplates();
            this.RegisterContainerTemplates();
            this.RegisterCrystalTemplates();
            this.RegisterModulesTemplates();
            this.RegisterEnergyTemplates();
            this.RegisterTankTemplates();
            this.RegisterTutorialTemplates();
            this.RegisterDetailTemplates();
            this.RegisterGoldBonusTemplates();
            TemplateRegistry.Register<EnergyDailyBonusTemplate>();
            TemplateRegistry.Register<QuestDailyBonusTemplate>();
            TemplateRegistry.Register<MoneyDailyBonusTemplate>();
        }

        private void RegisterSystems()
        {
            EngineService.RegisterSystem(new TutorialBuildSystem());
            EngineService.RegisterSystem(new TutorialShowSystem());
            EngineService.RegisterSystem(new TutorialSkipSystem());
            EngineService.RegisterSystem(new ProfileSummarySectionUISystem());
            EngineService.RegisterSystem(new ProfileStatsSectionUISystem());
            EngineService.RegisterSystem(new ProfileAccountSectionUISystem());
            EngineService.RegisterSystem(new LogoutSystem());
            EngineService.RegisterSystem(new UserEmailShowSystem());
            EngineService.RegisterSystem(new LeagueRewardScreenSystem());
            EngineService.RegisterSystem(new ModulesBadgesSystem());
            EngineService.RegisterSystem(new LoginRewardSystem());
            EngineService.RegisterSystem(new ModuleSystem());
            EngineService.RegisterSystem(new MountItemSystem());
            EngineService.RegisterSystem(new GarageItemsScreenSystem());
            EngineService.RegisterSystem(new ItemsListScreenSystem());
            EngineService.RegisterSystem(new GarageListItemContentSystem());
            EngineService.RegisterSystem(new GarageNavigationSystem());
            EngineService.RegisterSystem(new QuickRegistrartionNavigationSystem());
            EngineService.RegisterSystem(new DisplayMountButtonSystem());
            EngineService.RegisterSystem(new DisplayBuyButtonSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionCheckSystem());
            EngineService.RegisterSystem(new DisplayUserItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new DisplayMarketItemRestrictionDescriptionSystem());
            EngineService.RegisterSystem(new GarageItemsCategoryScreenSystem());
            EngineService.RegisterSystem(new PriceLabelSystem());
            EngineService.RegisterSystem(new XPriceLabelSystem());
            EngineService.RegisterSystem(new CreateBuyItemPacksButtonsSystem());
            EngineService.RegisterSystem(new PresetSystem(EngineService.SystemRegistry));
            EngineService.RegisterSystem(new HiddenInGarageItemSystem());
            EngineService.RegisterSystem(new UserItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new MarketItemRestrictionBadgeSystem());
            EngineService.RegisterSystem(new BlockKeyboardForListOnConfirmButtonSystem());
            EngineService.RegisterSystem(new GarageSaleSystem());
            EngineService.RegisterSystem(new PromoCodeSystem());
            EngineService.RegisterSystem(new ContainersScreenSystem());
            EngineService.RegisterSystem(new ContainerContentScreenSystem());
            EngineService.RegisterSystem(new DisplayOpenContainerButtonSystem());
            EngineService.RegisterSystem(new DisplayContainerContentButtonSystem());
            EngineService.RegisterSystem(new GarageItemOnlyInContainerUISystem());
            EngineService.RegisterSystem(new BuyContainersSystem());
            EngineService.RegisterSystem(new BuyEnergySystem());
            EngineService.RegisterSystem(new MarketInfoSystem());
            EngineService.RegisterSystem(new MarketItemPriceSystem());
            this.RegisterPropertiesSystems();
            EngineService.RegisterSystem(new SlotsSystem());
            EngineService.RegisterSystem(new TankPartItemStatBuilderSystem());
            EngineService.RegisterSystem(new ModulesListSystem());
            EngineService.RegisterSystem(new ModulesScreenSystem());
            EngineService.RegisterSystem(new NewModulesScreenSystem());
            EngineService.RegisterSystem(new ModulesTutorialSystem());
            EngineService.RegisterSystem(new ModulesButtonsSystem());
            EngineService.RegisterSystem(new PresetUISystem());
            EngineService.RegisterSystem(new CustomizationUISystem());
            EngineService.RegisterSystem(new MainScreenSystem());
            EngineService.RegisterSystem(new ChangeScreenLogSystem());
            EngineService.RegisterSystem(new GoldBonusCounterSystem());
            EngineService.RegisterSystem(new GaragePriceSystem());
            GarageItemsRegistrySystem service = new GarageItemsRegistrySystem();
            ServiceRegistry.Current.RegisterService<GarageItemsRegistry>(service);
            EngineService.RegisterSystem(service);
            EngineService.RegisterSystem(new ArmsRaceUISystem());
            EngineService.RegisterSystem(new ItemUnlockUISystem());
            EngineService.RegisterSystem(new WelcomeScreenSystem());
            EngineService.RegisterSystem(new DailyBonusScreenSystem());
            EngineService.RegisterSystem(new ExitDialogSystem());
            EngineService.RegisterSystem(new ReleaseGiftsScreenSystem());
            EngineService.RegisterSystem(new PremiumUiShopSystem());
            EngineService.RegisterSystem(new PremiumLearnMoreSystem());
            EngineService.RegisterSystem(new PremiumMoneyBonusSystem());
            EngineService.RegisterSystem(new GoldBoxesUiShopSystem());
            EngineService.RegisterSystem(new GoldBoxesLearnMoreSystem());
            this.RegisterAvatarSystems();
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegistersTemplates();
            this.RegisterSystems();
            this.RegisterPayableSystemsAndTemplates();
            this.RegisterPremiumSystemsAndTemplates();
        }

        private void RegisterTankTemplates()
        {
            TemplateRegistry.Register<TankUserItemTemplate>();
            TemplateRegistry.Register<TankMarketItemTemplate>();
        }

        private void RegisterTutorialTemplates()
        {
            TemplateRegistry.Register<TutorialsConfigurationTemplate>();
            TemplateRegistry.Register<TutorialDataTemplate>();
            TemplateRegistry.Register<TutorialStepDataTemplate>();
            TemplateRegistry.Register<TutorialHighlightTankDataTemplate>();
            TemplateRegistry.Register<TutorialSelectItemDataTemplate>();
            TemplateRegistry.Register<TutorialRewardDataTemplate>();
        }

        private void RegisterWeaponTemplates()
        {
            TemplateRegistry.Register<WeaponItemTemplate>();
            TemplateRegistry.Register<WeaponMarketItemTemplate>();
            TemplateRegistry.Register<HammerMarketItemTemplate>();
            TemplateRegistry.Register<HammerUserItemTemplate>();
            TemplateRegistry.Register<RailgunMarketItemTemplate>();
            TemplateRegistry.Register<RailgunUserItemTemplate>();
            TemplateRegistry.Register<RicochetMarketItemTemplate>();
            TemplateRegistry.Register<RicochetUserItemTemplate>();
            TemplateRegistry.Register<ShaftMarketItemTemplate>();
            TemplateRegistry.Register<ShaftUserItemTemplate>();
            TemplateRegistry.Register<SmokyMarketItemTemplate>();
            TemplateRegistry.Register<SmokyUserItemTemplate>();
            TemplateRegistry.Register<ThunderMarketItemTemplate>();
            TemplateRegistry.Register<ThunderUserItemTemplate>();
            TemplateRegistry.Register<TwinsMarketItemTemplate>();
            TemplateRegistry.Register<TwinsUserItemTemplate>();
            TemplateRegistry.Register<FlamethrowerMarketItemTemplate>();
            TemplateRegistry.Register<FlamethrowerUserItemTemplate>();
            TemplateRegistry.Register<FreezeMarketItemTemplate>();
            TemplateRegistry.Register<FreezeUserItemTemplate>();
            TemplateRegistry.Register<VulcanMarketItemTemplate>();
            TemplateRegistry.Register<VulcanUserItemTemplate>();
            TemplateRegistry.Register<IsisMarketItemTemplate>();
            TemplateRegistry.Register<IsisUserItemTemplate>();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

