namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class ContainerSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateContainerContentItems(NodeAddedEvent e, ContainerItemWithGroupNode container, [JoinByContainer] ICollection<SingleNode<ContainerContentItemComponent>> containerContentItems)
        {
            if (containerContentItems.Count <= 0)
            {
                this.CreateContainerContentItems(container.itemsContainerItem.Items, container, false);
                if ((container.itemsContainerItem.RareItems != null) && (container.itemsContainerItem.RareItems.Count > 0))
                {
                    this.CreateContainerContentItems(container.itemsContainerItem.RareItems, container, true);
                }
            }
        }

        private void CreateContainerContentItems(List<ContainerItem> containerItems, ContainerItemWithGroupNode container, bool isRare)
        {
            int num = 0;
            if (isRare)
            {
                num = 100;
            }
            foreach (ContainerItem item in containerItems)
            {
                Entity entity = base.CreateEntity("ContainerContentItem");
                entity.AddComponent<ContainerContentItemComponent>();
                OrderItemComponent component = new OrderItemComponent {
                    Index = ++num
                };
                entity.AddComponent(component);
                if (isRare)
                {
                    entity.AddComponent<RareContainerContentItemComponent>();
                }
                DescriptionBundleItemComponent component3 = new DescriptionBundleItemComponent {
                    Names = container.descriptionBundleItem.Names
                };
                entity.AddComponent(component3);
                if (item.ItemBundles.Count == 1)
                {
                    SimpleContainerContentItemComponent component5 = new SimpleContainerContentItemComponent {
                        MarketItemId = item.ItemBundles[0].MarketItem,
                        NameLokalizationKey = item.NameLocalizationKey
                    };
                    entity.AddComponent(component5);
                }
                else
                {
                    BundleContainerContentItemComponent component7 = new BundleContainerContentItemComponent {
                        MarketItems = item.ItemBundles,
                        NameLokalizationKey = item.NameLocalizationKey
                    };
                    entity.AddComponent(component7);
                }
                entity.CreateGroup<ContainerContentItemGroupComponent>();
                container.containerGroup.Attach(entity);
            }
        }

        [OnEventFire]
        public void CreateContainerGroup(NodeAddedEvent e, ContainerMarketItemNode container)
        {
            container.Entity.CreateGroup<ContainerGroupComponent>();
        }

        [OnEventFire]
        public void JoinContainerItem(JoinContainerItemEvent e, SimpleContainerContentItemNode containerContentItemNode, [JoinByContainer] ContainerMarketItemWithGroupNode container)
        {
            e.ContainerEntity = container.Entity;
        }

        [OnEventFire]
        public void JoinMarketItem(NodeAddedEvent e, [Combine] SingleNode<MarketItemComponent> marketItemNode, [Combine] SimpleContainerContentItemNode containerContentItemNode, [JoinByContainer] ContainerMarketItemWithGroupNode container)
        {
            if (containerContentItemNode.simpleContainerContentItem.MarketItemId.Equals(marketItemNode.Entity.Id))
            {
                if (!marketItemNode.Entity.HasComponent<ContainerContentItemGroupComponent>())
                {
                    containerContentItemNode.containerContentItemGroup.Attach(marketItemNode.Entity);
                }
                else
                {
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(marketItemNode.Entity.GetComponent<ContainerContentItemGroupComponent>().Key);
                    JoinContainerItemEvent eventInstance = new JoinContainerItemEvent();
                    base.ScheduleEvent(eventInstance, entity);
                    if (container.itemsContainerItem.Items.Count < eventInstance.ContainerEntity.GetComponent<ItemsContainerItemComponent>().Items.Count)
                    {
                        entity.GetComponent<ContainerContentItemGroupComponent>().Detach(marketItemNode.Entity);
                        containerContentItemNode.containerContentItemGroup.Attach(marketItemNode.Entity);
                    }
                }
            }
        }

        [OnEventFire]
        public void JoinUserContainerToMarketContainer(NodeAddedEvent e, ContainerUserItemNode userContainer, [JoinByMarketItem] Optional<ContainerMarketItemWithGroupNode> marketContainer)
        {
            if (marketContainer.IsPresent())
            {
                marketContainer.Get().containerGroup.Attach(userContainer.Entity);
            }
            else
            {
                userContainer.Entity.CreateGroup<ContainerGroupComponent>();
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class ContainerItemNode : Node
        {
            public ItemsContainerItemComponent itemsContainerItem;
            public DescriptionItemComponent descriptionItem;
            public DescriptionBundleItemComponent descriptionBundleItem;
            public MarketItemGroupComponent marketItemGroup;
            public ImageItemComponent imageItem;
        }

        public class ContainerItemWithGroupNode : ContainerSystem.ContainerItemNode
        {
            public ContainerGroupComponent containerGroup;
        }

        public class ContainerMarketItemNode : ContainerSystem.ContainerItemNode
        {
            public MarketItemComponent marketItem;
        }

        public class ContainerMarketItemWithGroupNode : ContainerSystem.ContainerMarketItemNode
        {
            public ContainerGroupComponent containerGroup;
        }

        public class ContainerUserItemNode : ContainerSystem.ContainerItemNode
        {
            public UserItemComponent userItem;
        }

        public class JoinContainerItemEvent : Event
        {
            public Entity ContainerEntity { get; set; }
        }

        public class SimpleContainerContentItemNode : Node
        {
            public ContainerContentItemComponent containerContentItem;
            public SimpleContainerContentItemComponent simpleContainerContentItem;
            public ContainerContentItemGroupComponent containerContentItemGroup;
            public ContainerGroupComponent containerGroup;
        }
    }
}

