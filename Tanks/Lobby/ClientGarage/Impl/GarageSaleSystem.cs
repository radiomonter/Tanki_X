namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    internal class GarageSaleSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<UserItemNode, bool> <>f__am$cache0;

        [OnEventFire]
        public void AddItemPreview(NodeAddedEvent e, NewsItemUIWithMarketItemGroupNode newsItem, [JoinByMarketItem] MarketItemNode marketItem)
        {
            NewsItem data = newsItem.newsItem.Data;
            if (string.IsNullOrEmpty(data.PreviewImageUrl) && string.IsNullOrEmpty(data.PreviewImageGuid))
            {
                string itemOrChildImage = this.GetItemOrChildImage(marketItem.Entity);
                if (itemOrChildImage != null)
                {
                    newsItem.newsItemUI.ImageContainer.SetImageSkin(itemOrChildImage, 1.734104f);
                    newsItem.newsItemUI.ImageContainer.FitInParent = true;
                }
            }
        }

        [OnEventFire]
        public void ApplySale(ApplyMarketItemSaleClientEvent e, MarketItemNode marketItem)
        {
            MarketItemSaleComponent component = new MarketItemSaleComponent {
                endDate = e.EndDate
            };
            marketItem.Entity.RemoveComponentIfPresent<MarketItemSaleComponent>();
            marketItem.Entity.AddComponent(component);
        }

        private int CalculateAdditionalPrice(ItemAutoIncreasePriceComponent increasePrice, int itemCount)
        {
            itemCount++;
            if (itemCount <= increasePrice.StartCount)
            {
                return 0;
            }
            int num2 = (itemCount - increasePrice.StartCount) * increasePrice.PriceIncreaseAmount;
            int maxAdditionalPrice = increasePrice.MaxAdditionalPrice;
            return (((maxAdditionalPrice <= 0) || (num2 < maxAdditionalPrice)) ? num2 : maxAdditionalPrice);
        }

        [OnEventFire]
        public void CancelSale(CancelMarketItemSaleClientEvent e, MarketItemNode marketItem)
        {
            marketItem.Entity.RemoveComponentIfPresent<MarketItemSaleComponent>();
        }

        [OnEventFire]
        public void FilterFirstPurchase(NewsItemFilterEvent e, SingleNode<NewsItemComponent> newsItem, [JoinAll] Optional<ActivePaymentSaleNode> saleState)
        {
            if (this.IsFirstPurchaseNews(newsItem))
            {
                e.Hide = !saleState.IsPresent() || !saleState.Get().activePaymentSale.Personal;
            }
        }

        [OnEventFire]
        public void FilterOwnItems(NewsItemFilterEvent e, NewsItemWithMarketItemGroupNode newsItem, [JoinByMarketItem, Combine] SingleNode<UserItemComponent> userItem)
        {
            e.Hide = !userItem.Entity.HasComponent<UserItemCounterComponent>();
        }

        private string GetItemOrChildImage(Entity item)
        {
            string spriteUid = null;
            if (item.HasComponent<ImageItemComponent>())
            {
                spriteUid = item.GetComponent<ImageItemComponent>().SpriteUid;
            }
            if (spriteUid == null)
            {
                DefaultSkinWithImageNode node = base.Select<DefaultSkinWithImageNode>(item, typeof(ParentGroupComponent)).FirstOrDefault<DefaultSkinWithImageNode>();
                if (node != null)
                {
                    spriteUid = node.imageItem.SpriteUid;
                }
            }
            return spriteUid;
        }

        private bool IsFirstPurchaseNews(SingleNode<NewsItemComponent> newsItem) => 
            !string.IsNullOrEmpty(newsItem.component.Data.PreviewImageUrl) ? newsItem.component.Data.PreviewImageUrl.Contains("illustration_sale") : false;

        [OnEventFire]
        public void UpdateFirstBuySale(UpdateGaragePriceEvent e, MarketItemWithFirstBuySaleNode marketItem, [JoinByMarketItem] ICollection<UserItemNode> userItems)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = i => i.userGroup.Key == SelfUserComponent.SelfUser.Id;
            }
            int num = (userItems.Where<UserItemNode>(<>f__am$cache0).ToList<UserItemNode>().Count != 0) ? 0 : marketItem.firstBuySale.SalePercent;
            GarageItemsRegistry.GetItem<GarageItem>(marketItem.Entity).PersonalSalePercent = num;
        }

        [OnEventFire]
        public void UpdateGaragePrice(NodeAddedEvent e, MarketItemNode marketItem)
        {
            base.NewEvent<UpdateGaragePriceEvent>().Attach(marketItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateGaragePrice(NodeAddedEvent e, UserItemNode userItem, [JoinByMarketItem] MarketItemNode marketItem)
        {
            base.NewEvent<UpdateGaragePriceEvent>().Attach(marketItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateIncreasePrice(UpdateGaragePriceEvent e, MarketItemWithAutoIncreasePriceNode marketItem, [JoinByMarketItem] ICollection<UserItemNode> userItems)
        {
            int itemCount = 0;
            foreach (UserItemNode node in userItems)
            {
                if (node.Entity.IsSameGroup<UserGroupComponent>(SelfUserComponent.SelfUser) && !node.Entity.HasComponent<CreatedByRankItemComponent>())
                {
                    itemCount++;
                }
            }
            GarageItemsRegistry.GetItem<GarageItem>(marketItem.Entity).AdditionalPrice = this.CalculateAdditionalPrice(marketItem.itemAutoIncreasePrice, itemCount);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class ActivePaymentSaleNode : Node
        {
            public ActivePaymentSaleComponent activePaymentSale;
            public SelfUserComponent selfUser;
        }

        public class DefaultSkinWithImageNode : Node
        {
            public ImageItemComponent imageItem;
            public DefaultSkinItemComponent defaultSkinItem;
            public SkinItemComponent skinItem;
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketItemWithAutoIncreasePriceNode : Node
        {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
            public ItemAutoIncreasePriceComponent itemAutoIncreasePrice;
        }

        public class MarketItemWithFirstBuySaleNode : Node
        {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
            public FirstBuySaleComponent firstBuySale;
        }

        public class NewsItemUIWithMarketItemGroupNode : Node
        {
            public NewsItemComponent newsItem;
            public NewsItemUIComponent newsItemUI;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class NewsItemWithMarketItemGroupNode : Node
        {
            public NewsItemComponent newsItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class UpdateGaragePriceEvent : Event
        {
        }

        [Not(typeof(CreatedByRankItemComponent))]
        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

