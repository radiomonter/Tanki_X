namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class PresetUISystem : ECSSystem
    {
        private void Add<T>(List<T> list, Entity marketItem, string preview) where T: GarageItem, new()
        {
            T item = GarageItemsRegistry.GetItem<T>(marketItem.Id);
            item.Preview = preview;
            list.Add(item);
        }

        [OnEventFire]
        public void GetGraffities(GetAllGraffitiesEvent e, Node any, [JoinAll, Combine] MarketGraffitiNode graffiti)
        {
            this.Add<VisualItem>(e.Items, graffiti.Entity, graffiti.imageItem.SpriteUid);
        }

        [OnEventFire]
        public void GetShells(GetAllShellsEvent e, GarageMarketItemNode marketItem, [JoinByParentGroup, Combine] ShellMarketItemNode shell)
        {
            this.Add<VisualItem>(e.Items, shell.Entity, shell.imageItem.SpriteUid);
        }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SingleNode<MainScreenComponent> ui, SelfUserNode self, [JoinByUser, Context] MountedHullNode hull, [JoinByMarketItem, Context] GarageMarketItemNode marketItem)
        {
            ui.component.MountedHull = GarageItemsRegistry.GetItem<TankPartItem>(hull.marketItemGroup.Key);
        }

        [OnEventComplete]
        public void InitHull(NodeAddedEvent e, SingleNode<SelectedHullUIComponent> hullUI, SelfUserNode self, [Context] PresetNode preset, [JoinByUser, Context] MountedHullNode hull, [JoinByMarketItem, Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup, Context, Combine] MountedSkin mountedSkin)
        {
            if (preset.userGroup.Key == mountedSkin.userGroup.Key)
            {
                this.SetItem(hull, hullUI.component, mountedSkin);
            }
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SingleNode<MainScreenComponent> ui, SelfUserNode self, [JoinByUser, Context] MountedTurretNode turret, [JoinByMarketItem, Context] GarageMarketItemNode marketItem)
        {
            ui.component.MountedTurret = GarageItemsRegistry.GetItem<TankPartItem>(turret.marketItemGroup.Key);
        }

        [OnEventComplete]
        public void InitTurret(NodeAddedEvent e, SingleNode<SelectedTurretUIComponent> turretUI, SelfUserNode self, [Context] PresetNode preset, [JoinByUser, Context] MountedTurretNode turret, [JoinByMarketItem, Context] ParentGarageMarketItemNode marketItem, [JoinByParentGroup, Context, Combine] MountedSkin mountedSkin)
        {
            if (preset.userGroup.Key == mountedSkin.userGroup.Key)
            {
                this.SetItem(turret, turretUI.component, mountedSkin);
            }
        }

        [OnEventFire]
        public void LocalizeModuleProperties(NodeAddedEvent e, [Combine] SingleNode<ModuleVisualPropertiesComponent> props, SingleNode<LocalizedVisualPropertiesComponent> locale)
        {
            Dictionary<string, string> names = locale.component.Names;
            Dictionary<string, string> units = locale.component.Units;
            foreach (ModuleVisualProperty property in props.component.Properties)
            {
                if (names.ContainsKey(property.Name))
                {
                    property.Name = names[property.Name];
                }
                if ((property.Unit != null) && units.ContainsKey(property.Unit))
                {
                    property.Unit = units[property.Unit];
                }
            }
        }

        [OnEventFire]
        public void LocalizeProperties(NodeAddedEvent e, [Combine] SingleNode<VisualPropertiesComponent> props, SingleNode<LocalizedVisualPropertiesComponent> locale)
        {
            Dictionary<string, string> names = locale.component.Names;
            Dictionary<string, string> units = locale.component.Units;
            foreach (MainVisualProperty property in props.component.MainProperties)
            {
                if (names.ContainsKey(property.Name))
                {
                    property.Name = names[property.Name];
                }
            }
            foreach (VisualProperty property2 in props.component.Properties)
            {
                if (names.ContainsKey(property2.Name))
                {
                    property2.Name = names[property2.Name];
                }
                if ((property2.Unit != null) && units.ContainsKey(property2.Unit))
                {
                    property2.Unit = units[property2.Unit];
                }
            }
        }

        private void SetItem(GarageUserItemNode item, SelectedItemUI ui, MountedSkin mountedSkin)
        {
            ui.Set(GarageItemsRegistry.GetItem<TankPartItem>(item.marketItemGroup.Key), mountedSkin.descriptionItem.Name, item.upgradeLevelItem.Level);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class GarageMarketItemNode : Node
        {
            public DescriptionItemComponent descriptionItem;
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
            public VisualPropertiesComponent visualProperties;
            public GarageMarketItemRegisteredComponent garageMarketItemRegistered;
        }

        public class GarageUserItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public DescriptionItemComponent descriptionItem;
            public UserGroupComponent userGroup;
            public ExperienceItemComponent experienceItem;
            public VisualPropertiesComponent visualProperties;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class MarketGraffitiNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
            public GraffitiItemComponent graffitiItem;
            public ImageItemComponent imageItem;
        }

        public class MountedHullNode : PresetUISystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;
        }

        public class MountedSkin : Node
        {
            public SkinItemComponent skinItem;
            public MountedItemComponent mountedItem;
            public ParentGroupComponent parentGroup;
            public DescriptionItemComponent descriptionItem;
            public UserGroupComponent userGroup;
        }

        public class MountedTurretNode : PresetUISystem.GarageUserItemNode
        {
            public MountedItemComponent mountedItem;
            public WeaponItemComponent weaponItem;
        }

        public class ParentGarageMarketItemNode : PresetUISystem.GarageMarketItemNode
        {
            public ParentGroupComponent parentGroup;
        }

        public class PresetNode : Node
        {
            public PresetItemComponent presetItem;
            public SelectedPresetComponent selectedPreset;
            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class ShellMarketItemNode : Node
        {
            public ShellItemComponent shellItem;
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
            public ParentGroupComponent parentGroup;
            public ImageItemComponent imageItem;
        }
    }
}

