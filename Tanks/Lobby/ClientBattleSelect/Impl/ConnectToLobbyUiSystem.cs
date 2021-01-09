namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientMatchMaking.Impl;
    using tanks.modules.lobby.ClientPayment.Scripts.API;
    using UnityEngine;

    public class ConnectToLobbyUiSystem : ECSSystem
    {
        [OnEventFire]
        public void ConnectToLobby(ButtonClickEvent e, ConnectToButtonNode button, [JoinAll] DialogNode dialog, [JoinAll] SelfUserNode selfUser)
        {
            long num;
            if (long.TryParse(dialog.inputField.Input, out num))
            {
                ConnectToCustomLobbyEvent eventInstance = new ConnectToCustomLobbyEvent {
                    LobbyId = num
                };
                base.ScheduleEvent(eventInstance, selfUser);
            }
        }

        [OnEventFire]
        public void InputChanged(InputFieldValueChangedEvent e, DialogNode dialog)
        {
            dialog.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
        }

        [OnEventFire]
        public void LobbyGuiAdded(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> gui, [JoinAll] DialogNode dialog)
        {
            dialog.esm.Esm.ChangeState<InputFieldStates.ValidState>();
            dialog.connectToLobbyDialog.Hide();
        }

        [OnEventFire]
        public void LobbyOpened(NodeAddedEvent e, SelfOpenLobbyNode lobby, [JoinAll, Combine] OpenLobbyButtonNode button)
        {
            button.openLobbyButton.LobbyId = lobby.Entity.Id;
            GUIUtility.systemCopyBuffer = button.openLobbyButton.LobbyId.ToString();
            button.openLobbyButton.Price = -1;
        }

        [OnEventFire]
        public void OnEnterError(EnterBattleLobbyFailedEvent e, SelfUserNode selfUser, [JoinAll] DialogNode dialog)
        {
            dialog.inputField.ErrorMessage = !e.LobbyIsFull ? ((string) dialog.connectToLobbyDialog.CommonErrorText) : ((string) dialog.connectToLobbyDialog.LobbyIsFullText);
            dialog.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void OnError(CustomLobbyNotExistsEvent e, SelfUserNode selfUser, [JoinAll] DialogNode dialog)
        {
            dialog.inputField.ErrorMessage = (string) dialog.connectToLobbyDialog.LobbyNotExists;
            dialog.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void OpenDialog(ButtonClickEvent e, OpenDialogButtonNode button, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<ConnectToLobbyDialogComponent>().Show(null);
        }

        [OnEventFire]
        public void OpenLobby(ButtonClickEvent e, OpenLobbyButtonNode button, [JoinAll] CustomLobbyNode lobby, [JoinAll] SelfUserNode selfUser)
        {
            if (lobby.Entity.HasComponent<OpenToConnectLobbyComponent>())
            {
                GUIUtility.systemCopyBuffer = button.openLobbyButton.LobbyId.ToString();
                button.openLobbyButton.Price = -1;
            }
            else
            {
                long exchangingCrystalls = lobby.openCustomLobbyPrice.OpenPrice - selfUser.userMoney.Money;
                if (exchangingCrystalls <= 0L)
                {
                    base.ScheduleEvent<OpenCustomLobbyEvent>(selfUser);
                }
                else
                {
                    MainScreenComponent.Instance.ShowHome();
                    MainScreenComponent.Instance.ShowShopIfNotVisible();
                    base.ScheduleEvent(new GoToExchangeCryScreen(exchangingCrystalls), selfUser);
                }
            }
        }

        [OnEventFire]
        public void OpenLobbyButtonAdded(NodeAddedEvent e, OpenLobbyButtonNode button, [JoinAll] SelfUserNode selfUser, [JoinAll] CustomLobbyNode lobby)
        {
            button.openLobbyButton.ResetButtonText();
            button.openLobbyButton.Price = (int) lobby.openCustomLobbyPrice.OpenPrice;
            bool flag2 = lobby.Entity.HasComponent<OpenToConnectLobbyComponent>();
            if (lobby.Entity.HasComponent<SelfComponent>() && flag2)
            {
                button.openLobbyButton.LobbyId = lobby.Entity.Id;
                button.openLobbyButton.Price = -1;
            }
        }

        [OnEventFire]
        public void PlayScreenAdded(NodeAddedEvent e, PlayScreenNode screen, [JoinAll] SelfUserNode selfUser)
        {
            screen.playScreen.ConnectToBattleButton.SetActive(!selfUser.Entity.HasComponent<SquadGroupComponent>());
        }

        [OnEventFire]
        public void ShowCustomLobbyElements(NodeAddedEvent e, OpenLobbyButtonNode button, CustomLobbyNode lobby)
        {
            button.openLobbyButton.ResetButtonText();
            button.openLobbyButton.Price = (int) lobby.openCustomLobbyPrice.OpenPrice;
            bool flag2 = lobby.Entity.HasComponent<OpenToConnectLobbyComponent>();
            if (lobby.Entity.HasComponent<SelfComponent>() && flag2)
            {
                button.openLobbyButton.LobbyId = lobby.Entity.Id;
                button.openLobbyButton.Price = -1;
            }
        }

        [OnEventFire]
        public void SquadUserAdded(NodeAddedEvent e, SquadSelfUserNode selfUser, [JoinAll] PlayScreenNode screen)
        {
            screen.playScreen.ConnectToBattleButton.SetActive(false);
        }

        public class ConnectToButtonNode : Node
        {
            public ConnectToLobbyButtonComponent connectToLobbyButton;
        }

        public class CustomLobbyNode : Node
        {
            public OpenCustomLobbyPriceComponent openCustomLobbyPrice;
            public CustomBattleLobbyComponent customBattleLobby;
            public UserGroupComponent userGroup;
        }

        public class DialogNode : Node
        {
            public ConnectToLobbyDialogComponent connectToLobbyDialog;
            public InputFieldComponent inputField;
            public ESMComponent esm;
        }

        public class OpenDialogButtonNode : Node
        {
            public OpenConnectToLobbyDialogButtonComponent openConnectToLobbyDialogButton;
        }

        public class OpenLobbyButtonNode : Node
        {
            public OpenLobbyButtonComponent openLobbyButton;
        }

        public class OpenLobbyNode : ConnectToLobbyUiSystem.CustomLobbyNode
        {
            public OpenToConnectLobbyComponent openToConnectLobby;
        }

        public class PlayScreenNode : Node
        {
            public PlayScreenComponent playScreen;
        }

        public class SelfOpenLobbyNode : ConnectToLobbyUiSystem.OpenLobbyNode
        {
            public SelfComponent self;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserComponent user;
            public UserUidComponent userUid;
            public UserMoneyComponent userMoney;
        }

        public class SquadSelfUserNode : ConnectToLobbyUiSystem.SelfUserNode
        {
            public SquadGroupComponent squadGroup;
        }
    }
}

