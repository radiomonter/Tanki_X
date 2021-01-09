namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class GarageItemsCategoryScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<SingleNode<ContainerContentItemComponent>, Entity> <>f__am$cache0;

        [OnEventFire]
        public void GoToContainerScreenFromContainerItem(ButtonClickEvent e, SingleNode<GoToContainersScreenButtonComponent> button, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            Entity[] entities = new Entity[] { selectedItem.component.SelectedItem, button.Entity };
            base.NewEvent<ShowParentItemBySelectedItemEvent>().AttachAll(entities).Schedule();
        }

        [OnEventFire]
        public void GoToContainerScreenFromContainerItem(ShowParentItemBySelectedItemEvent e, SingleNode<MarketItemComponent> selectedItem, [JoinByContainerContentItem] SingleNode<ContainerContentItemComponent> containerContent, [JoinByContainer] ContainerMarketItemNode container, SingleNode<GoToContainersScreenButtonComponent> button)
        {
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = GarageCategory.CONTAINERS,
                SelectedItem = container.Entity
            };
            base.ScheduleEvent(eventInstance, button);
        }

        [OnEventFire]
        public void ShowContainerContentScreen(ButtonClickEvent e, SingleNode<ContainerContentButtonComponent> containerContentButton, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            base.ScheduleEvent<ShowContainerContentScreenEvent>(selectedItem.component.SelectedItem);
        }

        [OnEventFire]
        public void ShowContainerContentScreen(ShowContainerContentScreenEvent e, ContainerItemNode container, [JoinByContainer] ICollection<SingleNode<ContainerContentItemComponent>> items)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.Entity;
            }
            this.ShowItemsListScreen<ContainerContentScreenComponent>(items.Select<SingleNode<ContainerContentItemComponent>, Entity>(<>f__am$cache0));
        }

        [OnEventFire]
        public void ShowContainers(ButtonClickEvent e, SingleNode<ContainersButtonComponent> button)
        {
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = GarageCategory.CONTAINERS
            };
            base.ScheduleEvent(eventInstance, button);
        }

        private void ShowItemsListScreen<T>(IEnumerable<Entity> items) where T: MonoBehaviour, Component
        {
            Entity context = base.CreateEntity("GarageItems");
            context.AddComponent(new ItemsListForViewComponent(new List<Entity>(items)));
            context.AddComponent<SelectedItemComponent>();
            ShowScreenLeftEvent<T> eventInstance = new ShowScreenLeftEvent<T>();
            eventInstance.SetContext(context, true);
            base.ScheduleEvent(eventInstance, context);
        }

        public class ContainerItemNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public DescriptionBundleItemComponent descriptionBundleItem;
        }

        public class ContainerMarketItemNode : GarageItemsCategoryScreenSystem.ContainerItemNode
        {
            public MarketItemComponent marketItem;
        }

        public class PaintButtonNode : Node
        {
            public TankPaintButtonComponent tankPaintButton;
            public TextMappingComponent textMapping;
        }

        public class ShowParentItemBySelectedItemEvent : Event
        {
        }

        public class ShowSkinItemsListScreenBySelectedItemEvent : Event
        {
        }
    }
}

