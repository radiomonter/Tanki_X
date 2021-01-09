namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class CreateBuyItemPacksButtonsSystem : ECSSystem
    {
        private void ActivateButton(ScreenNode screen, int packSize, int index, bool priceActivity, bool xPriceActivity)
        {
            EntityBehaviour behaviour = screen.buyItemPacksButtons.BuyButtons[index];
            behaviour.GetComponent<ItemPackButtonComponent>().Count = packSize;
            behaviour.GetComponent<UniversalPriceButtonComponent>().PriceActivity = priceActivity;
            behaviour.GetComponent<UniversalPriceButtonComponent>().XPriceActivity = xPriceActivity;
            behaviour.gameObject.SetActive(true);
            base.NewEvent<SetBuyItemPackButtonInfoEvent>().Attach(behaviour.Entity).Schedule();
        }

        [OnEventFire]
        public void ClearBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinAll] ScreenNode screen)
        {
            screen.buyItemPacksButtons.SetBuyButtonsInactive();
        }

        private int CreateButtons(List<int> packs, ScreenNode screen, int index, bool priceActivity, bool xPriceActivity)
        {
            <CreateButtons>c__AnonStorey0 storey = new <CreateButtons>c__AnonStorey0 {
                screen = screen,
                index = index,
                priceActivity = priceActivity,
                xPriceActivity = xPriceActivity,
                $this = this
            };
            if ((packs != null) && packs.Any<int>())
            {
                packs.ForEach(new Action<int>(storey.<>m__0));
            }
            return storey.index;
        }

        [OnEventComplete]
        public void CreateBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinByMarketItem] MarketItemNode marketItem, [JoinAll] ScreenNode screen)
        {
            int index = 0;
            index = this.CreateButtons(marketItem.itemPacks.ForPrice, screen, index, true, false);
            this.CreateButtons(marketItem.itemPacks.ForXPrice, screen, index, false, true);
        }

        [OnEventComplete]
        public void CreateBuyButtons(ListItemSelectedEvent e, GarageItemNode item, [JoinByMarketItem] MarketItemPackNode marketItem, [JoinAll] ScreenNode screen)
        {
            int index = 0;
            index = this.CreateButtons(marketItem.packPrice.PackPrice.Keys.ToList<int>(), screen, index, true, false);
            this.CreateButtons(marketItem.packPrice.PackXPrice.Keys.ToList<int>(), screen, index, false, true);
        }

        [CompilerGenerated]
        private sealed class <CreateButtons>c__AnonStorey0
        {
            internal CreateBuyItemPacksButtonsSystem.ScreenNode screen;
            internal int index;
            internal bool priceActivity;
            internal bool xPriceActivity;
            internal CreateBuyItemPacksButtonsSystem $this;

            internal void <>m__0(int packSize)
            {
                if (packSize > 0)
                {
                    this.$this.ActivateButton(this.screen, packSize, this.index, this.priceActivity, this.xPriceActivity);
                    this.index++;
                }
            }
        }

        public class GarageItemNode : Node
        {
            public GarageListItemContentComponent garageListItemContent;
            public MarketItemGroupComponent marketItemGroup;
        }

        [Not(typeof(PackPriceComponent)), Not(typeof(HiddenInGarageItemComponent))]
        public class MarketItemNode : Node
        {
            public ItemPacksComponent itemPacks;
            public MarketItemComponent marketItem;
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
        }

        [Not(typeof(HiddenInGarageItemComponent))]
        public class MarketItemPackNode : Node
        {
            public ItemPacksComponent itemPacks;
            public MarketItemComponent marketItem;
            public PriceItemComponent priceItem;
            public XPriceItemComponent xPriceItem;
            public PackPriceComponent packPrice;
        }

        public class ScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
            public BuyItemPacksButtonsComponent buyItemPacksButtons;
        }
    }
}

