namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.API;
    using Lobby.ClientPayment.Impl;
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    public class SelectCountryScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<KeyValuePair<string, string>> <>f__am$cache0;

        [OnEventFire]
        public void ChangeCountry(DialogConfirmEvent e, SingleNode<SelectCountryDialogComponent> dialog)
        {
            if (!string.IsNullOrEmpty(dialog.component.country.Value))
            {
                SelectCountryEvent eventInstance = new SelectCountryEvent {
                    CountryCode = dialog.component.country.Key,
                    CountryName = dialog.component.country.Value
                };
                base.ScheduleEvent(eventInstance, dialog.Entity);
                dialog.component.Hide();
            }
        }

        [OnEventFire]
        public void ChangeCountry(ConfirmUserCountryEvent e, SingleNode<UserCountryComponent> country, [JoinAll] Optional<SingleNode<OpenSelectCountryButtonComponent>> button)
        {
            country.component.CountryCode = e.CountryCode;
            if (button.IsPresent())
            {
                button.Get().component.CountryCode = e.CountryCode;
            }
        }

        [OnEventFire]
        public void Continue(SelectCountryEvent e, Node stub, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            ConfirmUserCountryEvent eventInstance = new ConfirmUserCountryEvent {
                CountryCode = e.CountryCode
            };
            base.ScheduleEvent(eventInstance, selfUser);
            PaymentStatisticsEvent event3 = new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.COUNTRY_SELECT,
                Screen = "SelectCountryScreen"
            };
            base.ScheduleEvent(event3, session);
        }

        [OnEventFire]
        public void DisableCountryButton(DisableCountryButtonEvent e, SingleNode<OpenSelectCountryButtonComponent> button)
        {
            button.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void GoToPayment(ButtonClickEvent e, SingleNode<UserXCrystalsIndicatorComponent> button, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<GoToXCryShopScreen>(user);
        }

        [OnEventFire]
        public void InitChangeCountryButton(NodeAddedEvent e, SingleNode<OpenSelectCountryButtonComponent> button, UserWithCountryNode country)
        {
            if ((country.userCountry.CountryCode == "RU") || (country.userPublisher.Publisher == Publisher.CONSALA))
            {
                base.NewEvent<DisableCountryButtonEvent>().Attach(button).ScheduleDelayed(0f);
            }
            button.component.CountryCode = country.userCountry.CountryCode;
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] SingleNode<CountriesComponent> countries, [JoinAll] UserWithCountryNode country)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in countries.component.Names)
            {
                if (pair.Key != "TR")
                {
                    list.Add(pair);
                }
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => string.Compare(a.Value, b.Value, StringComparison.Ordinal);
            }
            list.Sort(<>f__am$cache0);
            screen.selectCountryDialog.Init(list);
            screen.selectCountryDialog.Select(country.userCountry.CountryCode);
        }

        [OnEventFire]
        public void LogEnterPayment(GoToPaymentRequestEvent e, SingleNode<SelfUserComponent> user, [JoinByUser] SingleNode<ClientSessionComponent> session, [JoinAll] ActiveScreenNode activeScreenNode)
        {
            PaymentStatisticsEvent eventInstance = new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.OPEN_PAYMENT,
                Screen = activeScreenNode.screen.gameObject.name
            };
            base.ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void ParsePaymentLink(ParseLinkEvent e, Node node, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            if (e.Link.StartsWith("payment"))
            {
                e.CustomNavigationEvent = base.NewEvent<GoToXCryShopScreen>().Attach(user);
            }
        }

        public class ActiveScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class ScreenNode : Node
        {
            public SelectCountryDialogComponent selectCountryDialog;
        }

        public class UserWithCountryNode : Node
        {
            public SelfUserComponent selfUser;
            public UserCountryComponent userCountry;
            public UserPublisherComponent userPublisher;
        }

        [Not(typeof(UserCountryComponent))]
        public class UserWithoutCountryNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

