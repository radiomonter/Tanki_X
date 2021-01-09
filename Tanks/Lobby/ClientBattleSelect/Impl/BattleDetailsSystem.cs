namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNavigation.Impl;

    public class BattleDetailsSystem : ECSSystem
    {
        private long selectedBattleId = -1L;

        [OnEventFire]
        public void DelaySetDefaultText(NodeAddedEvent e, ScreenNode screen)
        {
            base.NewEvent<DelayedSetTopPanelTextEvent>().Attach(screen).ScheduleDelayed(screen.battleSelectScreenHeaderText.HeaderTextShowDelaySeconds);
        }

        [OnEventFire]
        public void HideIndicators(NodeRemoveEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators)
        {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(false);
            battleDetailsIndicators.battleDetailsIndicators.TimeIndicator.SetActive(false);
            battleDetailsIndicators.battleDetailsIndicators.LevelWarning.Hide();
            battleDetailsIndicators.battleDetailsIndicators.BattleLevelsIndicator.Hide();
            battleDetailsIndicators.battleDetailsIndicators.ArchivedBattleIndicator.SetActive(false);
        }

        [OnEventFire]
        public void SelectBattle(NodeAddedEvent e, SelectedBattleNode battle)
        {
            base.Log.DebugFormat("SelectBattle {0}", battle);
            base.ScheduleEvent<SelectBattleEvent>(battle);
            this.selectedBattleId = battle.Entity.Id;
        }

        [OnEventFire]
        public void SetDefaultText(DelayedSetTopPanelTextEvent e, ActiveScreenNode screen, [JoinAll, Mandatory] TopPanelNode topPanel)
        {
            if (string.IsNullOrEmpty(topPanel.topPanel.NewHeader))
            {
                topPanel.topPanel.SetHeaderTextImmediately(screen.battleSelectScreenHeaderText.HeaderText);
            }
        }

        [OnEventFire]
        public void SetMapNameText(NodeAddedEvent e, SelectedBattleNode battle, [JoinByMap, Mandatory] MapNode map)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent();
            eventInstance.Immediate(map.descriptionItem.Name + " " + battle.battleMode.BattleMode);
            base.ScheduleEvent(eventInstance, battle);
        }

        [OnEventFire]
        public void ShowArchivedBattleIndicator(NodeAddedEvent e, ArchivedBattleNode battle, [Context, JoinByBattle] ScreenWithBattleGroupNode screen, [Context, JoinByScreen] BattleDetailsIndicatorsNode indicators)
        {
            indicators.battleDetailsIndicators.LevelWarning.Hide();
            indicators.battleDetailsIndicators.ArchivedBattleIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowLevelWarning(NodeAddedEvent e, SelectedBattleWithInfoNode battle, [Context, JoinByBattle] ScreenWithBattleGroupNode screen, [Context, JoinByScreen] BattleDetailsIndicatorsNode indicators)
        {
            PersonalBattleInfo info = battle.personalBattleInfo.Info;
            BattleSelectScreenLocalizationComponent battleSelectScreenLocalization = screen.battleSelectScreenLocalization;
            string text = battleSelectScreenLocalization.BattleLevelsIndicatorText + battle.battleLevelRange.Range.Position;
            if (battle.battleLevelRange.Range.Position != battle.battleLevelRange.Range.EndPosition)
            {
                text = text + "-" + battle.battleLevelRange.Range.EndPosition;
            }
            indicators.battleDetailsIndicators.BattleLevelsIndicator.ShowText(text);
            if (!battle.Entity.HasComponent<ArchivedBattleComponent>())
            {
                if (!info.CanEnter)
                {
                    indicators.battleDetailsIndicators.LevelWarning.ShowText(battleSelectScreenLocalization.LevelErrorText);
                }
                else if (!info.InLevelRange)
                {
                    indicators.battleDetailsIndicators.LevelWarning.ShowText(battleSelectScreenLocalization.LevelWarningEquipDowngradedText);
                }
            }
        }

        [OnEventFire]
        public void ShowScoreIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context, JoinByScreen] ScreenWithBattleGroupNode screen, [Context, JoinByBattle] DMBattleWithScoreLimitNode battleDMWithScoreLimit)
        {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowScoreIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context, JoinByScreen] ScreenWithBattleGroupNode screen, [Context, JoinByBattle] TeamBattleNode teamBattle)
        {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowTimeIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context, JoinByScreen] ScreenWithBattleGroupNode screen, [Context, JoinByBattle] BattleWithTimeLimitNode battleDMWithTimeLimit)
        {
            battleDetailsIndicators.battleDetailsIndicators.TimeIndicator.SetActive(true);
        }

        [OnEventFire]
        public void UnselectBattle(NodeRemoveEvent e, SelectedBattleNode battle)
        {
            bool flag = this.selectedBattleId != battle.Entity.Id;
            base.Log.DebugFormat("UnselectBattle {0} skip={1}", battle, flag);
            if (!flag)
            {
                base.ScheduleEvent<UnselectBattleEvent>(battle);
                this.selectedBattleId = -1L;
            }
        }

        public class ActiveScreenNode : BattleDetailsSystem.ScreenNode
        {
            public ActiveScreenComponent activeScreen;
        }

        public class ArchivedBattleNode : BattleDetailsSystem.SelectedBattleNode
        {
            public ArchivedBattleComponent archivedBattle;
        }

        public class BattleDetailsIndicatorsNode : Node
        {
            public BattleDetailsIndicatorsComponent battleDetailsIndicators;
            public ScreenGroupComponent screenGroup;
        }

        public class BattleWithTimeLimitNode : BattleDetailsSystem.SelectedBattleNode
        {
            public TimeLimitComponent timeLimit;
        }

        public class DelayedSetTopPanelTextEvent : Event
        {
        }

        public class DMBattleWithScoreLimitNode : BattleDetailsSystem.SelectedBattleNode
        {
            public DMComponent dm;
            public ScoreLimitComponent scoreLimit;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public DescriptionItemComponent descriptionItem;
        }

        public class ScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public BattleSelectScreenHeaderTextComponent battleSelectScreenHeaderText;
            public ScreenGroupComponent screenGroup;
        }

        public class ScreenWithBattleGroupNode : BattleDetailsSystem.ScreenNode
        {
            public BattleGroupComponent battleGroup;
            public BattleSelectScreenLocalizationComponent battleSelectScreenLocalization;
        }

        public class SelectedBattleNode : Node
        {
            public BattleComponent battle;
            public BattleModeComponent battleMode;
            public BattleGroupComponent battleGroup;
            public SelectedListItemComponent selectedListItem;
            public MapGroupComponent mapGroup;
            public BattleLevelRangeComponent battleLevelRange;
            public BattleConfiguredComponent battleConfigured;
        }

        public class SelectedBattleWithInfoNode : BattleDetailsSystem.SelectedBattleNode
        {
            public PersonalBattleInfoComponent personalBattleInfo;
        }

        public class TeamBattleNode : BattleDetailsSystem.SelectedBattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class TopPanelNode : Node
        {
            public TopPanelComponent topPanel;
        }
    }
}

