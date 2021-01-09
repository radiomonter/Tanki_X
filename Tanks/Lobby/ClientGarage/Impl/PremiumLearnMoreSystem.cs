namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;

    public class PremiumLearnMoreSystem : ECSSystem
    {
        private void FillElements(List<GoodsNode> sortedGoods, SingleNode<PremiumLearnMoreComponent> infoUi)
        {
            for (int i = 0; i < sortedGoods.Count; i++)
            {
                this.FillUiElement(sortedGoods.ElementAt<GoodsNode>(i), infoUi.component.uiElements[i]);
            }
        }

        [OnEventFire]
        public void FillInfoElements(NodeAddedEvent e, SingleNode<PremiumLearnMoreComponent> infoUi, [JoinAll] ICollection<GoodsNode> goods, [JoinAll] PremiumUiShopSystem.BaseUserNode selfUser)
        {
            List<GoodsNode> sortedGoods = new List<GoodsNode>();
            SelectGoods(goods, selfUser, sortedGoods);
            sortedGoods.Sort(new PremiumGoodsNodeComparer());
            this.FillElements(sortedGoods, infoUi);
        }

        private void FillUiElement(GoodsNode sortedGoods, PremiumInfoUiElement infoUi)
        {
            string str = sortedGoods.countableItemsPack.Pack.First<KeyValuePair<long, int>>().Value.ToString();
            string description = sortedGoods.specialOfferContentLocalization.Description;
            bool flag = sortedGoods.countableItemsPack.Pack.ContainsKey(-180272377L);
            string str3 = !flag ? string.Empty : "+";
            string[] textArray1 = new string[] { str, " ", description, " ", str3 };
            infoUi.daysText.text = string.Concat(textArray1);
            infoUi.crystalObject.SetActive(flag);
            infoUi.tabCrystalObject.SetActive(flag);
        }

        private static void SelectGoods(ICollection<GoodsNode> goods, PremiumUiShopSystem.BaseUserNode selfUser, List<GoodsNode> sortedGoods)
        {
            int rank = selfUser.userRank.Rank;
            foreach (GoodsNode node in goods)
            {
                int minRank = node.premiumOffer.MinRank;
                int maxRank = node.premiumOffer.MaxRank;
                if ((rank >= minRank) && (rank < maxRank))
                {
                    sortedGoods.Add(node);
                }
            }
        }

        [OnEventFire]
        public void ShowInfoDialog(ButtonClickEvent e, SingleNode<PremiumLearnMoreButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs60)
        {
            PremiumLearnMoreComponent component = dialogs60.component.Get<PremiumLearnMoreComponent>();
            component.tabManager.index = button.component.idx;
            component.Show(null);
        }

        public class GoodsNode : Node
        {
            public PremiumOfferComponent premiumOffer;
            public CountableItemsPackComponent countableItemsPack;
            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;
            public OrderItemComponent orderItem;
        }

        private class PremiumGoodsNodeComparer : IComparer<PremiumLearnMoreSystem.GoodsNode>
        {
            public int Compare(PremiumLearnMoreSystem.GoodsNode a, PremiumLearnMoreSystem.GoodsNode b) => 
                a.orderItem.Index.CompareTo(b.orderItem.Index);
        }
    }
}

