namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;

    public class DetailItem : GarageItem
    {
        public override string ToString() => 
            $"Detail item: marketItem = {this.MarketItem}, TargetMarketItem = {this.TargetMarketItem.MarketItem}, Name = {base.Name}, Preview = {base.Preview}, Count = {this.Count}, RequiredCount = {this.RequiredCount}";

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public int Count =>
            (base.UserItem != null) ? (base.UserItem.HasComponent<UserItemCounterComponent>() ? ((int) base.UserItem.GetComponent<UserItemCounterComponent>().Count) : 0) : 0;

        public long RequiredCount =>
            this.MarketItem.GetComponent<DetailItemComponent>().RequiredCount;

        public override Entity MarketItem
        {
            get => 
                base.MarketItem;
            set
            {
                base.MarketItem = value;
                base.Preview = value.GetComponent<ImageItemComponent>().SpriteUid;
            }
        }

        public GarageItem TargetMarketItem =>
            GarageItemsRegistry.GetItem<GarageItem>(this.MarketItem.GetComponent<DetailItemComponent>().TargetMarketItemId);
    }
}

