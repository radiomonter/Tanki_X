namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class VisualItem : GarageItem
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
                base.Preview = value.GetComponent<ImageItemComponent>().SpriteUid;
                if (value.HasComponent<SkinItemComponent>())
                {
                    this.Type = VisualItemType.Skin;
                }
                else if (value.HasComponent<GraffitiItemComponent>())
                {
                    this.Type = VisualItemType.Graffiti;
                }
                else if (value.HasComponent<TankPaintItemComponent>())
                {
                    this.Type = VisualItemType.Paint;
                }
                else if (value.HasComponent<WeaponPaintItemComponent>())
                {
                    this.Type = VisualItemType.Coating;
                }
                else if (value.HasComponent<ShellItemComponent>())
                {
                    this.Type = VisualItemType.Shell;
                }
                else if (value.HasComponent<AvatarItemComponent>())
                {
                    this.Type = VisualItemType.Avatar;
                }
                else
                {
                    this.Type = VisualItemType.Other;
                    base.Preview = value.GetComponent<CardImageItemComponent>().SpriteUid;
                }
                if (this.MarketItem.HasComponent<ParentGroupComponent>())
                {
                    this.ParentItem = GarageItemsRegistry.GetItem<TankPartItem>(this.MarketItem.GetComponent<ParentGroupComponent>().Key);
                }
            }
        }

        public VisualItemType Type { get; protected set; }

        public enum VisualItemType
        {
            Skin,
            Paint,
            Coating,
            Graffiti,
            Shell,
            Other,
            Avatar
        }
    }
}

