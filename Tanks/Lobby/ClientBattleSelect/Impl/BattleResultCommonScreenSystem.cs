namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientQuests.API;
    using Tanks.Lobby.ClientQuests.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine.UI;

    public class BattleResultCommonScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<QuestNode, bool> <>f__am$cache0;

        [OnEventFire]
        public void BattleResultRemoved(NodeRemoveEvent e, SingleNode<BattleResultCommonUIComponent> battleResultUI)
        {
            base.ScheduleEvent<ClearBattleResultTankEvent>(EngineService.EntityStub);
        }

        [OnEventFire]
        public void BuildBestPlayerTank(BuildBestPlayerTankEvent e, Node any, [JoinAll] SingleNode<BattleResultsComponent> battleResults, [JoinAll] SingleNode<BattleResultCommonUIComponent> screen)
        {
            UserResult mvp = this.FindMostValuablePlayer(battleResults.component.ResultForClient);
            this.BuildTank(mvp, true, screen.component.tankPreviewImage1);
        }

        [OnEventFire]
        public void BuildSelfTank(BuildSelfPlayerTankEvent e, Node any, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<BattleResultsComponent> battleResults, [JoinAll] SingleNode<BattleResultCommonUIComponent> screen)
        {
            UserResult mvp = battleResults.component.ResultForClient.FindUserResultByUserId(user.Entity.Id);
            this.BuildTank(mvp, false, screen.component.tankPreviewImage2);
        }

        private void BuildTank(UserResult mvp, bool bestPlayerScreen, Image image)
        {
            BuildBattleResultTankEvent eventInstance = new BuildBattleResultTankEvent {
                HullGuid = GetHullGuid(mvp),
                WeaponGuid = GetTurretGuid(mvp),
                PaintGuid = GetPaintGuid(mvp),
                CoverGuid = GetCoverGuid(mvp),
                BestPlayerScreen = bestPlayerScreen
            };
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            SetImage(image, eventInstance);
        }

        [OnEventFire]
        public void CreateResultsEntity(BattleResultForClientEvent e, SelfUserNode user, [JoinAll] ICollection<SingleNode<BattleResultsComponent>> otherResults)
        {
            this.UpdateResultEntity(e.UserResultForClient, otherResults);
        }

        [OnEventFire]
        public void DelayShowBackgroundAndLoadHangar(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [Mandatory, JoinAll] SelfUserNode user)
        {
            base.NewEvent<GoBackFromBattleWithResultsEvent>().Attach(user).ScheduleDelayed(4f);
            base.NewEvent<LoadHangarEvent>().Attach(user).ScheduleDelayed(3.5f);
        }

        [OnEventFire]
        public void FillMVPScreen(NodeAddedEvent e, SingleNode<MVPScreenUIComponent> screen, SingleNode<BattleResultsComponent> battleResults, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] SelfUserNode user)
        {
            screen.component.SetModuleConfig(moduleUpgradeConfig.moduleUpgradablePowerConfig);
            UserResult mvp = this.FindMostValuablePlayer(battleResults.component.ResultForClient);
            bool mvpIsPlayer = false;
            if (!battleResults.component.ResultForClient.Spectator)
            {
                UserResult result2 = battleResults.component.ResultForClient.FindUserResultByUserId(user.Entity.Id);
                mvpIsPlayer = mvp.UserId == result2.UserId;
            }
            screen.component.SetResults(mvp, battleResults.component.ResultForClient, mvpIsPlayer);
        }

        private UserResult FindMostValuablePlayer(BattleResultForClient battleResults) => 
            (battleResults.DmUsers.Count <= 0) ? ((battleResults.RedUsers.Count != 0) ? ((battleResults.BlueUsers.Count != 0) ? ((battleResults.RedUsers[0].ScoreWithoutPremium <= battleResults.BlueUsers[0].ScoreWithoutPremium) ? battleResults.BlueUsers[0] : battleResults.RedUsers[0]) : battleResults.RedUsers[0]) : battleResults.BlueUsers[0]) : battleResults.DmUsers[0];

        private static string GetCoverGuid(UserResult mvp) => 
            GetItemGuid(mvp.CoatingId);

        private static string GetHullGuid(UserResult mvp) => 
            GetItemGuid(mvp.HullSkinId);

        private static string GetItemGuid(long marketItemId) => 
            GarageItemsRegistry.GetItem<GarageItem>(marketItemId).AssertGuid;

        private static string GetPaintGuid(UserResult mvp) => 
            GetItemGuid(mvp.PaintId);

        private static string GetTurretGuid(UserResult mvp) => 
            GetItemGuid(mvp.WeaponSkinId);

        [OnEventFire]
        public void OnContinue(ButtonClickEvent e, SingleNode<ContinueBattleButtonComponent> button)
        {
            base.ScheduleEvent<GoBackRequestEvent>(button);
        }

        [OnEventFire]
        public void RemoveResults(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] ICollection<SingleNode<BattleResultsComponent>> otherResults)
        {
            foreach (SingleNode<BattleResultsComponent> node in otherResults)
            {
                base.DeleteEntity(node.Entity);
            }
        }

        [OnEventFire]
        public void ScreenInit(NodeAddedEvent e, SingleNode<BattleResultCommonUIComponent> battleResultScreenUI, [JoinAll] SelfUserNode selfUserNode, [JoinAll] SingleNode<BattleResultsComponent> results)
        {
            BattleResultForClient resultForClient = results.component.ResultForClient;
            if (resultForClient.Spectator)
            {
                battleResultScreenUI.component.ShowScreen(resultForClient.Custom, true, false, false, false);
            }
            else
            {
                bool flag = resultForClient.PersonalResult.MaxEnergySource == EnergySource.MVP_BONUS;
                bool flag2 = (resultForClient.PersonalResult.MaxEnergySource == EnergySource.UNFAIR_MM) || (resultForClient.PersonalResult.MaxEnergySource == EnergySource.DISBALANCE_BONUS);
                GetBattleTypeEvent eventInstance = new GetBattleTypeEvent {
                    WithCashback = flag || flag2
                };
                base.ScheduleEvent(eventInstance, battleResultScreenUI);
                bool tutor = (selfUserNode.userStatistics.Statistics["ALL_BATTLES_PARTICIPATED"] <= 4L) || (eventInstance.BattleType == BattleResultsAwardsScreenComponent.BattleTypes.Tutorial);
                battleResultScreenUI.component.ShowScreen(resultForClient.Custom, false, tutor, selfUserNode.Entity.HasComponent<SquadGroupComponent>(), true);
            }
        }

        private static void SetImage(Image image, BuildBattleResultTankEvent buildEvent)
        {
            image.material.SetTexture("_MainTex", buildEvent.tankPreviewRenderTexture);
            image.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void ShowQuestWindowAfterBattleFinishScreen(ShowQuestProgressIfNeedEvent e, Node any, [JoinAll] SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<WindowsSpaceComponent> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ICollection<QuestNode> quests)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = q => !q.questProgress.PrevValue.Equals(q.questProgress.CurrentValue);
            }
            if (quests.Count<QuestNode>(<>f__am$cache0) > 0)
            {
                QuestWindowComponent component = dialogs.component.Get<QuestWindowComponent>();
                component.ShowOnMainScreen = false;
                component.ShowProgress = true;
                component.Show(screens.component.Animators);
            }
        }

        [OnEventFire]
        public void ShowResultsScreen(GoBackFromBattleWithResultsEvent e, SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<BattleResultScreenComponent>>(EngineService.EntityStub);
        }

        [OnEventFire]
        public void ToMainScreen(ButtonClickEvent e, SingleNode<ToMainScreenBattleButtonComponent> button)
        {
            MainScreenComponent.Instance.ShowHome();
            base.ScheduleEvent<ShowQuestProgressIfNeedEvent>(button);
        }

        private void UpdateResultEntity(BattleResultForClient result, ICollection<SingleNode<BattleResultsComponent>> otherResults)
        {
            foreach (SingleNode<BattleResultsComponent> node in otherResults)
            {
                base.DeleteEntity(node.Entity);
            }
            BattleResultsComponent component = new BattleResultsComponent {
                ResultForClient = result
            };
            base.CreateEntity("BattleResults").AddComponent(component);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueIconComponent leagueIcon;
            public ChestBattleRewardComponent chestBattleReward;
        }

        public class ModuleUpgradeConfigNode : Node
        {
            public ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig;
        }

        public class QuestNode : Node
        {
            public QuestProgressComponent questProgress;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserStatisticsComponent userStatistics;
        }

        public class ShowQuestProgressIfNeedEvent : Event
        {
        }
    }
}

