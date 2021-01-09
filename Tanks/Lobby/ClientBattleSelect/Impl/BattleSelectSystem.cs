namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleSelectSystem : ECSSystem
    {
        public static int TRAIN_BATTLE_MAXIMAL_RANK = 1;
        [CompilerGenerated]
        private static Func<BattleEntry, string> <>f__am$cache0;

        [OnEventFire]
        public void AddSearchResult(NodeAddedEvent e, BattleNode battle, [JoinAll] BattleSelectNode battleSelect)
        {
            base.Log.InfoFormat("AddSearchResult(NA) call TryAddSearchDataToBattle " + battle.Entity, new object[0]);
            this.TryAddSearchDataToBattle(battle.Entity, battleSelect);
        }

        [OnEventComplete, Mandatory]
        public void AddSearchResult(SearchResultEvent e, BattleSelectNode battleSelect, [JoinAll] ICollection<BattleNode> battles)
        {
            if (base.Log.IsInfoEnabled)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = data => data.Id.ToString();
                }
                base.Log.InfoFormat("AddSearchResult Battles={0}", string.Join(", ", e.NewBattleEntries.Select<BattleEntry, string>(<>f__am$cache0).ToArray<string>()));
            }
            battleSelect.searchResult.PinnedBattles.AddRange(e.NewBattleEntries);
            battleSelect.searchResult.PersonalInfos.AddRange(e.NewPersonalBattleInfos);
            battleSelect.searchResult.BattlesCount = e.BattlesCount;
            foreach (BattleNode node in battles)
            {
                base.Log.InfoFormat("AddSearchResult(EVENT) call TryAddSearchDataToBattle " + node.Entity, new object[0]);
                this.TryAddSearchDataToBattle(node.Entity, battleSelect);
            }
        }

        [OnEventFire]
        public void ClearContext(NodeRemoveEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen, [JoinByScreen] ActiveContextNode context)
        {
            context.battleSelectScreenContext.BattleId = null;
        }

        [OnEventFire, Mandatory]
        public void ClearSearchResult(ResetSearchCallbackEvent e, BattleSelectNode battleSelect, [JoinAll] ICollection<BattleNode> battles)
        {
            foreach (BattleNode node in battles)
            {
                this.TryRemoveSearchDataFromBattle(node.Entity, battleSelect);
            }
            battleSelect.searchResult.PinnedBattles.Clear();
            battleSelect.searchResult.PersonalInfos.Clear();
            battleSelect.searchResult.BattlesCount = 0;
        }

        private EventBuilder CreateShowEvent(long? battleId)
        {
            Entity context = base.CreateEntity("BattleSelectScreenContext");
            context.AddComponent(new BattleSelectScreenContextComponent(battleId));
            ShowScreenLeftEvent<BattleSelectScreenComponent> eventInstance = new ShowScreenLeftEvent<BattleSelectScreenComponent>();
            eventInstance.SetContext(context, true);
            return base.NewEvent(eventInstance).Attach(context);
        }

        [OnEventFire]
        public void Deinit(NodeRemoveEvent e, ScreenInitNode screen, BattleSelectNode battleSelect)
        {
            base.Log.Info("Deinit");
            base.ScheduleEvent<ResetSearchEvent>(battleSelect);
            screen.Entity.RemoveComponent<BattleSelectLoadedComponent>();
        }

        [OnEventFire]
        public void EnableBattleScreen(EnterRelevantBattleFailedEvent e, Node any, [JoinAll] SingleNode<BattlesButtonComponent> button)
        {
            button.component.gameObject.SetInteractable(true);
        }

        private int GetEffectiveLevel(Optional<MountedWeaponNode> weapon, Optional<MountedHullNode> hull) => 
            (!weapon.IsPresent() || !hull.IsPresent()) ? TRAIN_BATTLE_MAXIMAL_RANK : Math.Max(weapon.Get().upgradeLevelItem.Level, hull.Get().upgradeLevelItem.Level);

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<BattlesButtonComponent> button)
        {
            button.component.gameObject.SetInteractable(true);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, ScreenInitNode screen, BattleSelectNode battleSelect)
        {
            screen.Entity.AddComponent<BattleSelectLoadedComponent>();
        }

        [OnEventFire]
        public void NextBattles(ButtonClickEvent e, SingleNode<NextBattlesButtonComponent> nextBattlesButton, [JoinByScreen] BattleSelectScreenNode screen)
        {
            screen.lazyList.Scroll(1);
        }

        [OnEventFire]
        public void ParseLink(ParseLinkEvent e, Node node)
        {
            if (e.Link.StartsWith("battleselect"))
            {
                long? battleId = null;
                e.CustomNavigationEvent = this.CreateShowEvent(battleId);
            }
            else
            {
                long num;
                if (e.Link.StartsWith("battle/") && long.TryParse(e.Link.Substring("battle/".Length), out num))
                {
                    e.CustomNavigationEvent = base.NewEvent(new ShowBattleEvent(num)).Attach(node);
                }
            }
        }

        [OnEventFire]
        public void PrevBattles(ButtonClickEvent e, SingleNode<PrevBattlesButtonComponent> prevBattlesButton, [JoinByScreen] BattleSelectScreenNode screen)
        {
            screen.lazyList.Scroll(-1);
        }

        [OnEventFire]
        public void RequestOnServer(ScreenRangeChangedEvent e, BattleSelectScreenNode screen, [JoinAll] BattleSelectNode battleSelect)
        {
            base.ScheduleEvent(new SearchRequestChangedEvent(e.Range), battleSelect);
        }

        [OnEventFire]
        public void SetBattlesCount(SearchResultEvent e, BattleSelectNode battleSelect, [JoinAll] BattleSelectScreenNode screen)
        {
            screen.lazyList.ItemsCount = e.BattlesCount;
        }

        [OnEventFire]
        public void SetIdToContext(NodeAddedEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen, [JoinByScreen] ActiveContextNode context)
        {
            context.battleSelectScreenContext.BattleId = new long?(battle.Entity.Id);
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] NotBattleSelectScreenNode screen)
        {
            this.ShowBattleSelect(new long?(e.BattleId));
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] BattleSelectScreenNode screen, [JoinAll] BattleSelectNode battleSelect)
        {
            IndexRange visibleItemsRange = screen.lazyList.VisibleItemsRange;
            screen.lazyList.ClearItems();
            base.ScheduleEvent<ResetSearchEvent>(battleSelect);
            base.ScheduleEvent(new RequestBattleEvent(e.BattleId), battleSelect);
            base.ScheduleEvent(new SearchRequestChangedEvent(visibleItemsRange), battleSelect);
        }

        private void ShowBattleSelect(long? battleId)
        {
            this.CreateShowEvent(battleId).Schedule();
        }

        [OnEventFire]
        public void ShowBattlesScreen(ButtonClickEvent e, SingleNode<BattlesButtonComponent> node, [JoinAll] SelfUserNode user1, [JoinByUser] Optional<MountedWeaponNode> weapon, [JoinAll] SelfUserNode user2, [JoinByUser] Optional<MountedHullNode> hull)
        {
            if (this.TryEnterTrainBattle(user1, this.GetEffectiveLevel(weapon, hull)))
            {
                node.component.gameObject.SetInteractable(false);
            }
            else
            {
                long? battleId = null;
                this.ShowBattleSelect(battleId);
            }
        }

        [OnEventFire]
        public void ShowBattleWithContext(NodeAddedEvent e, ActiveContextNode activeContext, [JoinAll] BattleSelectNode battleSelect)
        {
            long? battleId = activeContext.battleSelectScreenContext.BattleId;
            if (battleId != null)
            {
                base.ScheduleEvent(new RequestBattleEvent(battleId.Value), battleSelect);
            }
        }

        private void TryAddSearchDataToBattle(Entity battle, BattleSelectNode battleSelect)
        {
            base.Log.InfoFormat("TryAddSearchDataToBattle {0}", battle.Id);
            if (!battle.HasComponent<SearchDataComponent>())
            {
                for (int i = 0; i < battleSelect.searchResult.PinnedBattles.Count; i++)
                {
                    BattleEntry battleEntry = battleSelect.searchResult.PinnedBattles[i];
                    if (battleEntry.Id == battle.Id)
                    {
                        base.Log.InfoFormat("AddSearchDataToBattle {0}", battle.Id);
                        battle.AddComponent(new SearchDataComponent(battleEntry, i));
                        PersonalBattleInfoComponent component = new PersonalBattleInfoComponent {
                            Info = battleSelect.searchResult.PersonalInfos[i]
                        };
                        battle.AddComponent(component);
                        return;
                    }
                }
            }
        }

        private bool TryEnterTrainBattle(SelfUserNode selfUser, int effectiveLevel)
        {
            if (effectiveLevel >= TRAIN_BATTLE_MAXIMAL_RANK)
            {
                return false;
            }
            EnterRelevantBattleEvent eventInstance = new EnterRelevantBattleEvent {
                PreferredTeam = TeamColor.BLUE,
                PreferredBattle = 0L,
                Source = "Train"
            };
            base.ScheduleEvent(eventInstance, selfUser);
            return true;
        }

        private void TryRemoveSearchDataFromBattle(Entity battle, BattleSelectNode battleSelect)
        {
            base.Log.InfoFormat("TryRemoveSearchDataFromBattle {0}", battle.Id);
            if (battle.HasComponent<SearchDataComponent>())
            {
                for (int i = 0; i < battleSelect.searchResult.PinnedBattles.Count; i++)
                {
                    BattleEntry entry = battleSelect.searchResult.PinnedBattles[i];
                    if (entry.Id == battle.Id)
                    {
                        base.Log.InfoFormat("RemoveSearchDataFromBattle {0}", battle.Id);
                        battle.RemoveComponent<SearchDataComponent>();
                        battle.RemoveComponent<PersonalBattleInfoComponent>();
                        return;
                    }
                }
            }
        }

        [OnEventFire]
        public void UpdateScrollButtonsVisibility(ScrollLimitEvent e, BattleSelectScreenNode screen)
        {
            screen.battleSelectScreen.PrevBattlesButton.SetActive(!screen.lazyList.AtLimitLow);
            screen.battleSelectScreen.NextBattlesButton.SetActive(!screen.lazyList.AtLimitHigh);
        }

        public class ActiveContextNode : Node
        {
            public BattleSelectScreenContextComponent battleSelectScreenContext;
            public ScreenGroupComponent screenGroup;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class BattleSelectNode : Node
        {
            public BattleSelectComponent battleSelect;
            public SearchResultComponent searchResult;
        }

        public class BattleSelectScreenNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public ActiveScreenComponent activeScreen;
            public BattleSelectLoadedComponent battleSelectLoaded;
            public ScreenGroupComponent screenGroup;
            public LazyListComponent lazyList;
        }

        public class MountedHullNode : Node
        {
            public TankItemComponent tankItem;
            public MountedItemComponent mountedItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class MountedWeaponNode : Node
        {
            public WeaponItemComponent weaponItem;
            public MountedItemComponent mountedItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        [Not(typeof(BattleSelectScreenComponent))]
        public class NotBattleSelectScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
        }

        public class ScreenInitNode : Node
        {
            public BattleSelectScreenComponent battleSelectScreen;
            public ScreenGroupComponent screenGroup;
            public LazyListComponent lazyList;
        }

        public class SelectedBattleNode : Node
        {
            public BattleComponent battle;
            public SelectedListItemComponent selectedListItem;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }
    }
}

