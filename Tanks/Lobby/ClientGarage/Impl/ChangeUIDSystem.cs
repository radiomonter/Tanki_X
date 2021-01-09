namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientEntrance.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientPayment.API;
    using Tanks.Lobby.ClientUserProfile.Impl.ChangeUID;

    public class ChangeUIDSystem : ECSSystem
    {
        [OnEventFire]
        public void CompleteBuyUIDChange(CompleteBuyUIDChangeEvent e, SelfUserNode userNode, [JoinAll] ActiveChangeUIDScreenNode activeChangeUIDScreenNode, [JoinByScreen] XButtonNode buttonNode, [JoinByScreen] LoginInputFieldValidStateNode inputField)
        {
            if (e.Success)
            {
                base.ScheduleEvent<UIDChangedEvent>(userNode);
                base.ScheduleEvent<ShowScreenLeftEvent<MainScreenComponent>>(userNode);
            }
            else
            {
                inputField.inputField.Input = string.Empty;
                buttonNode.confirmButton.FlipFront();
            }
        }

        public class ActiveChangeUIDScreenNode : Node
        {
            public ChangeUIDScreenComponent changeUIDScreen;
            public ActiveScreenComponent activeScreen;
        }

        public class ChangeUIDNode : Node
        {
            public ChangeUIDComponent changeUid;
            public GoodsXPriceComponent goodsXPrice;
            public XPriceLabelComponent xPriceLabel;
        }

        public class LoginInputFieldValidStateNode : Node
        {
            public RegistrationLoginInputComponent registrationLoginInput;
            public InputFieldComponent inputField;
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class SelfSteamUserNode : Node
        {
            public SteamUserComponent steamUser;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
        }

        public class XButtonNode : Node
        {
            public XPriceLabelComponent xPriceLabel;
            public BuyButtonComponent buyButton;
            public ConfirmButtonComponent confirmButton;
        }
    }
}

