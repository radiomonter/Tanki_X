namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class GraphicsSettingsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void Apply(ButtonClickEvent e, SingleNode<ApplyButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen, [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualityCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualityCarousel, [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualityCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, [JoinByCarousel] CurrentWindowModeCarouselItemNode windowMode, SingleNode<ApplyButtonComponent> button1, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [JoinByCarousel] CurrentScreenResolutionCarouselItemNode screenResolutionItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            <Apply>c__AnonStorey0 storey = new <Apply>c__AnonStorey0();
            GraphicsSettings iNSTANCE = GraphicsSettings.INSTANCE;
            bool windowed = windowMode.windowModeVariant.Windowed;
            iNSTANCE.ApplyWindowMode(windowed);
            windowModeSettingsCarousel.windowModeSettingsCarousel.FullScreen = !windowed;
            ScreenResolutionVariantComponent screenResolutionVariant = screenResolutionItem.screenResolutionVariant;
            storey.resolutionWidth = screenResolutionVariant.Width;
            storey.resolutionHeight = screenResolutionVariant.Height;
            graphicsSettingsIndexes.component.CurrentScreenResolutionIndex = iNSTANCE.ScreenResolutions.FindIndex(new Predicate<Resolution>(storey.<>m__0));
            iNSTANCE.ApplyScreenResolution(storey.resolutionWidth, storey.resolutionHeight, windowed);
            graphicsSettingsIndexes.component.CurrentSaturationLevelIndex = saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentVegetationQualityIndex = vegetationQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentGrassQualityIndex = grassQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentAntialiasingQualityIndex = antialiasingQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentRenderResolutionQualityIndex = renderResolutionQualityCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentAnisotropicQualityIndex = anisotropicQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentShadowQualityIndex = shadowQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentParticleQualityIndex = particleQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentTextureQualityIndex = textureQualitySettingCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = cartridgeAmountCarousel.carouselCurrentItemIndex.Index;
            graphicsSettingsIndexes.component.CurrentVSyncQualityIndex = vsyncQualityCarousel.carouselCurrentItemIndex.Index;
            if (!screen.graphicsSettingsScreen.NeedToReloadApplication)
            {
                base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
            }
            else
            {
                iNSTANCE.ApplyQualityLevel(qualityCarousel.carouselCurrentItemIndex.Index);
                base.ScheduleEvent<SwitchToEntranceSceneEvent>(button);
            }
        }

        [OnEventFire]
        public void Cancel(ButtonClickEvent e, SingleNode<CancelButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen, [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualitySettingsCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualitySettingsCarousel, [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            base.ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.CurrentQualityLevel), qualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentScreenResolutionIndex), screenResolutionCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentWindowModeIndex), windowModeCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentSaturationLevelIndex), saturationLevelSettingsCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentAntialiasingQualityIndex), antialiasingQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentRenderResolutionQualityIndex), renderResolutionQualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentVegetationQualityIndex), vegetationQualitySettingsCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentGrassQualityIndex), grassQualitySettingsCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentAnisotropicQualityIndex), anisotropicQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentShadowQualityIndex), shadowQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentParticleQualityIndex), particleQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentTextureQualityIndex), textureQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentCartridgeCaseAmountIndex), cartridgeAmountCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.CurrentVSyncQualityIndex), vsyncQualityCarousel);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyQualitySettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen)
        {
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyScreenResolutionSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen)
        {
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyWindowModeSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen)
        {
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyAnisotropicQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyAnisotropicQualityCarouselNode carousel1, [JoinByCarousel] CurrentAnisotropicQualityCarouselItemNode anisotropicQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyAnisotropicQuality(anisotropicQualityCarouselItem.anisotropicQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyAntialiasingQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyAntialiasingQualityCarouselNode carousel1, [JoinByCarousel] CurrentAntialiasingQualityCarouselItemNode antialiasingQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyAntialiasingQuality(antialiasingQualityCarouselItem.antialiasingQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyCartridgeAmountCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyCartridgeAmountCarouselNode carousel1, [JoinByCarousel] CurrentCartridgeCaseAmountCarouselItemNode caseAmountCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            int currentValue = caseAmountCarouselItem.cartridgeCaseSettingVariant.Value;
            GraphicsSettings.INSTANCE.ApplyCartridgeCaseAmount(currentValue);
            graphicsSettingsIndexes.component.CurrentCartridgeCaseAmountIndex = currentValue;
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyGrassQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyGrassQualityCarouselNode carousel1, [JoinByCarousel] CurrentGrassQualityCarouselItemNode grassQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyGrassLevel(grassQualityCarouselItem.grassSettings.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyParticleQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyParticleQualityCarouselNode carousel1, [JoinByCarousel] CurrentParticleQualityCarouselItemNode particleQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyParticleQuality(particleQualityCarouselItem.particleQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyRenderResolutionQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyRenderResolutionQualityCarouselNode carousel1, [JoinByCarousel] CurrentRenderResolutionQualityCarouselItemNode renderResolutionQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyRenderResolutionQuality(renderResolutionQualityCarouselItem.renderResolutionQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadySaturationLevelSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadySaturationLevelSettingsCarouselNode carousel1, [JoinByCarousel] CurrentSaturationLevelCarouselItemNode saturationLevelCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplySaturationLevel(saturationLevelCarouselItem.saturationLevelVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyShadowQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyShadowQualityCarouselNode carousel1, [JoinByCarousel] CurrentShadowQualityCarouselItemNode shadowQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyShadowQuality(shadowQualityCarouselItem.shadowQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyTextureQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyTextureQualityCarouselNode carousel1, [JoinByCarousel] CurrentTextureQualityCarouselItemNode textureQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyTextureQuality(textureQualityCarouselItem.textureQualityVariant.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyVegetationQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyVegetationQualityCarouselNode carousel1, [JoinByCarousel] CurrentVegetationQualityCarouselItemNode vegetationQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettings.INSTANCE.ApplyVegetationLevel(vegetationQualityCarouselItem.vegetationSettings.Value);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(CarouselItemChangedEvent e, ReadyVSyncQualityCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, ReadyVSyncQualityCarouselNode carousel1, [JoinByCarousel] CurrentVSyncCarouselItemNode vsyncQualityCarouselItem, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            int currentValue = vsyncQualityCarouselItem.vSyncSettingVariant.Value;
            GraphicsSettings.INSTANCE.ApplyVSyncQuality(currentValue);
            base.ScheduleEvent<GraphicsSettingsChangedEvent>(screen);
        }

        [OnEventFire]
        public void ChangeCarouselItem(GraphicsSettingsChangedEvent e, GraphicsSettingsScreenNode screen, [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualitySettingsCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualitySettingsCarousel, [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            this.UpdateRightMenuElements(qualityCarousel, screenResolutionCarousel, windowModeSettingsCarousel, saturationLevelSettingsCarousel, antialiasingQualitySettingCarousel, renderResolutionQualityCarousel, vegetationQualitySettingsCarousel, grassQualitySettingsCarousel, anisotropicQualitySettingCarousel, shadowQualitySettingCarousel, particleQualitySettingCarousel, textureQualitySettingCarousel, cartridgeAmountCarousel, vsyncQualityCarousel, screen, graphicsSettingsIndexes);
        }

        [OnEventFire]
        public void ChangeCustomSettings(CheckboxEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings, [JoinAll] AmbientOcclusionCheckboxNode AmbientOcclusionCheckbox, [JoinAll] BloomCheckboxNode BloomCheckbox, [JoinAll] ChromaticAberrationCheckboxNode ChromaticAberrationCheckbox, [JoinAll] GrainCheckboxNode GrainCheckbox, [JoinAll] VignetteCheckboxNode VignetteCheckbox, [JoinAll] DisableBattleNotificationsCheckboxNode battleNotificationsCheckbox)
        {
            settings.component.CustomSettings = checkboxNode.checkbox.IsChecked;
            AmbientOcclusionCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            BloomCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ChromaticAberrationCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrainCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VignetteCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AmbientOcclusionCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            BloomCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            ChromaticAberrationCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            GrainCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            VignetteCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            battleNotificationsCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeCustomSettings(CheckboxEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings, [JoinAll] TextureQualityCarouselNode TextureQualityCarousel, [JoinAll] ShadowQualityCarouselNode ShadowQualityCarousel, [JoinAll] ParticleQualityCarouselNode ParticleQualityCarousel, [JoinAll] VegetationQualityCarouselNode VegetationQualityCarousel, [JoinAll] GrassQualityCarouselNode GrassQualityCarousel, [JoinAll] AntialiasingQualityCarouselNode AntialiasingQualityCarousel, [JoinAll] RenderResolutionQualityCarouselNode RenderResolutionQualityCarousel, [JoinAll] AnisotropicQualityCarouselNode AnisotropicQualityCarousel, [JoinAll] CartridgeCaseAmountCarouselNode cartridgeCaseAmountCarousel, [JoinAll] VSyncQualityCarouselNode vsyncQualityCarousel)
        {
            settings.component.CustomSettings = checkboxNode.checkbox.IsChecked;
            TextureQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ShadowQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ParticleQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VegetationQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrassQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AntialiasingQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            RenderResolutionQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AnisotropicQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            cartridgeCaseAmountCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            vsyncQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            TextureQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ShadowQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ParticleQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            VegetationQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            GrassQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AntialiasingQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            RenderResolutionQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AnisotropicQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            cartridgeCaseAmountCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            vsyncQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, AmbientOcclusionCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.AmbientOcclusion = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, BloomCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.Bloom = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, ChromaticAberrationCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.ChromaticAberration = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, DisableBattleNotificationsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.DisableBattleNotifications = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, GrainCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.Grain = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeProcessingQualitySettings(CheckboxEvent e, VignetteCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            settings.component.Vignette = checkboxNode.checkbox.IsChecked;
            base.ScheduleEvent(new SettingsChangedEvent<PostProcessingQualityVariantComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void DetectFullScreenChange(UpdateEvent e, ReadyWindowModeSettingsCarouselNode carousel, [JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            bool fullScreen = Screen.fullScreen;
            if (carousel.windowModeSettingsCarousel.FullScreen != fullScreen)
            {
                base.ScheduleEvent(new SetCarouselItemIndexEvent(graphicsSettingsIndexes.component.CurrentWindowModeIndex), carousel);
            }
            carousel.windowModeSettingsCarousel.FullScreen = fullScreen;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, AmbientOcclusionCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.AmbientOcclusion;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, BloomCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.Bloom;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, ChromaticAberrationCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.ChromaticAberration;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.CustomSettings;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, DisableBattleNotificationsCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.DisableBattleNotifications;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, GrainCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.Grain;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, VignetteCheckboxNode checkboxNode, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            checkboxNode.checkbox.IsChecked = settings.component.Vignette;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, AmbientOcclusionCheckboxNode AmbientOcclusionCheckbox, BloomCheckboxNode BloomCheckbox, ChromaticAberrationCheckboxNode ChromaticAberrationCheckbox, GrainCheckboxNode GrainCheckbox, VignetteCheckboxNode VignetteCheckbox, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            AmbientOcclusionCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            BloomCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ChromaticAberrationCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrainCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VignetteCheckbox.dependentInteractivity.GetComponent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AmbientOcclusionCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            BloomCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            ChromaticAberrationCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            GrainCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
            VignetteCheckbox.dependentInteractivity.HideCheckbox(settings.component.CustomSettings);
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CustomSettingsCheckboxNode checkboxNode, TextureQualityCarouselNode TextureQualityCarousel, ShadowQualityCarouselNode ShadowQualityCarousel, ParticleQualityCarouselNode ParticleQualityCarousel, VegetationQualityCarouselNode VegetationQualityCarousel, GrassQualityCarouselNode GrassQualityCarousel, AntialiasingQualityCarouselNode AntialiasingQualityCarousel, RenderResolutionQualityCarouselNode RenderResolutionQualityCarousel, AnisotropicQualityCarouselNode AnisotropicQualityCarousel, CartridgeCaseAmountCarouselNode cartridgeCaseAmountCarousel, VSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings)
        {
            TextureQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ShadowQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            ParticleQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            VegetationQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            GrassQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AntialiasingQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            RenderResolutionQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            AnisotropicQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            cartridgeCaseAmountCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            vsyncQualityCarousel.dependentInteractivity.GetComponentInParent<LayoutElement>().ignoreLayout = !settings.component.CustomSettings;
            TextureQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ShadowQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            ParticleQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            VegetationQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            GrassQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AntialiasingQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            RenderResolutionQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            AnisotropicQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            cartridgeCaseAmountCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
            vsyncQualityCarousel.dependentInteractivity.HideCarouselInteractable(settings.component.CustomSettings);
        }

        [OnEventFire]
        public void InitAnisotropicQualityCarousel(NodeAddedEvent e, AnisotropicQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentAnisotropicQualityIndex));
        }

        [OnEventFire]
        public void InitAntialiasingQualityCarousel(NodeAddedEvent e, AntialiasingQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentAntialiasingQualityIndex));
        }

        [OnEventFire]
        public void InitCartridgeAmountCarousel(NodeAddedEvent e, CartridgeCaseAmountCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentCartridgeCaseAmountIndex));
        }

        [OnEventFire]
        public void InitDataFromCarousels(NodeAddedEvent e, GraphicsSettingsScreenNode screen, [Context, JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [Context, JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [Context, JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, [Context, JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [Context, JoinByScreen] ReadyVegetationQualityCarouselNode vegetationLevelSettingsCarousel, [Context, JoinByScreen] ReadyGrassQualityCarouselNode grassLevelSettingsCarousel, [Context, JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel, [Context, JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [Context, JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [Context, JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [Context, JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [Context, JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [Context, JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [Context, JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualitCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            this.UpdateRightMenuElements(qualityCarousel, screenResolutionCarousel, windowModeSettingsCarousel, saturationLevelSettingsCarousel, antialiasingQualitySettingCarousel, renderResolutionQualityCarousel, vegetationLevelSettingsCarousel, grassLevelSettingsCarousel, anisotropicQualitySettingCarousel, shadowQualitySettingCarousel, particleQualitySettingCarousel, textureQualitySettingCarousel, cartridgeAmountCarousel, vsyncQualitCarousel, screen, graphicsSettingsIndexes);
        }

        [OnEventFire]
        public void InitGrassQualityCarousel(NodeAddedEvent e, GrassQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentGrassQualityIndex));
        }

        [OnEventFire]
        public void InitParticleQualityCarousel(NodeAddedEvent e, ParticleQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentParticleQualityIndex));
        }

        [OnEventFire]
        public void InitQualitySettingsCarousel(NodeAddedEvent e, QualitySettingsCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(GraphicsSettings.INSTANCE.CurrentQualityLevel));
        }

        [OnEventFire]
        public void InitRenderResolutionQualityCarousel(NodeAddedEvent e, RenderResolutionQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentRenderResolutionQualityIndex));
        }

        [OnEventFire]
        public void InitSaturationLevelCarousel(NodeAddedEvent e, SaturationLevelCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentSaturationLevelIndex));
        }

        [OnEventFire]
        public void InitScreenResolutionSettingsCarousel(NodeAddedEvent e, ScreenResolutionSettingsCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentScreenResolutionIndex));
        }

        [OnEventFire]
        public void InitShadowQualityCarousel(NodeAddedEvent e, ShadowQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentShadowQualityIndex));
        }

        [OnEventFire]
        public void InitTextureQualityCarousel(NodeAddedEvent e, TextureQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentTextureQualityIndex));
        }

        [OnEventFire]
        public void InitVegetationQualityCarousel(NodeAddedEvent e, VegetationQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentVegetationQualityIndex));
        }

        [OnEventFire]
        public void InitVSyncQualityCarousel(NodeAddedEvent e, VSyncQualityCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingIndexes)
        {
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingIndexes.component.CurrentVSyncQualityIndex));
        }

        [OnEventFire]
        public void InitWindowModeSettingsCarousel(NodeAddedEvent e, WindowModeSettingsCarouselNode carousel, [Context, JoinByScreen] GraphicsSettingsScreenNode screen, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            carousel.windowModeSettingsCarousel.FullScreen = Screen.fullScreen;
            carousel.Entity.AddComponent(new CarouselCurrentItemIndexComponent(graphicsSettingsIndexes.component.CurrentWindowModeIndex));
        }

        [OnEventFire]
        public void SetDefault(ButtonClickEvent e, SingleNode<DefaultButtonComponent> button, [JoinByScreen] GraphicsSettingsScreenNode screen, [JoinByScreen] ReadyQualitySettingsCarouselNode qualityCarousel, [JoinByScreen] ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, [JoinByScreen] ReadyWindowModeSettingsCarouselNode windowModeCarousel, [JoinByScreen] ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, [JoinByScreen] ReadyAntialiasingQualityCarouselNode antialiasingQualityCarousel, [JoinByScreen] ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, [JoinByScreen] ReadyVegetationQualityCarouselNode vegetationQualityCarousel, [JoinByScreen] ReadyGrassQualityCarouselNode grassQualityCarousel, [JoinByScreen] ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, [JoinByScreen] ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, [JoinByScreen] ReadyParticleQualityCarouselNode particleQualitySettingCarousel, [JoinByScreen] ReadyTextureQualityCarouselNode textureQualitySettingCarousel, [JoinByScreen] ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, [JoinByScreen] ReadyVSyncQualityCarouselNode vsyncQualityCarousel, [JoinAll] SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            base.ScheduleEvent(new SetCarouselItemIndexEvent(GraphicsSettings.INSTANCE.DefaultQualityLevel), qualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultScreenResolutionIndex), screenResolutionCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultWindowModeIndex), windowModeCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultSaturationLevelIndex), saturationLevelSettingsCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultVegetationQualityIndex), vegetationQualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultGrassQualityIndex), grassQualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultAntialiasingQualityIndex), antialiasingQualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultRenderResolutionQualityIndex), renderResolutionQualityCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultAnisotropicQualityIndex), anisotropicQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultShadowQualityIndex), shadowQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultParticleQualityIndex), particleQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultTextureQualityIndex), textureQualitySettingCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultCartridgeCaseAmountIndex), cartridgeAmountCarousel);
            base.ScheduleEvent(new SetCarouselItemIndexEvent(component.DefaultVSyncQualityIndex), vsyncQualityCarousel);
        }

        private void UpdateRightMenuElements(ReadyQualitySettingsCarouselNode qualityCarousel, ReadyScreenResolutionSettingsCarouselNode screenResolutionCarousel, ReadyWindowModeSettingsCarouselNode windowModeSettingsCarousel, ReadySaturationLevelSettingsCarouselNode saturationLevelSettingsCarousel, ReadyAntialiasingQualityCarouselNode antialiasingQualitySettingCarousel, ReadyRenderResolutionQualityCarouselNode renderResolutionQualityCarousel, ReadyVegetationQualityCarouselNode vegetationQualitySettingCarousel, ReadyGrassQualityCarouselNode grassQualitySettingCarousel, ReadyAnisotropicQualityCarouselNode anisotropicQualitySettingCarousel, ReadyShadowQualityCarouselNode shadowQualitySettingCarousel, ReadyParticleQualityCarouselNode particleQualitySettingCarousel, ReadyTextureQualityCarouselNode textureQualitySettingCarousel, ReadyCartridgeAmountCarouselNode cartridgeAmountCarousel, ReadyVSyncQualityCarouselNode vsyncQualityCarousel, GraphicsSettingsScreenNode screen, SingleNode<GraphicsSettingsIndexesComponent> graphicsSettingsIndexes)
        {
            GraphicsSettingsScreenComponent graphicsSettingsScreen = screen.graphicsSettingsScreen;
            GraphicsSettingsIndexesComponent component = graphicsSettingsIndexes.component;
            int index = qualityCarousel.carouselCurrentItemIndex.Index;
            int num2 = screenResolutionCarousel.carouselCurrentItemIndex.Index;
            int num3 = windowModeSettingsCarousel.carouselCurrentItemIndex.Index;
            int num4 = saturationLevelSettingsCarousel.carouselCurrentItemIndex.Index;
            int num5 = vegetationQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num6 = grassQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num7 = antialiasingQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num8 = renderResolutionQualityCarousel.carouselCurrentItemIndex.Index;
            int num9 = anisotropicQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num10 = shadowQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num11 = particleQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num12 = textureQualitySettingCarousel.carouselCurrentItemIndex.Index;
            int num13 = cartridgeAmountCarousel.carouselCurrentItemIndex.Index;
            int num14 = vsyncQualityCarousel.carouselCurrentItemIndex.Index;
            bool needToShowChangePerfomance = index > GraphicsSettings.INSTANCE.DefaultQualityLevel;
            bool isCurrentQuality = index == GraphicsSettings.INSTANCE.CurrentQualityLevel;
            bool needToShowReload = ((index != GraphicsSettings.INSTANCE.CurrentQualityLevel) || ((num5 != component.CurrentVegetationQualityIndex) || ((num6 != component.CurrentGrassQualityIndex) || ((num7 != component.CurrentAntialiasingQualityIndex) || ((num8 != component.CurrentRenderResolutionQualityIndex) || ((num9 != component.CurrentAnisotropicQualityIndex) || ((num10 != component.CurrentShadowQualityIndex) || ((num11 != component.CurrentParticleQualityIndex) || (num12 != component.CurrentTextureQualityIndex))))))))) || (num14 != component.CurrentVSyncQualityIndex);
            bool needToShowButtons = ((num3 != component.CurrentWindowModeIndex) || (num2 != component.CurrentScreenResolutionIndex)) || needToShowReload;
            graphicsSettingsScreen.SetPerfomanceWarningVisibility(needToShowChangePerfomance, isCurrentQuality);
            graphicsSettingsScreen.SetVisibilityForChangeSettingsControls(needToShowReload, needToShowButtons);
            graphicsSettingsScreen.SetDefaultButtonVisibility(((index != GraphicsSettings.INSTANCE.DefaultQualityLevel) || ((num2 != component.DefaultScreenResolutionIndex) || ((num3 != component.DefaultWindowModeIndex) || ((num4 != component.DefaultSaturationLevelIndex) || ((num5 != component.DefaultVegetationQualityIndex) || ((num6 != component.DefaultGrassQualityIndex) || ((num7 != component.DefaultAntialiasingQualityIndex) || ((num8 != component.DefaultRenderResolutionQualityIndex) || ((num9 != component.DefaultAnisotropicQualityIndex) || ((num10 != component.DefaultShadowQualityIndex) || ((num11 != component.DefaultParticleQualityIndex) || (num12 != component.DefaultTextureQualityIndex)))))))))))) || (num14 != component.DefaultVSyncQualityIndex));
        }

        [CompilerGenerated]
        private sealed class <Apply>c__AnonStorey0
        {
            internal int resolutionWidth;
            internal int resolutionHeight;

            internal bool <>m__0(Resolution r) => 
                (r.width == this.resolutionWidth) && (r.height == this.resolutionHeight);
        }

        public class AmbientOcclusionCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public AmbientOcclusionCheckboxComponent ambientOcclusionCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class AnisotropicQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public AnisotropicQualityCarouselComponent anisotropicQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class AntialiasingQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public AntialiasingQualityCarouselComponent antialiasingQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class BloomCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public BloomCheckboxComponent bloomCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class CarouselNode : Node
        {
            public CarouselComponent carousel;
            public CarouselItemCollectionComponent carouselItemCollection;
            public ScreenGroupComponent screenGroup;
        }

        public class CartridgeCaseAmountCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public CartridgeCaseAmountCarouselComponent cartridgeCaseAmountCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class ChromaticAberrationCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public ChromaticAberrationCheckboxComponent chromaticAberrationCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class CurrentAnisotropicQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public AnisotropicQualityVariantComponent anisotropicQualityVariant;
        }

        public class CurrentAntialiasingQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public AntialiasingQualityVariantComponent antialiasingQualityVariant;
        }

        public class CurrentCarouselItemNode : Node
        {
            public CarouselCurrentItemComponent carouselCurrentItem;
        }

        public class CurrentCartridgeCaseAmountCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public CartridgeCaseSettingVariantComponent cartridgeCaseSettingVariant;
        }

        public class CurrentGrassQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public GrassSettingsComponent grassSettings;
        }

        public class CurrentParticleQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public ParticleQualityVariantComponent particleQualityVariant;
        }

        public class CurrentRenderResolutionQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public RenderResolutionQualityVariantComponent renderResolutionQualityVariant;
        }

        public class CurrentSaturationLevelCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public SaturationLevelVariantComponent saturationLevelVariant;
        }

        public class CurrentScreenResolutionCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public ScreenResolutionVariantComponent screenResolutionVariant;
        }

        public class CurrentShadowQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public ShadowQualityVariantComponent shadowQualityVariant;
        }

        public class CurrentTextureQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public TextureQualityVariantComponent textureQualityVariant;
        }

        public class CurrentVegetationQualityCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public VegetationSettingsComponent vegetationSettings;
        }

        public class CurrentVSyncCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public VSyncSettingVariantComponent vSyncSettingVariant;
        }

        public class CurrentWindowModeCarouselItemNode : GraphicsSettingsScreenSystem.CurrentCarouselItemNode
        {
            public WindowModeVariantComponent windowModeVariant;
        }

        public class CustomSettingsCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public CustomSettingsCheckboxComponent customSettingsCheckbox;
        }

        public class DisableBattleNotificationsCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public DisableBattleNotificationsCheckboxComponent disableBattleNotificationsCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class GrainCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public GrainCheckboxComponent grainCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class GraphicsSettingsScreenNode : Node
        {
            public GraphicsSettingsScreenComponent graphicsSettingsScreen;
            public ScreenGroupComponent screenGroup;
        }

        public class GrassQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public GrassQualityCarouselComponent grassQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class ParticleQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public ParticleQualityCarouselComponent particleQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class QualitySettingsCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
        }

        public class ReadyAnisotropicQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public AnisotropicQualityCarouselComponent anisotropicQualityCarousel;
        }

        public class ReadyAntialiasingQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public AntialiasingQualityCarouselComponent antialiasingQualityCarousel;
        }

        public class ReadyCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public CarouselCurrentItemIndexComponent carouselCurrentItemIndex;
        }

        public class ReadyCartridgeAmountCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public CartridgeCaseAmountCarouselComponent cartridgeCaseAmountCarousel;
        }

        public class ReadyGrassQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public GrassQualityCarouselComponent grassQualityCarousel;
        }

        public class ReadyParticleQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public ParticleQualityCarouselComponent particleQualityCarousel;
        }

        public class ReadyQualitySettingsCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public QualitySettingsCarouselComponent qualitySettingsCarousel;
        }

        public class ReadyRenderResolutionQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public RenderResolutionQualityCarouselComponent renderResolutionQualityCarousel;
        }

        public class ReadySaturationLevelSettingsCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public SaturationLevelCarouselComponent saturationLevelCarousel;
        }

        public class ReadyScreenResolutionSettingsCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }

        public class ReadyShadowQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public ShadowQualityCarouselComponent shadowQualityCarousel;
        }

        public class ReadyTextureQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public TextureQualityCarouselComponent textureQualityCarousel;
        }

        public class ReadyVegetationQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public VegetationQualityCarouselComponent vegetationQualityCarousel;
        }

        public class ReadyVSyncQualityCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public VSyncQualityCarouselComponent vSyncQualityCarousel;
        }

        public class ReadyWindowModeSettingsCarouselNode : GraphicsSettingsScreenSystem.ReadyCarouselNode
        {
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
        }

        public class RenderResolutionQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public RenderResolutionQualityCarouselComponent renderResolutionQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class SaturationLevelCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public SaturationLevelCarouselComponent saturationLevelCarousel;
        }

        public class ScreenResolutionSettingsCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }

        public class ShadowQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public ShadowQualityCarouselComponent shadowQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class TextureQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public TextureQualityCarouselComponent textureQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class VegetationQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public VegetationQualityCarouselComponent vegetationQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class VignetteCheckboxNode : Node
        {
            public CheckboxComponent checkbox;
            public VignetteCheckboxComponent vignetteCheckbox;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class VSyncQualityCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public VSyncQualityCarouselComponent vSyncQualityCarousel;
            public DependentInteractivityComponent dependentInteractivity;
        }

        public class WindowModeSettingsCarouselNode : GraphicsSettingsScreenSystem.CarouselNode
        {
            public WindowModeSettingsCarouselComponent windowModeSettingsCarousel;
        }
    }
}

