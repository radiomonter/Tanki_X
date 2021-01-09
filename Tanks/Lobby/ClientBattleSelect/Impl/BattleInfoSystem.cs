namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class BattleInfoSystem : ECSSystem
    {
        [OnEventFire, Mandatory]
        public void AddInfo(PersonalBattleInfoEvent e, SingleNode<BattleComponent> battle)
        {
            if (battle.Entity.HasComponent<PersonalBattleInfoComponent>())
            {
                battle.Entity.GetComponent<PersonalBattleInfoComponent>().Info = e.Info;
            }
            else
            {
                PersonalBattleInfoComponent component = new PersonalBattleInfoComponent {
                    Info = e.Info
                };
                battle.Entity.AddComponent(component);
            }
        }

        [OnEventFire]
        public void AddVisibleItem(NodeAddedEvent e, BattleNode battle, [JoinAll] ScreenNode screen)
        {
            if (screen.visibleItemsRange.Range.Contains(battle.searchData.IndexInSearchResult))
            {
                base.Log.InfoFormat("AddVisibleItem {0}", battle.Entity.Id);
                battle.Entity.AddComponent<VisibleItemComponent>();
            }
        }

        [OnEventFire]
        public void HideScore(NodeAddedEvent e, BattleScoreWithoutLimitNode battle)
        {
            battle.battleItemContent.HideScore();
        }

        [OnEventFire]
        public void HideTime(NodeAddedEvent e, BattleTimeWithoutLimitNode battle)
        {
            battle.battleItemContent.HideTime();
        }

        [OnEventFire]
        public void ReplaceIcon(NodeAddedEvent e, CTFBattleNode battle)
        {
            battle.battleItemContent.SetFlagAsScoreIcon();
        }

        [OnEventFire]
        public void RequestImage(NodeAddedEvent e, VisibleBattleNode battle, [JoinByMap] SingleNode<MapPreviewComponent> map, [JoinAll] ScreenNode screen)
        {
            base.Log.InfoFormat("RequestImage {0}", battle);
            RectTransform item = screen.lazyList.GetItem(battle.searchData.IndexInSearchResult);
            EntityBehaviour behaviour = Object.Instantiate<EntityBehaviour>(screen.battleSelectScreen.ItemContentPrefab);
            screen.lazyList.SetItemContent(battle.searchData.IndexInSearchResult, behaviour.GetComponent<RectTransform>());
            behaviour.BuildEntity(battle.Entity);
            screen.lazyList.UpdateSelection(battle.searchData.IndexInSearchResult);
            AssetRequestEvent eventInstance = new AssetRequestEvent();
            eventInstance.Init<MapPreviewDataComponent>(map.component.AssetGuid);
            base.ScheduleEvent(eventInstance, battle);
        }

        [OnEventFire]
        public void SetArchived(NodeAddedEvent e, ArchivedBattleNode battle)
        {
            battle.battleItemContent.EntranceLocked = true;
        }

        [OnEventFire]
        public void SetImage(NodeAddedEvent e, BattleWithPreviewDataNode battle, [JoinAll] ScreenNode screen)
        {
            base.Log.InfoFormat("SetImage {0}", battle.mapPreviewData.Data);
            battle.battleItemContent.SetPreview((Texture2D) battle.mapPreviewData.Data);
        }

        [OnEventFire]
        public void SetVisibleItems(ItemsVisibilityChangedEvent e, ScreenNode screen, [JoinAll] ICollection<BattleNode> battles)
        {
            screen.Entity.RemoveComponent<VisibleItemsRangeComponent>();
            screen.Entity.AddComponent(new VisibleItemsRangeComponent(e.Range));
            foreach (BattleNode node in battles)
            {
                int indexInSearchResult = node.searchData.IndexInSearchResult;
                IndexRange range = e.Range;
                if (!range.Contains(indexInSearchResult) && e.PrevRange.Contains(indexInSearchResult))
                {
                    base.Log.InfoFormat("RemoveVisibleItem {0}", node.Entity.Id);
                    node.Entity.RemoveComponent<VisibleItemComponent>();
                    continue;
                }
                if (e.Range.Contains(indexInSearchResult) && !e.PrevRange.Contains(indexInSearchResult))
                {
                    base.Log.InfoFormat("AddVisibleItem {0}", node.Entity.Id);
                    node.Entity.AddComponent<VisibleItemComponent>();
                }
            }
        }

        [OnEventFire]
        public void Update(UpdateEvent e, BattleWithViewNode battle)
        {
            battle.battleItemContent.SetModeField(battle.battleMode.BattleMode.ToString());
            battle.battleItemContent.SetUserCountField(battle.userCount.UserCount + " / " + battle.userLimit.UserLimit);
        }

        [OnEventFire, Conditional("DEBUG")]
        public void Update(UpdateEvent e, ScreenNode screen, [JoinAll] ICollection<BattleWithViewNode> battles)
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                screen.battleSelectScreen.DebugEnabled = !screen.battleSelectScreen.DebugEnabled;
            }
            foreach (BattleWithViewNode node in battles)
            {
                string text = string.Empty;
                if (screen.battleSelectScreen.DebugEnabled)
                {
                    SearchDataComponent searchData = node.searchData;
                    BattleEntry battleEntry = searchData.BattleEntry;
                    text = $"id={node.Entity.Id}
index={searchData.IndexInSearchResult}
relevance={battleEntry.Relevance}
friends={battleEntry.FriendsInBattle}
server={battleEntry.Server}
lobby={battleEntry.LobbyServer}";
                }
                node.battleItemContent.SetDebugField(text);
            }
        }

        [OnEventFire]
        public void UpdateTime(UpdateEvent e, BattleTimeWithLimitNode battle, [JoinByBattle] RoundNode round)
        {
            float timeLimitSec = battle.timeLimit.TimeLimitSec;
            if (battle.Entity.HasComponent<BattleStartTimeComponent>() && !round.Entity.HasComponent<RoundWarmingUpStateComponent>())
            {
                timeLimitSec -= Date.Now - battle.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime;
            }
            string timerText = TimerUtils.GetTimerText(timeLimitSec);
            battle.battleItemContent.SetTimeField(timerText);
        }

        public class ArchivedBattleNode : BattleInfoSystem.BattleWithViewNode
        {
            public ArchivedBattleComponent archivedBattle;
        }

        public class BattleNode : Node
        {
            public MapGroupComponent mapGroup;
            public BattleComponent battle;
            public BattleModeComponent battleMode;
            public UserCountComponent userCount;
            public UserLimitComponent userLimit;
            public SearchDataComponent searchData;
            public BattleConfiguredComponent battleConfigured;
        }

        [Not(typeof(ScoreLimitComponent))]
        public class BattleScoreWithoutLimitNode : BattleInfoSystem.BattleWithViewNode
        {
            public BattleScoreComponent battleScore;
        }

        public class BattleTimeWithLimitNode : BattleInfoSystem.BattleWithViewNode
        {
            public TimeLimitComponent timeLimit;
        }

        [Not(typeof(TimeLimitComponent))]
        public class BattleTimeWithoutLimitNode : BattleInfoSystem.BattleWithViewNode
        {
        }

        public class BattleWithPreviewDataNode : BattleInfoSystem.VisibleBattleNode
        {
            public MapPreviewDataComponent mapPreviewData;
            public BattleItemContentComponent battleItemContent;
        }

        public class BattleWithViewNode : BattleInfoSystem.VisibleBattleNode
        {
            public BattleItemContentComponent battleItemContent;
        }

        public class CTFBattleNode : BattleInfoSystem.VisibleBattleNode
        {
            public CTFComponent ctf;
            public BattleItemContentComponent battleItemContent;
        }

        public class RoundNode : Node
        {
            public BattleGroupComponent battleGroup;
            public RoundComponent round;
        }

        public class ScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public LazyListComponent lazyList;
            public VisibleItemsRangeComponent visibleItemsRange;
        }

        public class VisibleBattleNode : BattleInfoSystem.BattleNode
        {
            public VisibleItemComponent visibleItem;
        }
    }
}

