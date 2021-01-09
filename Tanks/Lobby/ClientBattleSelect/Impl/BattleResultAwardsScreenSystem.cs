namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
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
    using Tanks.Lobby.ClientUserProfile.API;

    public class BattleResultAwardsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachScreenToUserGroup(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelfUserNode selfUser)
        {
            selfUser.userGroup.Attach(screen.Entity);
        }

        [OnEventComplete]
        public void ContinueOnProgressShow(ShowBattleResultsScreenNotificationEvent e, Node any, [JoinAll] ScreenNode screen)
        {
            if (!e.NotificationExist)
            {
                screen.battleResultAwardsScreenAnimation.playActions = true;
            }
        }

        [OnEventComplete]
        public void GetBattleType(GetBattleTypeEvent e, Node any, [JoinAll] ResultsNode results, [JoinAll] ICollection<SingleNode<TutorialSetupEndgameScreenHandler>> tutorialHandlers)
        {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            BattleResultsAwardsScreenComponent.BattleTypes none = BattleResultsAwardsScreenComponent.BattleTypes.None;
            if (resultForClient.Custom)
            {
                none = BattleResultsAwardsScreenComponent.BattleTypes.Custom;
            }
            else if (tutorialHandlers.Count > 0)
            {
                foreach (SingleNode<TutorialSetupEndgameScreenHandler> node in tutorialHandlers)
                {
                    node.component.gameObject.SetActive(false);
                }
                none = BattleResultsAwardsScreenComponent.BattleTypes.Tutorial;
            }
            else
            {
                BattleType matchMakingModeType = resultForClient.MatchMakingModeType;
                if (matchMakingModeType == BattleType.ENERGY)
                {
                    none = BattleResultsAwardsScreenComponent.BattleTypes.Quick;
                }
                else if (matchMakingModeType == BattleType.ARCADE)
                {
                    none = BattleResultsAwardsScreenComponent.BattleTypes.Arcade;
                }
                else if (matchMakingModeType == BattleType.RATING)
                {
                    none = !e.WithCashback ? BattleResultsAwardsScreenComponent.BattleTypes.Ranked : BattleResultsAwardsScreenComponent.BattleTypes.RankedWithCashback;
                }
            }
            e.BattleType = none;
        }

        [OnEventFire]
        public void OpenGameplayChest(ButtonClickEvent e, SingleNode<OpenGameplayChestButtonComponent> button, [JoinAll] UserNode user, [JoinByLeague] LeagueNode league, [JoinAll] ScreenNode screen)
        {
            screen.battleResultsAwardsScreen.openChestButton.SetActive(false);
            Entity entity = Flow.Current.EntityRegistry.GetEntity(league.chestBattleReward.ChestId);
            GamePlayChestItemNode node = base.Select<GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault<GamePlayChestItemNode>();
            if ((node != null) && (node.userItemCounter.Count != 0L))
            {
                OpenContainerEvent eventInstance = new OpenContainerEvent {
                    Amount = node.userItemCounter.Count
                };
                base.ScheduleEvent(eventInstance, node);
            }
        }

        [OnEventFire]
        public void PutReputationToEnter(UpdateTopLeagueInfoEvent e, SelfUserNode user, [JoinAll] ScreenNode screen, [JoinAll] TopLeagueNode topLeague, [JoinAll] ResultsNode results)
        {
            if (!results.battleResults.ResultForClient.Spectator && screen.battleResultsAwardsScreen.CanShowLeagueProgress())
            {
                screen.battleResultsAwardsScreen.leagueResultUI.PutReputationToEnter(topLeague.Entity.Id, e.LastPlaceReputation);
            }
        }

        [OnEventFire]
        public void QuickGameCheck(CheckForQuickGameEvent e, Node any, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode)
        {
            if (chosenMode.IsPresent())
            {
                e.IsQuickGame = chosenMode.Get().component.ModeEntity.HasComponent<MatchMakingEnergyModeComponent>();
            }
        }

        [OnEventFire]
        public void ScreenInit(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<QuestNode> quests)
        {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            bool flag = personalResult.MaxEnergySource == EnergySource.MVP_BONUS;
            bool flag2 = (personalResult.MaxEnergySource == EnergySource.UNFAIR_MM) || (personalResult.MaxEnergySource == EnergySource.DISBALANCE_BONUS);
            GetBattleTypeEvent eventInstance = new GetBattleTypeEvent {
                WithCashback = flag || flag2
            };
            base.ScheduleEvent(eventInstance, screen);
            screen.battleResultsAwardsScreen.SetBattleType(eventInstance.BattleType);
            int userDMPlace = (resultForClient.BattleMode != BattleMode.DM) ? 0 : (resultForClient.DmUsers.IndexOf(resultForClient.FindUserResultByUserId(selfUser.Entity.Id)) + 1);
            this.ShowTitle(screen, results, userDMPlace);
            this.ShowReputation(screen, results, selfUser);
        }

        [OnEventFire]
        public void ScreenPartShown(ScreenPartShownEvent e, Node any, [JoinAll] ScreenNode screen, [JoinAll] UserNode user)
        {
            ShowBattleResultsScreenNotificationEvent eventInstance = new ShowBattleResultsScreenNotificationEvent {
                Index = 1
            };
            base.NewEvent(eventInstance).Attach(any).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void ShowExp(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results, [JoinAll] SelfUserNode selfUser, [JoinByLeague] LeagueNode league, [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] RankNamesNode rankNames, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig)
        {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            UserResult result = resultForClient.FindUserResultByUserId(selfUser.Entity.Id);
            int rank = selfUser.userRank.Rank;
            int index = rank - 2;
            int initValue = ((index < 0) || (index >= ranksExperiencesConfig.component.RanksExperiences.Length)) ? 0 : ranksExperiencesConfig.component.RanksExperiences[index];
            int num4 = rank - 1;
            int maxValue = ((num4 < 0) || (num4 >= ranksExperiencesConfig.component.RanksExperiences.Length)) ? 0 : ranksExperiencesConfig.component.RanksExperiences[num4];
            screen.battleResultsAwardsScreen.ShowRankProgress(initValue, personalResult.RankExp, maxValue, personalResult.RankExpDelta, result.ScoreWithoutPremium, rank, rankNames.ranksNames.Names);
            Entity entity = Flow.Current.EntityRegistry.GetEntity(league.chestBattleReward.ChestId);
            GamePlayChestItemNode node = base.Select<GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault<GamePlayChestItemNode>();
            screen.battleResultsAwardsScreen.openChestButton.SetActive((node != null) && (node.userItemCounter.Count != 0L));
            screen.battleResultsAwardsScreen.ShowContainerProgress(personalResult.ContainerScore, personalResult.ContainerScoreDelta, result.ScoreWithoutPremium, personalResult.ContainerScoreLimit, ((personalResult.Container == null) || !personalResult.Container.HasComponent<ImageItemComponent>()) ? string.Empty : personalResult.Container.GetComponent<ImageItemComponent>().SpriteUid);
            screen.battleResultsAwardsScreen.SetTankInfo(result.HullId, result.WeaponId, result.Modules, moduleUpgradeConfig.moduleUpgradablePowerConfig);
            screen.battleResultsAwardsScreen.SetHullExp(personalResult.TankInitExp, personalResult.TankExp, personalResult.TankFinalExp, personalResult.ItemsExpDelta, result.ScoreWithoutPremium, personalResult.TankLevel);
            screen.battleResultsAwardsScreen.SetTurretExp(personalResult.WeaponInitExp, personalResult.WeaponExp, personalResult.WeaponFinalExp, personalResult.ItemsExpDelta, result.ScoreWithoutPremium, personalResult.WeaponLevel);
        }

        public void ShowReputation(ScreenNode screen, ResultsNode results, SelfUserNode user)
        {
            if (!screen.battleResultsAwardsScreen.CanShowLeagueProgress())
            {
                screen.battleResultsAwardsScreen.HideLeagueProgress();
            }
            else
            {
                screen.battleResultsAwardsScreen.ShowLeagueProgress();
                LeagueResultUI leagueResultUI = screen.battleResultsAwardsScreen.leagueResultUI;
                BattleResultForClient resultForClient = results.battleResults.ResultForClient;
                PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
                Entity prevLeague = personalResult.PrevLeague;
                Entity league = personalResult.League;
                double reputation = personalResult.Reputation;
                double reputationDelta = personalResult.ReputationDelta;
                bool topLeague = league.HasComponent<TopLeagueComponent>();
                int leaguePlace = personalResult.LeaguePlace;
                bool unfairMatching = resultForClient.FindUserResultByUserId(user.Entity.Id).UnfairMatching;
                if (!ReferenceEquals(prevLeague, league))
                {
                    leagueResultUI.SetPreviousLeague(prevLeague);
                }
                leagueResultUI.SetCurrentLeague(league, reputation, (long) leaguePlace, topLeague, reputationDelta, unfairMatching);
                leagueResultUI.DealWithReputationChange();
                if (!ReferenceEquals(prevLeague, league))
                {
                    leagueResultUI.ShowNewLeague();
                }
            }
        }

        public void ShowTitle(ScreenNode screen, ResultsNode results, int userDMPlace)
        {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            BattleMode battleMode = resultForClient.BattleMode;
            screen.battleResultsAwardsScreen.SetupHeader(battleMode, resultForClient.MatchMakingModeType, resultForClient.PersonalResult.TeamBattleResult, Flow.Current.EntityRegistry.GetEntity(resultForClient.MapId).GetComponent<DescriptionItemComponent>().Name, userDMPlace);
        }

        [OnEventFire]
        public void ShowTutorialRewards(ShowTutorialRewardsEvent e, TutorialStepWithRewardsNode tutorialStepWithRewards, [JoinAll] ScreenNode screen)
        {
            List<SpecialOfferItem> items = new List<SpecialOfferItem>();
            foreach (Reward reward in tutorialStepWithRewards.tutorialRewardData.Rewards)
            {
                GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(reward.ItemId);
                if (item != null)
                {
                    items.Add(new SpecialOfferItem((int) reward.Count, item.Preview, item.Name));
                }
            }
            long crysCount = tutorialStepWithRewards.tutorialRewardData.CrysCount;
            if (crysCount > 0L)
            {
                items.Add(new SpecialOfferItem((int) crysCount, screen.battleResultsAwardsScreen.crysImageSkin.SpriteUid, screen.battleResultsAwardsScreen.crysLocalizedField.Value));
            }
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(screen.battleResultsAwardsScreen.tutorialCongratulationLocalizedField.Value, tutorialStepWithRewards.tutorialStepData.Message, items);
            specialOfferUI.SetTutorialRewardsButton();
            specialOfferUI.Appear();
        }

        [OnEventFire]
        public void TutorialsTriggered(GetBattleTypeEvent e, Node any, [JoinAll, Combine] SingleNode<TutorialEndGameTriggerComponent> tutorialTrigger)
        {
            tutorialTrigger.component.GetComponent<TutorialShowTriggerComponent>().Triggered();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class GamePlayChestItemNode : Node
        {
            public GameplayChestItemComponent gameplayChestItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

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

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RankNamesNode : Node
        {
            public RanksNamesComponent ranksNames;
        }

        public class ResultsNode : Node
        {
            public BattleResultsComponent battleResults;
        }

        public class ScreenNode : Node
        {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
            public BattleResultAwardsScreenAnimationComponent battleResultAwardsScreenAnimation;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserStatisticsComponent userStatistics;
            public UserRankComponent userRank;
            public UserExperienceComponent userExperience;
            public UserGroupComponent userGroup;
        }

        public class TopLeagueNode : Node
        {
            public TopLeagueComponent topLeague;
        }

        public class TutorialStepWithRewardsNode : Node
        {
            public TutorialStepDataComponent tutorialStepData;
            public TutorialGroupComponent tutorialGroup;
            public TutorialRewardDataComponent tutorialRewardData;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public LeagueGroupComponent leagueGroup;
            public UserReputationComponent userReputation;
            public UserLeaguePlaceComponent userLeaguePlace;
            public GameplayChestScoreComponent gameplayChestScore;
            public EarnedGameplayChestScoreComponent earnedGameplayChestScore;
        }
    }
}

