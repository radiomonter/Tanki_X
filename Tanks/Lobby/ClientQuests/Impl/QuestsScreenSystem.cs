namespace Tanks.Lobby.ClientQuests.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientQuests.API;
    using UnityEngine;

    public class QuestsScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<QuestNode> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<AnimatedQuestNode> <>f__am$cache1;

        [OnEventFire]
        public void ChangeQuest(QuestRemovedEvent e, QuestResultGUINode removedQuestResult, [JoinAll] ICollection<QuestWithoutGUINode> quests, [JoinAll] QuestsScreenNode screen, [JoinAll] ICollection<QuestCellNode> cells)
        {
            <ChangeQuest>c__AnonStorey2 storey = new <ChangeQuest>c__AnonStorey2 {
                removedQuestResult = removedQuestResult
            };
            QuestWithoutGUINode quest = quests.ToList<QuestWithoutGUINode>().Find(new Predicate<QuestWithoutGUINode>(storey.<>m__0));
            if ((quest != null) && !quest.Entity.Id.Equals(storey.removedQuestResult.Entity.Id))
            {
                this.CreateQuestItemGUIInstance(quest, cells, screen);
            }
            base.DeleteEntity(storey.removedQuestResult.Entity);
        }

        [OnEventFire]
        public void ClearQuestResult(NodeRemoveEvent e, SingleNode<QuestWindowComponent> mainScreen, [JoinAll, Combine] SingleNode<QuestResultComponent> result)
        {
            base.DeleteEntity(result.Entity);
        }

        [OnEventFire]
        public void ClearScreen(NodeRemoveEvent e, SingleNode<QuestWindowComponent> screen)
        {
            IEnumerator enumerator = screen.component.QuestsContainer.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Object.Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void CreateQuestItem(QuestsScreenNode screen, GameObject questCellGameObject, Entity quest)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(screen.questWindow.QuestPrefab);
            obj2.transform.SetParent(questCellGameObject.transform, false);
            obj2.GetComponent<EntityBehaviour>().BuildEntity(quest);
        }

        private void CreateQuestItemGUIInstance(QuestNode quest, ICollection<QuestCellNode> cells, QuestsScreenNode screen)
        {
            <CreateQuestItemGUIInstance>c__AnonStorey3 storey = new <CreateQuestItemGUIInstance>c__AnonStorey3 {
                quest = quest
            };
            QuestCellNode node = cells.ToList<QuestCellNode>().Find(new Predicate<QuestCellNode>(storey.<>m__0));
            if (node == null)
            {
                this.CreateQuestSlotWithItem(screen, storey.quest.Entity);
            }
            else
            {
                this.CreateQuestItem(screen, node.questCell.gameObject, storey.quest.Entity);
                if (!storey.quest.Entity.HasComponent<CompleteQuestComponent>())
                {
                    storey.quest.Entity.GetComponent<QuestItemGUIComponent>().AddQuest();
                }
                else
                {
                    storey.quest.Entity.GetComponent<QuestItemGUIComponent>().SetQuestCompleted(true);
                }
            }
        }

        [OnEventFire]
        public void CreateQuestItemGUIInstance(NodeAddedEvent e, QuestNode quest, [JoinAll] QuestsScreenNode screen, [JoinAll] ICollection<QuestCellNode> cells)
        {
            this.CreateQuestItemGUIInstance(quest, cells, screen);
        }

        [OnEventFire]
        public void CreateQuestItemGUIInstances(NodeAddedEvent e, QuestsScreenNode screen, [JoinAll] ICollection<QuestNode> quests)
        {
            <CreateQuestItemGUIInstances>c__AnonStorey1 storey = new <CreateQuestItemGUIInstances>c__AnonStorey1 {
                screen = screen,
                $this = this
            };
            if (quests.Count != 0)
            {
                List<QuestNode> source = new List<QuestNode>();
                using (IEnumerator<QuestNode> enumerator = quests.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        <CreateQuestItemGUIInstances>c__AnonStorey0 storey2 = new <CreateQuestItemGUIInstances>c__AnonStorey0 {
                            quest = enumerator.Current
                        };
                        int num = quests.Count<QuestNode>(new Func<QuestNode, bool>(storey2.<>m__0));
                        bool flag = num == 1;
                        bool flag2 = ((num == 2) && storey2.quest.Entity.HasComponent<QuestResultComponent>()) && storey.screen.questWindow.ShowProgress;
                        bool flag3 = ((num == 2) && !storey2.quest.Entity.HasComponent<QuestResultComponent>()) && !storey.screen.questWindow.ShowProgress;
                        if (flag || (flag2 || flag3))
                        {
                            source.Add(storey2.quest);
                        }
                    }
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = (a, b) => b.questOrder.Index.CompareTo(a.questOrder.Index);
                }
                source.Sort(<>f__am$cache0);
                source.ForEach(new Action<QuestNode>(storey.<>m__0));
                source.ForEach(new Action<QuestNode>(storey.<>m__1));
                if (source.Count != 0)
                {
                    base.ScheduleEvent<ShowNextQuestAnimationEvent>(source.First<QuestNode>());
                }
            }
        }

        [OnEventFire]
        public void CreateQuestResult(NodeAddedEvent e, CompletedRewardedQuestNode questNode)
        {
            if (!questNode.questProgress.PrevValue.Equals(questNode.questProgress.CurrentValue))
            {
                Entity entity = base.CreateEntity("QuestResult");
                entity.AddComponent<QuestResultComponent>();
                entity.AddComponent<QuestComponent>();
                entity.AddComponent<CompleteQuestComponent>();
                entity.AddComponent<RewardedQuestComponent>();
                QuestConditionComponent component = new QuestConditionComponent {
                    Condition = questNode.questCondition.Condition
                };
                entity.AddComponent(component);
                QuestRewardComponent component2 = new QuestRewardComponent {
                    Reward = questNode.questReward.Reward
                };
                entity.AddComponent(component2);
                QuestOrderComponent component3 = new QuestOrderComponent {
                    Index = questNode.questOrder.Index
                };
                entity.AddComponent(component3);
                QuestProgressComponent questProgress = questNode.questProgress;
                QuestProgressComponent component5 = new QuestProgressComponent {
                    PrevValue = questProgress.PrevValue,
                    CurrentValue = questProgress.CurrentValue,
                    TargetValue = questProgress.TargetValue,
                    PrevComplete = questProgress.PrevComplete,
                    CurrentComplete = questProgress.CurrentComplete
                };
                entity.AddComponent(component5);
                QuestConditionDescriptionComponent component6 = new QuestConditionDescriptionComponent {
                    condition = questNode.questConditionDescription.condition,
                    restrictionFormat = questNode.questConditionDescription.restrictionFormat,
                    restrictions = questNode.questConditionDescription.restrictions
                };
                entity.AddComponent(component6);
            }
        }

        private void CreateQuestSlotWithItem(QuestsScreenNode screen, Entity quest)
        {
            GameObject questCellGameObject = Object.Instantiate<GameObject>(screen.questWindow.QuestCellPrefab);
            questCellGameObject.transform.SetParent(screen.questWindow.QuestsContainer.transform, false);
            questCellGameObject.GetComponent<QuestCellComponent>().Order = quest.GetComponent<QuestOrderComponent>().Index;
            this.CreateQuestItem(screen, questCellGameObject, quest);
            if (quest.HasComponent<CompleteQuestComponent>() && !quest.HasComponent<QuestResultComponent>())
            {
                quest.GetComponent<QuestItemGUIComponent>().SetQuestCompleted(true);
            }
            else
            {
                quest.GetComponent<QuestItemGUIComponent>().ShowQuest();
            }
        }

        [OnEventFire]
        public void EnableQuests(NodeAddedEvent e, SingleNode<QuestsEnabledComponent> questsEnabled, SingleNode<MainScreenComponent> mainScreen, [JoinAll] ICollection<QuestNode> quest)
        {
            bool flag = quest.Count > 0;
            mainScreen.component.QuestsBtn.SetActive(flag);
        }

        [OnEventFire]
        public void OpenQuestDialogs(NodeAddedEvent e, SingleNode<MainScreenComponent> mainScreen, SingleNode<TutorialShowQuestsHandler> showQuestsHandler, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            showQuestsHandler.component.gameObject.SetActive(false);
            QuestWindowComponent component = dialogs.component.Get<QuestWindowComponent>();
            component.ShowOnMainScreen = true;
            component.ShowProgress = false;
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            component.HideDelegate = new Action(showQuestsHandler.component.OpenHullModules);
            component.Show(animators);
        }

        [OnEventFire]
        public void Register(NodeAddedEvent e, DailyQuestNode quest)
        {
        }

        [OnEventFire]
        public void RemoveQuest(QuestRemovedEvent e, NotResultQuestGUINode questNode)
        {
            base.DeleteEntity(questNode.Entity);
        }

        [OnEventFire]
        public void RemoveQuestAnimator(NodeRemoveEvent e, SingleNode<QuestItemGUIComponent> quest)
        {
            if (quest.Entity.HasComponent<QuestProgressAnimatorComponent>())
            {
                quest.Entity.RemoveComponent<QuestProgressAnimatorComponent>();
            }
        }

        [OnEventFire]
        public void RemoveQuestItemGUIInstances(NodeRemoveEvent e, QuestGUINode quest, [JoinAll] QuestsScreenNode screen)
        {
            quest.questItemGUI.RemoveQuest();
        }

        private void SaveAndResetPreviousQuestProgress(QuestNode questNode, bool showProgress)
        {
            if (!questNode.questProgress.PrevValue.Equals(questNode.questProgress.CurrentValue))
            {
                if (showProgress)
                {
                    if (questNode.Entity.HasComponent<QuestProgressAnimatorComponent>())
                    {
                        questNode.Entity.GetComponent<QuestProgressAnimatorComponent>().ProgressPrevValue = questNode.questProgress.PrevValue;
                    }
                    else
                    {
                        QuestProgressAnimatorComponent component = new QuestProgressAnimatorComponent {
                            ProgressPrevValue = questNode.questProgress.PrevValue
                        };
                        questNode.Entity.AddComponent(component);
                    }
                }
                base.ScheduleEvent<ResetQuestProgressEvent>(questNode.Entity);
            }
        }

        [OnEventFire]
        public void SetQuestUpdated(QuestProgressUpdatedEvent e, QuestNode quest, [JoinAll] QuestsScreenNode screen)
        {
            this.SaveAndResetPreviousQuestProgress(quest, screen.questWindow.ShowProgress);
            base.ScheduleEvent<ShowNextQuestAnimationEvent>(quest);
        }

        [OnEventFire]
        public void ShowNextQuestAnimation(ShowNextQuestAnimationEvent e, Node any, [JoinAll] ICollection<AnimatedQuestNode> quests)
        {
            if (quests.Count != 0)
            {
                List<AnimatedQuestNode> list = quests.ToList<AnimatedQuestNode>();
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = (a, b) => b.questOrder.Index.CompareTo(a.questOrder.Index);
                }
                list.Sort(<>f__am$cache1);
                float num = 1f;
                for (int i = 0; i < list.Count; i++)
                {
                    ShowQuestGUIAnimationEvent eventInstance = new ShowQuestGUIAnimationEvent {
                        ProgressDelay = num
                    };
                    base.NewEvent(eventInstance).Attach(list[i]).ScheduleDelayed((i + 1) * num);
                    base.NewEvent<TryShowQuestRewardNotification>().Attach(list[i]).ScheduleDelayed((i + 2) * num);
                }
            }
        }

        [OnEventFire]
        public void ShowQuests(ButtonClickEvent e, SingleNode<QuestsButtonComponent> questRewardButton, [JoinAll] SingleNode<WindowsSpaceComponent> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            QuestWindowComponent component = dialogs.component.Get<QuestWindowComponent>();
            component.ShowOnMainScreen = true;
            component.ShowProgress = false;
            component.Show(screens.component.Animators);
        }

        [CompilerGenerated]
        private sealed class <ChangeQuest>c__AnonStorey2
        {
            internal QuestsScreenSystem.QuestResultGUINode removedQuestResult;

            internal bool <>m__0(QuestsScreenSystem.QuestWithoutGUINode q) => 
                q.questOrder.Index == this.removedQuestResult.questOrder.Index;
        }

        [CompilerGenerated]
        private sealed class <CreateQuestItemGUIInstance>c__AnonStorey3
        {
            internal QuestsScreenSystem.QuestNode quest;

            internal bool <>m__0(QuestsScreenSystem.QuestCellNode cell) => 
                cell.questCell.Order == this.quest.questOrder.Index;
        }

        [CompilerGenerated]
        private sealed class <CreateQuestItemGUIInstances>c__AnonStorey0
        {
            internal QuestsScreenSystem.QuestNode quest;

            internal bool <>m__0(QuestsScreenSystem.QuestNode q) => 
                q.questOrder.Index == this.quest.questOrder.Index;
        }

        [CompilerGenerated]
        private sealed class <CreateQuestItemGUIInstances>c__AnonStorey1
        {
            internal QuestsScreenSystem.QuestsScreenNode screen;
            internal QuestsScreenSystem $this;

            internal void <>m__0(QuestsScreenSystem.QuestNode quest)
            {
                this.$this.SaveAndResetPreviousQuestProgress(quest, this.screen.questWindow.ShowProgress);
            }

            internal void <>m__1(QuestsScreenSystem.QuestNode quest)
            {
                this.$this.CreateQuestSlotWithItem(this.screen, quest.Entity);
            }
        }

        public class AnimatedQuestNode : QuestsScreenSystem.QuestNode
        {
            public QuestItemGUIComponent questItemGUI;
            public QuestProgressAnimatorComponent questProgressAnimator;
        }

        [Not(typeof(QuestResultComponent))]
        public class CompletedRewardedQuestNode : Node
        {
            public QuestComponent quest;
            public QuestOrderComponent questOrder;
            public QuestProgressComponent questProgress;
            public QuestConditionComponent questCondition;
            public QuestRewardComponent questReward;
            public CompleteQuestComponent completeQuest;
            public RewardedQuestComponent rewardedQuest;
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class DailyQuestNode : QuestsScreenSystem.QuestNode
        {
            public BattleCountQuestComponent battleCountQuest;
            public FlagQuestComponent flagQuest;
            public FragQuestComponent fragQuest;
            public ScoreQuestComponent scoreQuest;
            public SupplyQuestComponent supplyQuest;
            public WinQuestComponent winQuest;
        }

        [Not(typeof(QuestResultComponent))]
        public class NotResultQuestGUINode : QuestsScreenSystem.QuestNode
        {
            public QuestItemGUIComponent questItemGUI;
        }

        [Not(typeof(QuestResultComponent))]
        public class NotResultQuestWithoutGUINode : QuestsScreenSystem.QuestWithoutGUINode
        {
        }

        public class QuestCellNode : Node
        {
            public QuestCellComponent questCell;
        }

        public class QuestGUINode : QuestsScreenSystem.QuestNode
        {
            public QuestItemGUIComponent questItemGUI;
        }

        public class QuestNode : Node
        {
            public QuestComponent quest;
            public QuestOrderComponent questOrder;
            public QuestProgressComponent questProgress;
        }

        public class QuestResultGUINode : QuestsScreenSystem.QuestGUINode
        {
            public QuestResultComponent questResult;
        }

        public class QuestsButtonNode : Node
        {
            public QuestsButtonComponent questsButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class QuestsScreenNode : Node
        {
            public QuestWindowComponent questWindow;
        }

        [Not(typeof(QuestItemGUIComponent))]
        public class QuestWithoutGUINode : QuestsScreenSystem.QuestNode
        {
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfComponent self;
            public UserMoneyComponent userMoney;
        }

        public class TryShowQuestRewardNotification : Event
        {
        }
    }
}

