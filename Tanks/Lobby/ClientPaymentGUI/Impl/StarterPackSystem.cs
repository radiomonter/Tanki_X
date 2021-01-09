namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using UnityEngine;

    public class StarterPackSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<StarterPackScreenUIComponent> starterPack)
        {
            starterPack.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void AddSpecialOfferButtonOnLobbyEnter(NodeAddedEvent e, SingleNode<MainScreenComponent> mainScreen, [Combine] PersonalOfferNode personalOffer, [Context, JoinBy(typeof(SpecialOfferGroupComponent))] SpecialOfferNode specialOffer)
        {
            GameObject starterPackButton = mainScreen.component.StarterPackButton;
            StarterPackButtonComponent component = starterPackButton.GetComponent<StarterPackButtonComponent>();
            component.PackEntity = specialOffer.Entity;
            component.SetImage(specialOffer.starterPackVisualConfig.ButtonSpriteUid);
            starterPackButton.SetActive(true);
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals)
        {
            base.Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
        }

        [OnEventFire]
        public void CancelShopDialog(NodeRemoveEvent e, SingleNode<StarterPackScreenUIComponent> screen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            screen.component.shopDialogs.Cancel();
        }

        [OnEventFire]
        public void CheckItem(RequestInfoForItemsEvent e, SingleNode<StarterPackMainElementComponent> starterPack, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystal, [JoinAll] XCrystalMarketItemNode xCrystal)
        {
            <CheckItem>c__AnonStorey0 storey = new <CheckItem>c__AnonStorey0 {
                e = e,
                user = user,
                $this = this
            };
            storey.e.itemIds.ForEach(new Action<long>(storey.<>m__0));
            storey.e.crystalTitle = crystal.descriptionItem.Name;
            storey.e.crystalSprite = crystal.imageItem.SpriteUid;
            storey.e.xCrystalTitle = xCrystal.descriptionItem.Name;
            storey.e.xCrystalSprite = xCrystal.imageItem.SpriteUid;
            long itemId = starterPack.component.ItemId;
            Entity entityById = base.GetEntityById(itemId);
            storey.e.mainItemId = itemId;
            storey.e.mainItemDescription = starterPack.component.Description;
            storey.e.mainItemSprite = entityById.GetComponent<ImageItemComponent>().SpriteUid;
            storey.e.mainItemTitle = starterPack.component.Title;
            storey.e.mainItemCount = starterPack.component.Count;
            storey.e.mainItemCrystal = ReferenceEquals(entityById, crystal.Entity);
            storey.e.mainItemXCrystal = ReferenceEquals(entityById, xCrystal.Entity);
        }

        [OnEventFire]
        public void Clear(NodeRemoveEvent e, SingleNode<StarterPackScreenUIComponent> starterPack)
        {
            starterPack.component.Clear();
        }

        [OnEventFire]
        public void CloseScreenOnComplete(NodeRemoveEvent e, PersonalOfferNode personalOffer, [JoinAll] SingleNode<StarterPackScreenUIComponent> screen)
        {
            screen.component.Close();
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals)
        {
            base.Log.Debug("GoToUrl");
            deals.component.HandleGoToLink();
        }

        [OnEventFire]
        public void OpenSpecialOfferScreen(ShowStarterPackScreen e, SpecialOfferNode specialOffer, [JoinAll] SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.CloseAll(string.Empty);
            GameObject starterPackScreen = mainScreen.component.StarterPackScreen;
            starterPackScreen.GetComponent<StarterPackScreenUIComponent>().shopDialogs = dialogs.component.Get<ShopDialogs>();
            starterPackScreen.GetComponent<StarterPackScreenUIComponent>().PackEntity = specialOffer.Entity;
            MainScreenComponent.Instance.ShowStarterPack();
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals)
        {
            base.Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
        }

        [OnEventFire]
        public void RemoveButtonOnComplete(NodeRemoveEvent e, [Combine] PersonalOfferNode personalOffer, [JoinAll, Context] SingleNode<MainScreenComponent> mainScreen)
        {
            GameObject starterPackButton = mainScreen.component.StarterPackButton;
            if (starterPackButton != null)
            {
                starterPackButton.gameObject.SetActive(false);
            }
        }

        [OnEventFire]
        public void ShowScreenAfterBattle(NodeAddedEvent e, [Combine] SpecialOfferNode specialOffer, [Context, JoinBy(typeof(SpecialOfferGroupComponent))] PersonalOfferWithScreenNode personalOffer, [Context, JoinAll] SingleNode<MainScreenComponent> mainScreen)
        {
            base.NewEvent<ShowStarterPackScreen>().Attach(specialOffer).ScheduleDelayed(0f);
            base.NewEvent<SpecialOfferScreenShownEvent>().Attach(personalOffer).Schedule();
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<StarterPackScreenUIComponent> starterPack)
        {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals)
        {
            base.Log.Debug("Success");
            deals.component.HandleSuccess();
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<StarterPackScreenUIComponent> deals)
        {
            base.Log.Debug("SuccessMobile");
            deals.component.HandleSuccessMobile(e.TransactionId);
        }

        [CompilerGenerated]
        private sealed class <CheckItem>c__AnonStorey0
        {
            internal RequestInfoForItemsEvent e;
            internal StarterPackSystem.SelfUserNode user;
            internal StarterPackSystem $this;

            internal void <>m__0(long id)
            {
                Entity entityById = this.$this.GetEntityById(id);
                this.e.titles.Add(id, MarketItemNameLocalization.GetDetailedName(entityById));
                this.e.previews.Add(id, entityById.GetComponent<ImageItemComponent>().SpriteUid);
                ItemRarityComponent component = entityById.GetComponent<ItemRarityComponent>();
                this.e.rarityFrames.Add(id, component.NeedRarityFrame);
                this.e.rarities.Add(id, component.RarityType);
                if (this.user.purchasedItemList.Contains(id))
                {
                    this.e.purchased.Add(id);
                }
            }
        }

        public class CrystalMarketItemNode : StarterPackSystem.MarketItemNode
        {
            public CrystalItemComponent crystalItem;
        }

        public class MarketItemNode : Node
        {
            public DescriptionItemComponent descriptionItem;
            public ImageItemComponent imageItem;
            public MarketItemComponent marketItem;
        }

        public class PersonalOfferNode : Node
        {
            public SpecialOfferVisibleComponent specialOfferVisible;
            public PersonalSpecialOfferScreenComponent personalSpecialOfferScreen;
            public SpecialOfferRemainingTimeComponent specialOfferRemainingTime;
        }

        public class PersonalOfferWithScreenNode : StarterPackSystem.PersonalOfferNode
        {
            public SpecialOfferShowScreenForcedComponent specialOfferShowScreenForced;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public PurchasedItemListComponent purchasedItemList;
        }

        public class ShowStarterPackScreen : Event
        {
        }

        public class SpecialOfferNode : Node
        {
            public SpecialOfferComponent specialOffer;
            public SpecialOfferGroupComponent specialOfferGroup;
            public SpecialOfferEndTimeComponent specialOfferEndTime;
            public SpecialOfferContentLocalizationComponent specialOfferContentLocalization;
            public SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization;
            public SpecialOfferContentComponent specialOfferContent;
            public GoodsPriceComponent goodsPrice;
            public ItemsPackFromConfigComponent itemsPackFromConfig;
            public CountableItemsPackComponent countableItemsPack;
            public XCrystalsPackComponent xCrystalsPack;
            public CrystalsPackComponent crystalsPack;
            public BaseStarterPackSpecialOfferComponent baseStarterPackSpecialOffer;
            public StarterPackVisualConfigComponent starterPackVisualConfig;
        }

        public class StarterPackRegistrationNode : Node
        {
            public XCrystalsPackComponent xCrystalsPack;
            public CrystalsPackComponent crystalsPack;
            public CountableItemsPackComponent countableItemsPack;
        }

        public class XCrystalMarketItemNode : StarterPackSystem.MarketItemNode
        {
            public XCrystalItemComponent xCrystalItem;
        }
    }
}

