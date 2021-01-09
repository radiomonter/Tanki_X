namespace Tanks.Lobby.ClientSettings.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class ScreenResolutionSettingsCarouselBuilderSystem : ECSSystem
    {
        private const string SCREEN_RESOLUTION_ENTITY_NAME = "ScreenResolutionEntity";
        private const string SCREEN_RESOLUTION_CAPTION_FORMAT = "{0} x {1}";

        [OnEventFire]
        public void Build(NodeAddedEvent e, ScreenResolutionSettingsCarouselNode carousel)
        {
            CarouselGroupComponent carouselGroup = carousel.carouselGroup;
            List<Resolution> screenResolutions = GraphicsSettings.INSTANCE.ScreenResolutions;
            int count = screenResolutions.Count;
            List<Entity> list2 = new List<Entity>();
            for (int i = 0; i < count; i++)
            {
                Resolution resolution = screenResolutions[i];
                int width = resolution.width;
                int height = resolution.height;
                Entity entity = base.CreateEntity("ScreenResolutionEntity");
                entity.AddComponentAndGetInstance<CarouselItemTextComponent>().LocalizedCaption = $"{width} x {height}";
                ScreenResolutionVariantComponent component2 = entity.AddComponentAndGetInstance<ScreenResolutionVariantComponent>();
                component2.Width = width;
                component2.Height = height;
                carouselGroup.Attach(entity);
                list2.Add(entity);
            }
            CarouselItemCollectionComponent component = new CarouselItemCollectionComponent {
                Items = list2
            };
            carousel.Entity.AddComponent(component);
        }

        public class ScreenResolutionSettingsCarouselNode : Node
        {
            public CarouselComponent carousel;
            public CarouselGroupComponent carouselGroup;
            public ScreenResolutionSettingsCarouselComponent screenResolutionSettingsCarousel;
        }
    }
}

