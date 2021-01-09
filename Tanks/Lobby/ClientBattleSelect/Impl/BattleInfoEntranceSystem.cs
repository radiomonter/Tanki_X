namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BattleInfoEntranceSystem : ECSSystem
    {
        private bool CanEnter(BattleNode battle) => 
            battle.personalBattleInfo.Info.CanEnter;

        [OnEventFire]
        public void DisableBackButtonWhenLoadFail(EnterBattleRequestFailEvent e, SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<EnterBattleFailedEvent>(user);
        }

        [OnEventFire]
        public void EnableSpectatorButton(NodeAddedEvent e, SelectedNotArchivedBattleNode battle, [JoinAll] ScreenNode screen)
        {
            screen.battleSelectScreen.EnterBattleAsSpectatorButton.SetActive(true);
            screen.battleSelectScreen.EnterBattleAsSpectatorButton.SetInteractable(true);
        }

        [OnEventFire]
        public void EnterBattle(ButtonClickEvent e, BattleEnterButtonNode button, [JoinAll] SelectedBattleNode battle, [JoinAll] ScreenNode screen)
        {
            EnterBattleRequestEvent eventInstance = new EnterBattleRequestEvent(button.enterBattleButton.TeamColor) {
                Source = "BATTLES_LIST"
            };
            base.ScheduleEvent(eventInstance, battle);
            base.ScheduleEvent<EnterBattleAttemptEvent>(battle);
            this.LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void EnterBattleAsSpectator(ButtonClickEvent e, BattleEnterButtonAsSpectatorNode button, [JoinAll] SelectedBattleNode battle, [JoinAll] ScreenNode screen)
        {
            base.ScheduleEvent<EnterBattleAsSpectatorRequestEvent>(battle);
            base.ScheduleEvent<EnterBattleAttemptEvent>(battle);
            this.LockAllButtons(screen.battleSelectScreen);
        }

        private static GameObject GetTeamButton(TeamNode team, ScreenNode screen)
        {
            if (team.teamColor.TeamColor == TeamColor.RED)
            {
                return screen.battleSelectScreen.EnterBattleRedButton;
            }
            if (team.teamColor.TeamColor != TeamColor.BLUE)
            {
                throw new Exception("Team button not found: " + team.teamColor.TeamColor);
            }
            return screen.battleSelectScreen.EnterBattleBlueButton;
        }

        private void HideAllButtons(BattleSelectScreenComponent screenComponent)
        {
            this.LockAllButtons(screenComponent);
            screenComponent.EnterBattleDMButton.SetActive(false);
            screenComponent.EnterBattleRedButton.SetActive(false);
            screenComponent.EnterBattleBlueButton.SetActive(false);
            screenComponent.EnterBattleAsSpectatorButton.SetActive(false);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, ScreenNode screen)
        {
            this.HideAllButtons(screen.battleSelectScreen);
            this.LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void InitTeamButtonLock(NodeAddedEvent e, [Combine] TeamNode team, [JoinByBattle, Context] BattleNode battle, [JoinByBattle] ScreenNode screen)
        {
            GetTeamButton(team, screen).SetInteractable(!team.Entity.HasComponent<FullTeamComponent>() && this.CanEnter(battle));
        }

        private void LinkSpectatorButtonForNavigation(Selectable spectatorButton, Selectable up, Selectable down)
        {
            Navigation navigation = spectatorButton.navigation;
            navigation.selectOnDown = down;
            navigation.selectOnUp = up;
            spectatorButton.navigation = navigation;
        }

        private void LockAllButtons(BattleSelectScreenComponent screenComponent)
        {
            screenComponent.EnterBattleDMButton.SetInteractable(false);
            screenComponent.EnterBattleRedButton.SetInteractable(false);
            screenComponent.EnterBattleBlueButton.SetInteractable(false);
            screenComponent.EnterBattleAsSpectatorButton.SetInteractable(false);
        }

        [OnEventFire]
        public void LockDMButton(NodeAddedEvent e, SelectedArchivedBattleNode button, [JoinAll] ScreenNode screen)
        {
            this.HideAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void LockDMButton(NodeRemoveEvent e, SelectedNotFullDMNode battle, [JoinAll] ScreenNode screen)
        {
            screen.battleSelectScreen.EnterBattleDMButton.SetInteractable(false);
        }

        [OnEventFire]
        public void LockTeamButton(NodeAddedEvent e, FullTeamNode team, [JoinByBattle] ScreenNode screen)
        {
            GetTeamButton(team, screen).SetInteractable(false);
        }

        [OnEventFire]
        public void ResetButtons(NodeRemoveEvent e, SelectedBattleNode battle, [JoinAll] ScreenNode screen)
        {
            this.HideAllButtons(screen.battleSelectScreen);
            this.LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void ShowDMButton(NodeAddedEvent e, SelectedNotArchivedDMNode battle, [JoinAll] ScreenNode screen)
        {
            GameObject enterBattleDMButton = screen.battleSelectScreen.EnterBattleDMButton;
            enterBattleDMButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(enterBattleDMButton);
            this.LinkSpectatorButtonForNavigation(screen.battleSelectScreen.EnterBattleAsSpectatorButton.GetComponent<Selectable>(), enterBattleDMButton.GetComponent<Selectable>(), enterBattleDMButton.GetComponent<Selectable>());
        }

        [OnEventFire]
        public void ShowTeamButtons(NodeAddedEvent e, SelectedNotArchivedTeamNode battle, [JoinAll] ScreenNode screen)
        {
            screen.battleSelectScreen.EnterBattleRedButton.SetActive(true);
            screen.battleSelectScreen.EnterBattleBlueButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(screen.battleSelectScreen.EnterBattleBlueButton);
            this.LinkSpectatorButtonForNavigation(screen.battleSelectScreen.EnterBattleAsSpectatorButton.GetComponent<Selectable>(), screen.battleSelectScreen.EnterBattleRedButton.GetComponent<Selectable>(), screen.battleSelectScreen.EnterBattleBlueButton.GetComponent<Selectable>());
        }

        [OnEventFire]
        public void UnlockDMButton(NodeAddedEvent e, SelectedNotFullDMNode battle, [JoinAll] ScreenNode screen)
        {
            screen.battleSelectScreen.EnterBattleDMButton.SetInteractable(this.CanEnter(battle));
        }

        [OnEventFire]
        public void UnlockTeamButton(NodeRemoveEvent e, [Combine] FullTeamNode team, [JoinByBattle, Context] BattleNode battle, [JoinByBattle] ScreenNode screen)
        {
            GetTeamButton(team, screen).SetInteractable(this.CanEnter(battle));
        }

        public class BattleEnterButtonAsSpectatorNode : Node
        {
            public EnterBattleAsSpectatorButtonComponent enterBattleAsSpectatorButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class BattleEnterButtonNode : Node
        {
            public EnterBattleButtonComponent enterBattleButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
            public PersonalBattleInfoComponent personalBattleInfo;
        }

        public class FullTeamNode : BattleInfoEntranceSystem.TeamNode
        {
            public FullTeamComponent fullTeam;
        }

        public class ScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
        }

        public class SelectedArchivedBattleNode : BattleInfoEntranceSystem.BattleNode
        {
            public ArchivedBattleComponent archivedBattle;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedBattleNode : BattleInfoEntranceSystem.BattleNode
        {
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedNotArchivedBattleNode : BattleInfoEntranceSystem.SelectedBattleNode
        {
            public NotArchivedBattleComponent notArchivedBattle;
        }

        public class SelectedNotArchivedDMNode : BattleInfoEntranceSystem.SelectedNotArchivedBattleNode
        {
            public DMComponent dm;
        }

        public class SelectedNotArchivedTeamNode : BattleInfoEntranceSystem.SelectedNotArchivedBattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class SelectedNotFullDMNode : BattleInfoEntranceSystem.SelectedNotArchivedDMNode
        {
            public NotFullBattleComponent notFullBattle;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
            public TeamColorComponent teamColor;
        }
    }
}

