namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class GoldBoxesUiShopSystem : ECSSystem
    {
        private List<GoldBoxOfferNode> BuildList(ICollection<GoldBoxOfferNode> goods) => 
            new List<GoldBoxOfferNode>(goods);

        [OnEventFire]
        public void CreatePacks(NodeAddedEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode, [JoinAll] ICollection<GoldBoxOfferNode> goods)
        {
            List<GoldBoxOfferNode> list = this.BuildList(goods);
            list.Sort(new GoldBoxNodeComparer());
            foreach (GoldBoxOfferNode node in list)
            {
                GoldBoxesPackComponent pack = Object.Instantiate<GameObject>(shopNode.component.PackPrefab, shopNode.component.PackContainer).GetComponent<GoldBoxesPackComponent>();
                this.FillPack(pack, node);
            }
        }

        [OnEventFire]
        public void DestroyPacks(NodeRemoveEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode)
        {
            IEnumerator enumerator = shopNode.component.PackContainer.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Object.Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void FillPack(GoldBoxesPackComponent pack, GoldBoxOfferNode packNode)
        {
            pack.CardName = packNode.specialOfferContentLocalization.Title;
            pack.SpriteUid = packNode.specialOfferScreenLocalization.SpriteUid;
            pack.Discount = packNode.specialOfferContent.SalePercent;
            pack.HitMarkEnabled = packNode.specialOfferContent.HighlightTitle;
            pack.BoxCount = packNode.goldBonusOffer.Count;
            pack.Price = $"{packNode.goodsPrice.Price:0.00} {packNode.goodsPrice.Currency}";
            pack.GoodsEntity = packNode.Entity;
        }

        [OnEventFire]
        public void GetCounter(NodeAddedEvent e, SingleNode<GoldBoxesShopTabComponent> shopNode, [JoinAll] GoldBoxItemNode gold)
        {
            shopNode.component.UserBoxCount.text = gold.userItemCounter.Count.ToString();
        }

        [OnEventFire]
        public void RefreshCounter(TryToShowNotificationEvent e, Node any, [JoinAll] GoldBoxItemNode gold, [JoinAll] SingleNode<GoldBoxesShopTabComponent> shopNode)
        {
            shopNode.component.UserBoxCount.text = gold.userItemCounter.Count.ToString();
        }

        public class GoldBoxItemNode : Node
        {
            public GoldBonusItemComponent goldBonusItem;
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
            public UserItemCounterComponent userItemCounter;
        }

        private class GoldBoxNodeComparer : IComparer<GoldBoxesUiShopSystem.GoldBoxOfferNode>
        {
            public int Compare(GoldBoxesUiShopSystem.GoldBoxOfferNode a, GoldBoxesUiShopSystem.GoldBoxOfferNode b) => 
                a.goldBonusOffer.Count.CompareTo(b.goldBonusOffer.Count);
        }

        public class GoldBoxOfferNode : Node
        {
            public GoldBonusOfferComponent goldBonusOffer;
            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;
            public SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization;
            public SpecialOfferContentComponent specialOfferContent;
            public GoodsPriceComponent goodsPrice;
        }
    }
}

