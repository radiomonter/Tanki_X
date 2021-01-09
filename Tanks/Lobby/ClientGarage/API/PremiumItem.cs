namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PremiumItem : VisualItem
    {
        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem ParentItem { get; private set; }

        public override Entity MarketItem
        {
            get => 
                base.MarketItem;
            set
            {
                base.MarketItem = value;
                base.Preview = value.GetComponent<CardImageItemComponent>().SpriteUid;
                base.Type = VisualItem.VisualItemType.Other;
            }
        }
    }
}

