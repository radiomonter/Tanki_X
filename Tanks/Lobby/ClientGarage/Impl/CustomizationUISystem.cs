namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class CustomizationUISystem : ECSSystem
    {
        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SingleNode<CustomizationUIComponent> ui, HullNode hull, [Context, JoinByMarketItem] MarketItemNode marketItem)
        {
            ui.component.Hull = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SingleNode<CustomizationUIComponent> ui, TurretNode turret, [Context, JoinByMarketItem] MarketItemNode marketItem)
        {
            ui.component.Turret = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class HullNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
            public TankItemComponent tankItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
        }

        public class TurretNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
            public WeaponItemComponent weaponItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

