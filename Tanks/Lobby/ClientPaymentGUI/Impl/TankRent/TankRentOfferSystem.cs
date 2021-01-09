namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent
{
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientPaymentGUI.Impl;

    public class TankRentOfferSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMethod(NodeAddedEvent e, [Combine] SingleNode<PaymentMethodComponent> method, SingleNode<TankPurchaseScreenComponent> starterPack)
        {
            starterPack.component.AddMethod(method.Entity);
        }

        [OnEventFire]
        public void BuyPreset(ConfirmButtonClickEvent e, SingleNode<TankPurchaseScreenComponent> purchaseScreen, [JoinAll] SingleNode<SelectedPresetComponent> selectedPreset, [JoinBy(typeof(SpecialOfferGroupComponent))] TankRentOfferNode offer, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            ShopDialogs dialogs2 = dialogs.component.Get<ShopDialogs>();
            purchaseScreen.component.OpenPurchaseWindow(offer.Entity, dialogs2);
        }

        [OnEventFire]
        public void Cancel(PaymentIsCancelledEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            base.Log.Error("Error making payment: " + e.ErrorCode);
            deals.component.HandleError();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void DisablePresetNameChange(NodeAddedEvent e, SingleNode<SelectedPresetComponent> selectedPreset, [Context] SingleNode<PresetNameEditorComponent> presetNameEditor)
        {
            if (selectedPreset.Entity.HasComponent<SpecialOfferGroupComponent>())
            {
                presetNameEditor.component.DisableInput();
            }
            else
            {
                presetNameEditor.component.EnableInput();
            }
        }

        [OnEventFire]
        public void DisplayNumberOfBattlesLeft(NodeAddedEvent e, SingleNode<NumberOfBattlesLeftUIComponent> UI, SingleNode<SelectedPresetComponent> selectedPreset, [JoinBy(typeof(SpecialOfferGroupComponent)), Context] SingleNode<NumberOfBattlesPlayedWithTankComponent> personalOffer)
        {
            UI.component.DisplayBattlesLeft(personalOffer.component.BattlesLeft);
        }

        [OnEventFire]
        public void FillPurchaseWindow(NodeAddedEvent e, SingleNode<TankPurchaseScreenComponent> screen, [Context] SingleNode<SelectedPresetComponent> selectedPreset, [JoinBy(typeof(SpecialOfferGroupComponent)), Context, Combine] TankRentOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent)), Context] PersonalOfferNode personalOffer, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            ShopDialogs shopDialogs = dialogs.component.Get<ShopDialogs>();
            screen.component.InitiateScreen(offer.goodsPrice, personalOffer.discount, offer.legendaryTankSpecialOffer.TankRole, shopDialogs);
        }

        [OnEventFire]
        public void GoToUrl(GoToUrlToPayEvent e, Node any, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            deals.component.HandleGoToLink();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void GoToUrl(SteamBuyGoodsEvent e, Node any, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            deals.component.HandleGoToLink();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void HideMastery(NodeAddedEvent e, SingleNode<SelectedPresetComponent> selectedPreset, [Context] SingleNode<SelectedTurretUIComponent> selectedTurret, [Context] SingleNode<SelectedHullUIComponent> selectedHull)
        {
            if (selectedPreset.Entity.HasComponent<SpecialOfferGroupComponent>())
            {
                selectedTurret.component.DisableMasteryElement();
                selectedHull.component.DisableMasteryElement();
            }
            else
            {
                selectedTurret.component.EnableMasteryElement();
                selectedHull.component.EnableMasteryElement();
            }
        }

        [OnEventFire]
        public void HideOfferButton(HideOfferButtonEvent e, PersonalOfferWithEndTimeNode personalOffer, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void OnDiscountAdded(NodeAddedEvent e, [Combine] TankRentOfferNode good, [Context, JoinBy(typeof(SpecialOfferGroupComponent))] PersonalOfferNode personalOffer)
        {
            if (good.Entity.HasComponent<CustomOfferPriceForUIComponent>())
            {
                good.Entity.RemoveComponent<CustomOfferPriceForUIComponent>();
            }
            double num = good.goodsPrice.Price * (1f - personalOffer.discount.DiscountCoeff);
            num = good.goodsPrice.Round(num);
            good.Entity.AddComponent(new CustomOfferPriceForUIComponent(num));
        }

        [OnEventFire]
        public void QiwiError(InvalidQiwiAccountEvent e, Node node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            base.Log.Error("QIWI ERROR");
            deals.component.HandleQiwiError();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void ScheduleButtonHide(NodeAddedEvent e, PersonalOfferWithEndTimeNode personalOffer, [Context, JoinBy(typeof(SpecialOfferGroupComponent)), Combine] TankRentOfferNode offer)
        {
            float timeInSec = (float) (personalOffer.specialOfferEndTime.EndDate - Date.Now);
            base.NewEvent(new HideOfferButtonEvent()).Attach(personalOffer).ScheduleDelayed(timeInSec);
        }

        [OnEventFire]
        public void SetButtonBehaviour(NodeAddedEvent e, [Combine] PersonalOfferNode personalOffer, [JoinBy(typeof(SpecialOfferGroupComponent)), Context, Combine] TankRentOfferNode offer, [Context] SelfUserNode selfUser, [Context, Combine] OwnedPresetNode preset, [Context] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            if (preset.userGroup.Key != selfUser.userGroup.Key)
            {
                helper.component.SetButtonToScreenDisplayState();
            }
        }

        [OnEventFire]
        public void SetTimer(NodeAddedEvent e, [Combine] SingleNode<TankRentLeafletComponent> leaflet, [Context, Combine] PersonalOfferWithEndTimeNode personalOffer, [JoinBy(typeof(SpecialOfferGroupComponent)), Context, Combine] TankRentOfferNode offer, [Context] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            <SetTimer>c__AnonStorey2 storey = new <SetTimer>c__AnonStorey2 {
                helper = helper
            };
            leaflet.component.starterPackTimer.onTimerExpired = new StarterPackTimerComponent.TimerExpired(storey.<>m__0);
            float remaining = (float) (personalOffer.specialOfferEndTime.EndDate - Date.Now);
            leaflet.component.starterPackTimer.RunTimer(remaining);
        }

        [OnEventFire]
        public void SetTimerOnPresetsScreen(NodeAddedEvent e, SingleNode<LegendaryTankTimerComponent> timer, [Context] SingleNode<SelectedPresetComponent> selectedPreset, [JoinBy(typeof(SpecialOfferGroupComponent)), Context] PersonalOfferWithEndTimeNode personalOffer)
        {
            float remaining = (float) (personalOffer.specialOfferEndTime.EndDate - Date.Now);
            timer.component.timer.RunTimer(remaining);
        }

        [OnEventFire]
        public void ShowLegendaryTankSpecialOfferStarted(NodeAddedEvent e, [Combine] TankRentOfferNode offer, [Context, JoinBy(typeof(SpecialOfferGroupComponent))] OfferThatMustBeShown personalOffer, [Context] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            helper.component.tankRentOffer.SetActive(true);
            helper.component.SetButtonToOfferDisplayState();
            helper.component.tankRentButton.gameObject.SetActive(true);
            base.NewEvent<SpecialOfferScreenShownEvent>().Attach(personalOffer).Schedule();
        }

        [OnEventFire]
        public void ShowRentButton(NodeAddedEvent e, [Combine] PersonalOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent)), Context, Combine] TankRentOfferNode offer2, SingleNode<TankRentMainScreenElementsComponents> helper, [JoinAll] ICollection<TankRentOfferNode> specialOffers, [JoinAll] ICollection<PersonalOfferNode> personalOffers)
        {
            <ShowRentButton>c__AnonStorey0 storey = new <ShowRentButton>c__AnonStorey0 {
                specialOffers = specialOffers
            };
            if (personalOffers.Sum<PersonalOfferNode>(new Func<PersonalOfferNode, int>(storey.<>m__0)) == 3)
            {
                helper.component.tankRentButton.gameObject.SetActive(true);
            }
        }

        [OnEventFire]
        public void ShowTanksScreen(ButtonClickEvent e, SingleNode<TankRentResearchConfirmButton> button, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<TankRentOfferNode> offers, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            helper.component.SetButtonToScreenDisplayState();
            helper.component.tankRentOffer.SetActive(false);
            helper.component.ShowTankRentScreen();
            foreach (TankRentOfferNode node in offers)
            {
                IList<PersonalOfferNode> nodes = base.Select<PersonalOfferNode, SpecialOfferGroupComponent>(node.Entity);
                base.NewEvent(new StartSpecialOfferEvent()).Attach(selfUser).Attach<PersonalOfferNode>(nodes).Attach(node).Schedule();
            }
        }

        [OnEventFire]
        public void StartOffer(ButtonClickEvent e, SingleNode<TankRentLeafletComponent> leaflet, [JoinAll] SelfUserNode selfUser, [JoinByUser, Combine] PersonalOfferNode personalOffer, [JoinBy(typeof(SpecialOfferGroupComponent))] TankRentOfferNode offer, [JoinBy(typeof(SpecialOfferGroupComponent))] OwnedPresetNode preset, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            if (leaflet.component.TankRole == offer.legendaryTankSpecialOffer.TankRole)
            {
                helper.component.HideTankRentScreen();
                base.NewEvent(new MountPresetEvent()).Attach(preset).Schedule();
            }
        }

        [OnEventComplete]
        public void SteamComponentAdded(NodeAddedEvent e, SingleNode<SteamComponent> stemNode, [Context] SingleNode<TankPurchaseScreenComponent> starterPack)
        {
            starterPack.component.SteamComponentIsPresent = true;
        }

        [OnEventFire]
        public void Success(SuccessPaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            deals.component.HandleSuccess();
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void SuccessMobile(SuccessMobilePaymentEvent e, SingleNode<PaymentMethodComponent> node, [JoinAll] SingleNode<TankPurchaseScreenComponent> deals, [JoinAll] SingleNode<TankRentMainScreenElementsComponents> helper)
        {
            deals.component.HandleSuccessMobile(e.TransactionId);
            deals.component.gameObject.SetActive(false);
            helper.component.tankRentButton.gameObject.SetActive(false);
        }

        [CompilerGenerated]
        private sealed class <SetTimer>c__AnonStorey2
        {
            internal SingleNode<TankRentMainScreenElementsComponents> helper;

            internal void <>m__0()
            {
                this.helper.component.HideTankRentScreen();
            }
        }

        [CompilerGenerated]
        private sealed class <ShowRentButton>c__AnonStorey0
        {
            internal ICollection<TankRentOfferSystem.TankRentOfferNode> specialOffers;

            internal int <>m__0(TankRentOfferSystem.PersonalOfferNode personalOffer)
            {
                <ShowRentButton>c__AnonStorey1 storey = new <ShowRentButton>c__AnonStorey1 {
                    <>f__ref$0 = this,
                    personalOffer = personalOffer
                };
                return this.specialOffers.Count<TankRentOfferSystem.TankRentOfferNode>(new Func<TankRentOfferSystem.TankRentOfferNode, bool>(storey.<>m__0));
            }

            private sealed class <ShowRentButton>c__AnonStorey1
            {
                internal TankRentOfferSystem.PersonalOfferNode personalOffer;
                internal TankRentOfferSystem.<ShowRentButton>c__AnonStorey0 <>f__ref$0;

                internal bool <>m__0(TankRentOfferSystem.TankRentOfferNode specialOffer) => 
                    this.personalOffer.specialOfferGroup.Key == specialOffer.specialOfferGroup.Key;
            }
        }

        public class HideOfferButtonEvent : Event
        {
        }

        public class OfferThatMustBeShown : TankRentOfferSystem.PersonalOfferNode
        {
            public SpecialOfferShowScreenForcedComponent specialOfferShowScreenForced;
        }

        public class OwnedPresetNode : Node
        {
            public PresetItemComponent presetItem;
            public UserGroupComponent userGroup;
        }

        public class PersonalOfferNode : Node
        {
            public PersonalSpecialOfferPropertiesComponent personalSpecialOfferProperties;
            public SpecialOfferGroupComponent specialOfferGroup;
            public DiscountComponent discount;
        }

        public class PersonalOfferWithEndTimeNode : TankRentOfferSystem.PersonalOfferNode
        {
            public SpecialOfferEndTimeComponent specialOfferEndTime;
        }

        public class SelectedPresetNode : TankRentOfferSystem.OwnedPresetNode
        {
            public SelectedPresetComponent selectedPreset;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class TankRentOfferNode : Node
        {
            public SpecialOfferComponent specialOffer;
            public SpecialOfferGroupComponent specialOfferGroup;
            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
            public GoodsPriceComponent goodsPrice;
        }
    }
}

