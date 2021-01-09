namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityStandardAssets.ImageEffects;

    public class BattleScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ExitBattle(NodeRemoveEvent e, SingleNode<BattleScreenComponent> battleScreen, [JoinAll] SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            base.ScheduleEvent<ExitBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void ExitBattleOnLeaveLoad(NodeRemoveEvent e, SingleNode<BattleLoadScreenComponent> battleLoadScreen, [JoinAll] NotReadySelfBattleUserNode selfBattleUser)
        {
            base.ScheduleEvent<ExitBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void ExitBattleOnLoading(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<BattleLoadScreenComponent> battleLoadScreen)
        {
            base.ScheduleEvent<GoBackFromBattleEvent>(selfBattleUser);
        }

        [OnEventFire]
        public void GoBack(GoBackFromBattleEvent e, Node any)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<ExitBattleToLobbyLoadScreenComponent>>(any);
        }

        [OnEventFire]
        public void GoBackOnKick(KickFromBattleEvent e, SelfBattleUserNode battleUser, [JoinByBattle] SingleNode<RoundComponent> round, [JoinAll] Optional<SingleNode<BattleResultsComponent>> battleResults)
        {
            if (!battleResults.IsPresent())
            {
                base.ScheduleEvent<GoBackFromBattleEvent>(battleUser);
            }
        }

        [OnEventFire]
        public void GroupScoreTable(NodeAddedEvent e, ReadyBattleUserCommon battleUser, ScreenInitNode screen)
        {
            screen.Entity.AddComponent(new BattleGroupComponent(battleUser.battleGroup.Key));
        }

        [OnEventFire]
        public void HideCursor(NodeAddedEvent e, SingleNode<BattleScreenComponent> battleLoadScreen, [JoinAll] UserAsTank selfBattleUserAsTank)
        {
            base.ScheduleEvent<BattleFullyLoadedEvent>(selfBattleUserAsTank);
        }

        private bool IsDeserter(BattleNode battle, SelfBattleUserNode battleUser, ICollection<BattleUserAsTankNode> battleUsers)
        {
            Entity entity = battle.Entity;
            Entity entity2 = battleUser.Entity;
            if (entity.HasComponent<DMComponent>())
            {
                return (battleUsers.Count > 1);
            }
            if (!entity.HasComponent<TeamBattleComponent>())
            {
                return false;
            }
            int num = 0;
            foreach (BattleUserAsTankNode node in battleUsers)
            {
                Entity otherEntity = node.Entity;
                if (!entity2.IsSameGroup<TeamGroupComponent>(otherEntity) && !otherEntity.HasComponent<TankAutopilotComponent>())
                {
                    num++;
                }
            }
            return (num > 0);
        }

        [OnEventFire]
        public void OnRequestGoBack(RequestGoBackFromBattleEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ActiveBattleScreenNode screen, [JoinAll] SelfBattleNonSpectatorNode battleUser, [JoinByBattle] SingleNode<RoundActiveStateComponent> activeRound, [JoinByBattle] BattleNode battle, [JoinByBattle] ICollection<BattleUserAsTankNode> battleUsers, [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customLobby, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode)
        {
            bool isDeserter = this.IsDeserter(battle, battleUser, battleUsers);
            dialogs.component.Get<ExitBattleWindow>().Show(screen.Entity, battleUser.Entity, customLobby.IsPresent(), isDeserter, chosenMode.IsPresent() && chosenMode.Get().component.ModeEntity.HasComponent<MatchMakingDefaultModeComponent>());
        }

        [OnEventFire]
        public void ShowBackgroundAndLoadHangar(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round)
        {
            if (Camera.main)
            {
                BlurOptimized component = Camera.main.gameObject.GetComponent<BlurOptimized>();
                if (component != null)
                {
                    component.enabled = true;
                    component.blurSize = 3f;
                }
            }
        }

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser, [Context, JoinByBattle] DMBattleNode dmBattle)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowBattleScreen(NodeAddedEvent e, ReadyBattleUser battleUser, [Context, JoinByBattle] TeamBattleNode teamBattle)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowDMBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser, [JoinByBattle] SingleNode<DMComponent> dmBattle)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<DMBattleScpectatorScreenComponent>>(battleUser);
        }

        [OnEventFire]
        public void ShowGarage(NodeAddedEvent e, SingleNode<ExitBattleToLobbyLoadScreenComponent> screen, UserReadyForLobbyNode user)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(user);
            base.ScheduleEvent<GoBackFromBattleScreensEvent>(screen);
        }

        [OnEventFire]
        public void ShowGoBackRequest(SpectatorGoBackRequestEvent e, Node anyNode, [JoinAll] FreeCameraNode camera, [JoinAll] SelfBattleUserNode battleUser, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ActiveBattleScreenNode screen, [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customLobby)
        {
            dialogs.component.Get<ExitBattleWindow>().Show(screen.Entity, battleUser.Entity, customLobby.IsPresent(), false, false);
        }

        [OnEventFire]
        public void ShowResultTitle(NodeAddedEvent e, SingleNode<BattleResultsComponent> results, [JoinAll] SingleNode<MainHUDComponent> hud, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            hud.component.Hide();
            BattleResultForClient resultForClient = results.component.ResultForClient;
            if (resultForClient.PersonalResult != null)
            {
                if (resultForClient.BattleMode == BattleMode.DM)
                {
                    dialogs.component.Get<DMFinishWindow>().Show();
                }
                else
                {
                    TeamFinishWindow window = dialogs.component.Get<TeamFinishWindow>();
                    window.CustomBattle = resultForClient.Custom;
                    if (resultForClient.PersonalResult.TeamBattleResult == TeamBattleResult.DRAW)
                    {
                        window.ShowTie();
                    }
                    else if (resultForClient.PersonalResult.TeamBattleResult == TeamBattleResult.WIN)
                    {
                        window.ShowWin();
                    }
                    else
                    {
                        window.ShowLose();
                    }
                }
            }
        }

        [OnEventFire]
        public void ShowTeamBattleSpectatorScreen(NodeAddedEvent e, ReadySpectator battleUser, [JoinByBattle] SingleNode<TeamBattleComponent> teamBattle)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<TeamBattleSpectatorScreenComponent>>(battleUser);
        }

        [Not(typeof(BattleLoadScreenComponent))]
        public class ActiveBattleScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class BattleUserAsTankNode : Node
        {
            public BattleUserComponent battleUser;
            public BattleGroupComponent battleGroup;
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class DMBattleNode : Node
        {
            public DMComponent dm;
            public BattleGroupComponent battleGroup;
        }

        public class FreeCameraNode : Node
        {
            public SpectatorCameraComponent spectatorCamera;
            public FreeCameraComponent freeCamera;
        }

        [Not(typeof(UserReadyToBattleComponent))]
        public class NotReadySelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
        }

        public class ReadyBattleUser : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public BattleGroupComponent battleGroup;
        }

        public class ReadyBattleUserCommon : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadySpectator : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class ScreenInitNode : Node
        {
            public ScreenComponent screen;
            public BattleScreenComponent battleScreen;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfBattleNonSpectatorNode : BattleScreenSystem.SelfBattleUserNode
        {
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleGroupComponent battleGroup;
        }

        public class TeamBattleNode : Node
        {
            public TeamBattleComponent teamBattle;
            public BattleGroupComponent battleGroup;
        }

        public class UserAsTank : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class UserReadyForLobbyNode : Node
        {
            public SelfUserComponent selfUser;
            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}

