namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class GarageNavigationSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<ItemNode, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ItemNode, bool> <>f__am$cache1;

        private IEnumerable<ItemNode> FilterByParentItem(IEnumerable<ItemNode> itemsByCategory, GarageCategory garageCategory, Entity parentItem)
        {
            <FilterByParentItem>c__AnonStorey0 storey = new <FilterByParentItem>c__AnonStorey0 {
                parentItem = parentItem,
                $this = this
            };
            return (!ReferenceEquals(garageCategory, GarageCategory.MODULES) ? itemsByCategory.Where<ItemNode>(new Func<ItemNode, bool>(storey.<>m__0)) : Enumerable.Empty<ItemNode>());
        }

        private Entity FindMountedTank(ICollection<ItemNode> items)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = item => item.Entity.HasComponent<MountedItemComponent>() && item.Entity.HasComponent<TankItemComponent>();
            }
            ItemNode node = items.FirstOrDefault<ItemNode>(<>f__am$cache1);
            if (node == null)
            {
                throw new Exception("Mounted tank not found");
            }
            return node.Entity;
        }

        private Entity FindMountedWeapon(ICollection<ItemNode> items)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = item => item.Entity.HasComponent<MountedItemComponent>() && item.Entity.HasComponent<WeaponItemComponent>();
            }
            ItemNode node = items.FirstOrDefault<ItemNode>(<>f__am$cache0);
            if (node == null)
            {
                throw new Exception("Mounted weapon not found");
            }
            return node.Entity;
        }

        private GarageCategory GetCategoryByItem(Entity item)
        {
            for (int i = 0; i < GarageCategory.Values.Length; i++)
            {
                if (item.HasComponent(GarageCategory.Values[i].ItemMarkerComponentType))
                {
                    return GarageCategory.Values[i];
                }
            }
            throw new Exception("Category by item not found: " + item);
        }

        private Entity GetParentItem(Entity item, GarageCategory category)
        {
            if (!category.NeedParent)
            {
                return null;
            }
            long key = item.GetComponent<ParentGroupComponent>().Key;
            return EngineService.EntityRegistry.GetEntity(key);
        }

        private bool IsParentAndChild(Entity parent, Entity item) => 
            item.GetComponent<ParentGroupComponent>().Key == parent.Id;

        public GarageCategory ParseCategory(string str)
        {
            for (int i = 0; i < GarageCategory.Values.Length; i++)
            {
                if (GarageCategory.Values[i].LinkPart.Equals(str))
                {
                    return GarageCategory.Values[i];
                }
            }
            return null;
        }

        [OnEventFire]
        public void ParseCategoryLink(ParseLinkEvent e, Node node)
        {
            if (e.Link.StartsWith("garage/"))
            {
                string str = e.Link.Substring("garage/".Length);
                GarageCategory category = this.ParseCategory(str);
                if (category == null)
                {
                    e.ParseMessage = "Category not found: " + str;
                }
                else
                {
                    ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                        Category = category
                    };
                    e.CustomNavigationEvent = base.NewEvent(eventInstance).Attach(node);
                }
            }
        }

        [OnEventFire]
        public void ParseItemLink(ParseLinkEvent e, Node node)
        {
            long num;
            if (e.Link.StartsWith("item/") && long.TryParse(e.Link.Substring("item/".Length), out num))
            {
                if (!EngineService.EntityRegistry.ContainsEntity(num))
                {
                    e.ParseMessage = "Entity not found: " + num;
                }
                else
                {
                    Entity entity = EngineService.EntityRegistry.GetEntity(num);
                    if (!entity.HasComponent<MarketItemComponent>())
                    {
                        e.ParseMessage = "Entity is not market item: " + entity;
                    }
                    else
                    {
                        ShowGarageItemEvent eventInstance = new ShowGarageItemEvent {
                            Item = entity
                        };
                        e.CustomNavigationEvent = base.NewEvent(eventInstance).Attach(node);
                    }
                }
            }
        }

        [OnEventFire]
        public void ShowGarageCategory(ShowGarageCategoryEvent e, Node node, [JoinAll] ICollection<ItemNode> items)
        {
            base.Log.InfoFormat("ShowGarageCategory {0} ParentItem={1} SelectedItem={2}", e.Category, e.ParentItem, e.SelectedItem);
            MainScreenComponent instance = MainScreenComponent.Instance;
            if (!instance.gameObject.activeSelf)
            {
                instance.ShowHome();
            }
            CustomizationUIComponent componentInChildrenIncludeInactive = instance.GetComponentInChildrenIncludeInactive<CustomizationUIComponent>();
            if ((e.ParentItem == null) && e.Category.NeedParent)
            {
                e.ParentItem = !ReferenceEquals(e.Category, GarageCategory.MODULES) ? instance.MountedTurret.UserItem : instance.MountedHull.UserItem;
            }
            if (ReferenceEquals(e.Category, GarageCategory.CONTAINERS) || ReferenceEquals(e.Category, GarageCategory.BLUEPRINTS))
            {
                base.ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                if (!e.SelectedItem.HasComponent<HangarItemPreviewComponent>())
                {
                    e.SelectedItem.AddComponent<HangarItemPreviewComponent>();
                }
                instance.ShowShopIfNotVisible();
                int newIndex = !ReferenceEquals(e.Category, GarageCategory.BLUEPRINTS) ? 2 : 1;
                if (ShopTabManager.Instance == null)
                {
                    ShopTabManager.shopTabIndex = newIndex;
                }
                else
                {
                    ShopTabManager.Instance.Show(newIndex);
                }
            }
            else if (ReferenceEquals(e.Category, GarageCategory.GRAFFITI))
            {
                base.ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                componentInChildrenIncludeInactive.HullVisual(3);
            }
            else if (ReferenceEquals(e.Category, GarageCategory.HULLS))
            {
                instance.ShowHulls(GarageItemsRegistry.GetItem<TankPartItem>(e.SelectedItem));
            }
            else if (ReferenceEquals(e.Category, GarageCategory.WEAPONS))
            {
                instance.ShowTurrets(GarageItemsRegistry.GetItem<TankPartItem>(e.SelectedItem));
            }
            else if (ReferenceEquals(e.Category, GarageCategory.MODULES))
            {
                componentInChildrenIncludeInactive.HullModules();
            }
            else if (ReferenceEquals(e.Category, GarageCategory.SHELLS))
            {
                base.ScheduleEvent<ListItemSelectedEvent>(e.ParentItem);
                base.ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                componentInChildrenIncludeInactive.TurretVisualNoSwitch(4);
            }
            else if (ReferenceEquals(e.Category, GarageCategory.PAINTS))
            {
                base.ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                if (e.SelectedItem.HasComponent<TankPaintItemComponent>())
                {
                    componentInChildrenIncludeInactive.HullVisualNoSwitch(1);
                }
                else
                {
                    componentInChildrenIncludeInactive.TurretVisualNoSwitch(2);
                }
            }
            else if (ReferenceEquals(e.Category, GarageCategory.SKINS))
            {
                base.ScheduleEvent<ListItemSelectedEvent>(e.ParentItem);
                base.ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                if (e.ParentItem.HasComponent<TankItemComponent>())
                {
                    componentInChildrenIncludeInactive.HullVisualNoSwitch(0);
                }
                else
                {
                    componentInChildrenIncludeInactive.TurretVisualNoSwitch(0);
                }
            }
        }

        [OnEventFire]
        public void ShowGarageItem(ShowGarageItemEvent e, Node node)
        {
            GarageCategory categoryByItem = this.GetCategoryByItem(e.Item);
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = categoryByItem,
                ParentItem = this.GetParentItem(e.Item, categoryByItem),
                SelectedItem = e.Item
            };
            base.ScheduleEvent(eventInstance, node);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <FilterByParentItem>c__AnonStorey0
        {
            internal Entity parentItem;
            internal GarageNavigationSystem $this;

            internal bool <>m__0(GarageNavigationSystem.ItemNode item) => 
                this.$this.IsParentAndChild(this.parentItem, item.Entity);
        }

        public class ItemNode : Node
        {
            public GarageItemComponent garageItem;
        }
    }
}

