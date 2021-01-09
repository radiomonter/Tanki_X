namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;

    public class GarageItemsRegistrySystem : ECSSystem, GarageItemsRegistry
    {
        private readonly Dictionary<long, GarageItem> cache = new Dictionary<long, GarageItem>();
        private readonly HashSet<TankPartItem> hulls = new HashSet<TankPartItem>();
        private readonly HashSet<TankPartItem> turrets = new HashSet<TankPartItem>();
        private readonly HashSet<VisualItem> paints = new HashSet<VisualItem>();
        private readonly HashSet<VisualItem> coatings = new HashSet<VisualItem>();
        private readonly HashSet<ContainerBoxItem> containers = new HashSet<ContainerBoxItem>();
        private readonly HashSet<DetailItem> details = new HashSet<DetailItem>();
        private readonly HashSet<ModuleItem> modules = new HashSet<ModuleItem>();
        private readonly HashSet<PremiumItem> premiumQuests = new HashSet<PremiumItem>();
        private readonly HashSet<PremiumItem> premiumBoost = new HashSet<PremiumItem>();
        private readonly HashSet<Avatar> avatars = new HashSet<Avatar>();
        [CompilerGenerated]
        private static Action<ContainerBoxItem> <>f__am$cache0;

        private T AddOrUpdate<T>(long id, Action<T> update, HashSet<T> set = null) where T: GarageItem, new()
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(id);
            T item = this.GetItem<T>(id);
            if (item == null)
            {
                return null;
            }
            if (set != null)
            {
                set.Add(item);
            }
            update(item);
            entity.AddComponentIfAbsent<GarageMarketItemRegisteredComponent>();
            return item;
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<HullSkinBattleItemComponent> item)
        {
            this.CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<TankPaintBattleItemComponent> item)
        {
            this.CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<WeaponPaintBattleItemComponent> item)
        {
            this.CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<WeaponSkinBattleItemComponent> item)
        {
            this.CacheBattleItem(item);
        }

        private void CacheBattleItem(BattleItemNode item)
        {
            this.GetItem<GarageItem>(item.marketItemGroup.Key).ConfigPath = ((EntityImpl) item.Entity).TemplateAccessor.Get().ConfigPath;
        }

        public T GetItem<T>(Entity marketEntity) where T: GarageItem, new() => 
            this.GetItem<T>(marketEntity.Id);

        public T GetItem<T>(long marketId) where T: GarageItem, new()
        {
            if (!this.cache.ContainsKey(marketId))
            {
                this.cache.Add(marketId, Activator.CreateInstance<T>());
            }
            return (this.cache[marketId] as T);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<AvatarItemComponent> item)
        {
            <ItemCreated>c__AnonStorey0 storey = new <ItemCreated>c__AnonStorey0 {
                item = item
            };
            this.AddOrUpdate<Avatar>(storey.item.Entity.Id, new Action<Avatar>(storey.<>m__0), this.avatars);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ContainerMarkerComponent> item)
        {
            <ItemCreated>c__AnonStorey4 storey = new <ItemCreated>c__AnonStorey4 {
                item = item
            };
            this.AddOrUpdate<ContainerBoxItem>(storey.item.Entity.Id, new Action<ContainerBoxItem>(storey.<>m__0), this.containers);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<DetailItemComponent> item)
        {
            <ItemCreated>c__AnonStorey16 storey = new <ItemCreated>c__AnonStorey16 {
                item = item
            };
            this.AddOrUpdate<DetailItem>(storey.item.Entity.Id, new Action<DetailItem>(storey.<>m__0), this.details);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<GoldBonusItemComponent> item)
        {
            <ItemCreated>c__AnonStorey18 storey = new <ItemCreated>c__AnonStorey18 {
                item = item
            };
            this.AddOrUpdate<GarageItem>(storey.item.Entity.Id, new Action<GarageItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<GraffitiItemComponent> item)
        {
            <ItemCreated>c__AnonStorey11 storey = new <ItemCreated>c__AnonStorey11 {
                item = item
            };
            this.AddOrUpdate<VisualItem>(storey.item.Entity.Id, new Action<VisualItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PremiumBoostItemComponent> item)
        {
            <ItemCreated>c__AnonStorey2 storey = new <ItemCreated>c__AnonStorey2 {
                item = item
            };
            this.AddOrUpdate<PremiumItem>(storey.item.Entity.Id, new Action<PremiumItem>(storey.<>m__0), this.premiumBoost);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PremiumQuestItemComponent> item)
        {
            <ItemCreated>c__AnonStorey3 storey = new <ItemCreated>c__AnonStorey3 {
                item = item
            };
            this.AddOrUpdate<PremiumItem>(storey.item.Entity.Id, new Action<PremiumItem>(storey.<>m__0), this.premiumQuests);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PresetItemComponent> item)
        {
            <ItemCreated>c__AnonStorey15 storey = new <ItemCreated>c__AnonStorey15 {
                item = item
            };
            this.AddOrUpdate<GarageItem>(storey.item.Entity.Id, new Action<GarageItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ShellItemComponent> item)
        {
            <ItemCreated>c__AnonStorey13 storey = new <ItemCreated>c__AnonStorey13 {
                item = item
            };
            this.AddOrUpdate<VisualItem>(storey.item.Entity.Id, new Action<VisualItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<TankPaintItemComponent> item)
        {
            <ItemCreated>c__AnonStoreyD yd = new <ItemCreated>c__AnonStoreyD {
                item = item
            };
            this.AddOrUpdate<VisualItem>(yd.item.Entity.Id, new Action<VisualItem>(yd.<>m__0), this.paints);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<WeaponPaintItemComponent> item)
        {
            <ItemCreated>c__AnonStoreyF yf = new <ItemCreated>c__AnonStoreyF {
                item = item
            };
            this.AddOrUpdate<VisualItem>(yf.item.Entity.Id, new Action<VisualItem>(yf.<>m__0), this.coatings);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<SkinItemComponent> item)
        {
            <ItemCreated>c__AnonStoreyB yb = new <ItemCreated>c__AnonStoreyB {
                item = item
            };
            TankPartItem item2 = this.GetItem<TankPartItem>(yb.item.parentGroup.Key);
            VisualItem item3 = this.AddOrUpdate<VisualItem>(yb.item.Entity.Id, new Action<VisualItem>(yb.<>m__0), null);
            if (item3 == null)
            {
                Console.WriteLine("GarageItemsRegistrySystem.ItemCreated error. Skin created error with id: " + yb.item.Entity.Id);
            }
            item2.Skins.Add(item3);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<AvatarItemComponent> item)
        {
            <ItemCreated>c__AnonStorey1 storey = new <ItemCreated>c__AnonStorey1 {
                item = item
            };
            this.AddOrUpdate<Avatar>(storey.item.marketItemGroup.Key, new Action<Avatar>(storey.<>m__0), this.avatars);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<DetailItemComponent> item)
        {
            <ItemCreated>c__AnonStorey17 storey = new <ItemCreated>c__AnonStorey17 {
                item = item
            };
            this.AddOrUpdate<DetailItem>(storey.item.marketItemGroup.Key, new Action<DetailItem>(storey.<>m__0), this.details);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<GraffitiItemComponent> item)
        {
            <ItemCreated>c__AnonStorey12 storey = new <ItemCreated>c__AnonStorey12 {
                item = item
            };
            this.AddOrUpdate<VisualItem>(storey.item.marketItemGroup.Key, new Action<VisualItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<ModuleItemComponent> item)
        {
            <ItemCreated>c__AnonStorey1C storeyc = new <ItemCreated>c__AnonStorey1C {
                item = item
            };
            this.AddOrUpdate<ModuleItem>(storeyc.item.marketItemGroup.Key, new Action<ModuleItem>(storeyc.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<ShellItemComponent> item)
        {
            <ItemCreated>c__AnonStorey14 storey = new <ItemCreated>c__AnonStorey14 {
                item = item
            };
            this.AddOrUpdate<VisualItem>(storey.item.marketItemGroup.Key, new Action<VisualItem>(storey.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<SkinItemComponent> item)
        {
            <ItemCreated>c__AnonStoreyC yc = new <ItemCreated>c__AnonStoreyC {
                item = item
            };
            this.AddOrUpdate<VisualItem>(yc.item.marketItemGroup.Key, new Action<VisualItem>(yc.<>m__0), null);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<TankPaintItemComponent> item)
        {
            <ItemCreated>c__AnonStoreyE ye = new <ItemCreated>c__AnonStoreyE {
                item = item
            };
            this.AddOrUpdate<VisualItem>(ye.item.marketItemGroup.Key, new Action<VisualItem>(ye.<>m__0), this.paints);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<WeaponPaintItemComponent> item)
        {
            <ItemCreated>c__AnonStorey10 storey = new <ItemCreated>c__AnonStorey10 {
                item = item
            };
            this.AddOrUpdate<VisualItem>(storey.item.marketItemGroup.Key, new Action<VisualItem>(storey.<>m__0), this.coatings);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context, JoinByParentGroup] Optional<MarketItemNode<ModuleCardItemComponent>> card)
        {
            <ItemCreated>c__AnonStorey1A storeya = new <ItemCreated>c__AnonStorey1A {
                item = item,
                card = card
            };
            this.AddOrUpdate<ModuleItem>(storeya.item.Entity.Id, new Action<ModuleItem>(storeya.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context, JoinByParentGroup] Optional<UserItemNode<ModuleCardItemComponent>> card)
        {
            <ItemCreated>c__AnonStorey19 storey = new <ItemCreated>c__AnonStorey19 {
                item = item,
                card = card
            };
            this.AddOrUpdate<ModuleItem>(storey.item.Entity.Id, new Action<ModuleItem>(storey.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context, JoinByMarketItem] ModuleEffectUpgradablePropertyNode property)
        {
            <ItemCreated>c__AnonStorey1B storeyb = new <ItemCreated>c__AnonStorey1B {
                item = item,
                property = property
            };
            this.AddOrUpdate<ModuleItem>(storeyb.item.Entity.Id, new Action<ModuleItem>(storeyb.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<TankItemComponent> item, [Context, JoinByParentGroup] DefaultSkinNode skin)
        {
            <ItemCreated>c__AnonStorey7 storey = new <ItemCreated>c__AnonStorey7 {
                item = item,
                skin = skin
            };
            this.AddOrUpdate<TankPartItem>(storey.item.Entity.Id, new Action<TankPartItem>(storey.<>m__0), this.hulls);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<WeaponItemComponent> item, [Context, JoinByParentGroup] DefaultSkinNode skin)
        {
            <ItemCreated>c__AnonStorey9 storey = new <ItemCreated>c__AnonStorey9 {
                item = item,
                skin = skin
            };
            this.AddOrUpdate<TankPartItem>(storey.item.Entity.Id, new Action<TankPartItem>(storey.<>m__0), this.turrets);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, UserItemNode<ModuleItemComponent> item, [Context, JoinByModule] SlotNode slot)
        {
            <ItemCreated>c__AnonStorey1D storeyd = new <ItemCreated>c__AnonStorey1D {
                item = item,
                slot = slot
            };
            this.AddOrUpdate<ModuleItem>(storeyd.item.marketItemGroup.Key, new Action<ModuleItem>(storeyd.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeRemoveEvent e, SlotNode slot, [Context, JoinByModule] UserItemNode<ModuleItemComponent> item)
        {
            <ItemCreated>c__AnonStorey1E storeye = new <ItemCreated>c__AnonStorey1E {
                item = item
            };
            this.AddOrUpdate<ModuleItem>(storeye.item.marketItemGroup.Key, new Action<ModuleItem>(storeye.<>m__0), this.modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<ContainerMarkerComponent> item, [JoinByUser, Context] UserNode self)
        {
            <ItemCreated>c__AnonStorey5 storey = new <ItemCreated>c__AnonStorey5 {
                item = item
            };
            this.AddOrUpdate<ContainerBoxItem>(storey.item.marketItemGroup.Key, new Action<ContainerBoxItem>(storey.<>m__0), this.containers);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<TankItemComponent> item, [JoinByUser, Context] UserNode self)
        {
            <ItemCreated>c__AnonStorey8 storey = new <ItemCreated>c__AnonStorey8 {
                item = item
            };
            this.AddOrUpdate<TankPartItem>(storey.item.marketItemGroup.Key, new Action<TankPartItem>(storey.<>m__0), this.hulls);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context, JoinByMarketItem, Combine] UserItemNode<WeaponItemComponent> item, [JoinByUser, Context] UserNode self)
        {
            <ItemCreated>c__AnonStoreyA ya = new <ItemCreated>c__AnonStoreyA {
                item = item
            };
            this.AddOrUpdate<TankPartItem>(ya.item.marketItemGroup.Key, new Action<TankPartItem>(ya.<>m__0), this.turrets);
        }

        [OnEventFire]
        public void ItemCreated(ItemsCountChangedEvent e, UserItemNode<ContainerMarkerComponent> item, [JoinByUser] UserNode self, UserItemNode<ContainerMarkerComponent> item1, [JoinByMarketItem] BaseMarketItemNode marketItem)
        {
            <ItemCreated>c__AnonStorey6 storey = new <ItemCreated>c__AnonStorey6 {
                item = item
            };
            if (e.Delta > 0L)
            {
                this.AddOrUpdate<ContainerBoxItem>(storey.item.marketItemGroup.Key, new Action<ContainerBoxItem>(storey.<>m__0), null);
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = containerItem => containerItem.Opend();
                }
                this.AddOrUpdate<ContainerBoxItem>(storey.item.marketItemGroup.Key, <>f__am$cache0, null);
            }
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<ItemsContainerItemComponent> itemNode)
        {
            ContainerBoxItem item = this.GetItem<ContainerBoxItem>(itemNode.marketItemGroup.Key);
            this.containers.Remove(item);
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<TankItemComponent> itemNode)
        {
            TankPartItem item = this.GetItem<TankPartItem>(itemNode.marketItemGroup.Key);
            this.hulls.Remove(item);
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<WeaponItemComponent> itemNode)
        {
            TankPartItem item = this.GetItem<TankPartItem>(itemNode.marketItemGroup.Key);
            this.turrets.Remove(item);
        }

        [OnEventFire]
        public void SendMounted(NodeAddedEvent e, UserItemNode<MountedItemComponent> item)
        {
            if (this.cache.ContainsKey(item.marketItemGroup.Key))
            {
                this.cache[item.marketItemGroup.Key].Mounted();
            }
        }

        public ICollection<TankPartItem> Hulls =>
            this.hulls;

        public ICollection<TankPartItem> Turrets =>
            this.turrets;

        public ICollection<VisualItem> Paints =>
            this.paints;

        public ICollection<VisualItem> Coatings =>
            this.coatings;

        public ICollection<ContainerBoxItem> Containers =>
            this.containers;

        public ICollection<DetailItem> Details =>
            this.details;

        public ICollection<ModuleItem> Modules =>
            this.modules;

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey0
        {
            internal GarageItemsRegistrySystem.MarketItemNode<AvatarItemComponent> item;

            internal void <>m__0(Avatar avatarItem)
            {
                avatarItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1
        {
            internal GarageItemsRegistrySystem.UserItemNode<AvatarItemComponent> item;

            internal void <>m__0(Avatar avatarItem)
            {
                avatarItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey10
        {
            internal GarageItemsRegistrySystem.UserItemNode<WeaponPaintItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey11
        {
            internal GarageItemsRegistrySystem.MarketItemNode<GraffitiItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey12
        {
            internal GarageItemsRegistrySystem.UserItemNode<GraffitiItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey13
        {
            internal GarageItemsRegistrySystem.MarketItemNode<ShellItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey14
        {
            internal GarageItemsRegistrySystem.UserItemNode<ShellItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey15
        {
            internal GarageItemsRegistrySystem.MarketItemNode<PresetItemComponent> item;

            internal void <>m__0(GarageItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey16
        {
            internal GarageItemsRegistrySystem.MarketItemNode<DetailItemComponent> item;

            internal void <>m__0(DetailItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey17
        {
            internal GarageItemsRegistrySystem.UserItemNode<DetailItemComponent> item;

            internal void <>m__0(DetailItem garageItem)
            {
                garageItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey18
        {
            internal GarageItemsRegistrySystem.MarketItemNode<GoldBonusItemComponent> item;

            internal void <>m__0(GarageItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey19
        {
            internal GarageItemsRegistrySystem.MarketItemNode<ModuleItemComponent> item;
            internal Optional<GarageItemsRegistrySystem.UserItemNode<ModuleCardItemComponent>> card;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
                if (this.card.IsPresent())
                {
                    garageItem.CardItem = this.card.Get().Entity;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1A
        {
            internal GarageItemsRegistrySystem.MarketItemNode<ModuleItemComponent> item;
            internal Optional<GarageItemsRegistrySystem.MarketItemNode<ModuleCardItemComponent>> card;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
                if (this.card.IsPresent())
                {
                    garageItem.MarketCardItem = this.card.Get().Entity;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1B
        {
            internal GarageItemsRegistrySystem.MarketItemNode<ModuleItemComponent> item;
            internal GarageItemsRegistrySystem.ModuleEffectUpgradablePropertyNode property;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.MarketItem = this.item.Entity;
                garageItem.Property = this.property.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1C
        {
            internal GarageItemsRegistrySystem.UserItemNode<ModuleItemComponent> item;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1D
        {
            internal GarageItemsRegistrySystem.UserItemNode<ModuleItemComponent> item;
            internal GarageItemsRegistrySystem.SlotNode slot;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.UserItem = this.item.Entity;
                garageItem.Slot = this.slot.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey1E
        {
            internal GarageItemsRegistrySystem.UserItemNode<ModuleItemComponent> item;

            internal void <>m__0(ModuleItem garageItem)
            {
                garageItem.UserItem = this.item.Entity;
                garageItem.Slot = null;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey2
        {
            internal GarageItemsRegistrySystem.MarketItemNode<PremiumBoostItemComponent> item;

            internal void <>m__0(PremiumItem premiumBoostItem)
            {
                premiumBoostItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey3
        {
            internal GarageItemsRegistrySystem.MarketItemNode<PremiumQuestItemComponent> item;

            internal void <>m__0(PremiumItem premiumQuestItem)
            {
                premiumQuestItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey4
        {
            internal GarageItemsRegistrySystem.MarketItemNode<ContainerMarkerComponent> item;

            internal void <>m__0(ContainerBoxItem containerItem)
            {
                containerItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey5
        {
            internal GarageItemsRegistrySystem.UserItemNode<ContainerMarkerComponent> item;

            internal void <>m__0(ContainerBoxItem containerItem)
            {
                containerItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey6
        {
            internal GarageItemsRegistrySystem.UserItemNode<ContainerMarkerComponent> item;

            internal void <>m__0(ContainerBoxItem containerItem)
            {
                containerItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey7
        {
            internal GarageItemsRegistrySystem.MarketParentItemNode<TankItemComponent> item;
            internal GarageItemsRegistrySystem.DefaultSkinNode skin;

            internal void <>m__0(TankPartItem partItem)
            {
                partItem.MarketItem = this.item.Entity;
                partItem.Preview = this.skin.imageItem.SpriteUid;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey8
        {
            internal GarageItemsRegistrySystem.UserItemNode<TankItemComponent> item;

            internal void <>m__0(TankPartItem partItem)
            {
                partItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStorey9
        {
            internal GarageItemsRegistrySystem.MarketParentItemNode<WeaponItemComponent> item;
            internal GarageItemsRegistrySystem.DefaultSkinNode skin;

            internal void <>m__0(TankPartItem partItem)
            {
                partItem.MarketItem = this.item.Entity;
                partItem.Preview = this.skin.imageItem.SpriteUid;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyA
        {
            internal GarageItemsRegistrySystem.UserItemNode<WeaponItemComponent> item;

            internal void <>m__0(TankPartItem partItem)
            {
                partItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyB
        {
            internal GarageItemsRegistrySystem.MarketParentItemNode<SkinItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyC
        {
            internal GarageItemsRegistrySystem.UserItemNode<SkinItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyD
        {
            internal GarageItemsRegistrySystem.MarketItemNode<TankPaintItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.MarketItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyE
        {
            internal GarageItemsRegistrySystem.UserItemNode<TankPaintItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.UserItem = this.item.Entity;
            }
        }

        [CompilerGenerated]
        private sealed class <ItemCreated>c__AnonStoreyF
        {
            internal GarageItemsRegistrySystem.MarketItemNode<WeaponPaintItemComponent> item;

            internal void <>m__0(VisualItem visualItem)
            {
                visualItem.MarketItem = this.item.Entity;
            }
        }

        public class BaseMarketItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
        }

        public class BattleItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
        }

        public class BattleItemNode<T> : GarageItemsRegistrySystem.BattleItemNode
        {
            public T marker;
        }

        public class DefaultSkinNode : Node
        {
            public SkinItemComponent skinItem;
            public ImageItemComponent imageItem;
            public ParentGroupComponent parentGroup;
            public DefaultSkinItemComponent defaultSkinItem;
        }

        public class MarketItemNode<T> : GarageItemsRegistrySystem.BaseMarketItemNode
        {
            public T marker;
        }

        public class MarketParentItemNode<T> : GarageItemsRegistrySystem.MarketItemNode<T>
        {
            public ParentGroupComponent parentGroup;
        }

        public class ModuleEffectUpgradablePropertyNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public ModuleVisualPropertiesComponent moduleVisualProperties;
        }

        public class SlotNode : Node
        {
            public UserItemComponent userItem;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotTankPartComponent slotTankPart;
            public ModuleGroupComponent moduleGroup;
        }

        public class UserItemNode : Node
        {
            public SelfComponent self;
            public MarketItemGroupComponent marketItemGroup;
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
        }

        public class UserItemNode<T> : GarageItemsRegistrySystem.UserItemNode
        {
            public T marker;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }
    }
}

