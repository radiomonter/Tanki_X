namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class ContainerContentScreenSystem : ECSSystem
    {
        private void MarkItem(GameObject itemGameObject, bool active)
        {
            itemGameObject.GetComponentInChildrenIncludeInactive<TickMarkerComponent>().gameObject.SetActive(active);
        }

        [OnEventComplete]
        public void MarkUserItems(ShowGarageItemsEvent e, [Combine] ContainerContentItemNode containerContentItem, [JoinAll] ContainerContentScreenNode screenNode)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(containerContentItem.simpleContainerContentItem.MarketItemId);
            if (base.Select<SingleNode<UserItemComponent>>(entity, typeof(MarketItemGroupComponent)).Count > 0)
            {
                this.MarkItem(containerContentItem.garageListItem.gameObject, true);
            }
        }

        public class ContainerContentItemNode : Node
        {
            public SimpleContainerContentItemComponent simpleContainerContentItem;
            public ContainerContentItemComponent containerContentItem;
            public GarageListItemComponent garageListItem;
        }

        public class ContainerContentScreenNode : Node
        {
            public ContainerContentScreenComponent containerContentScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class ItemsListNode : Node
        {
            public ItemsListForViewComponent itemsListForView;
            public ScreenGroupComponent screenGroup;
        }
    }
}

