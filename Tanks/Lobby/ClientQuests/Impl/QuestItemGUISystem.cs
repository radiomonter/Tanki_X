namespace Tanks.Lobby.ClientQuests.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientProfile.API;
    using Tanks.Lobby.ClientQuests.API;

    public class QuestItemGUISystem : ECSSystem
    {
        private string GetDescriptionPart(Dictionary<CaseType, string> cases, float value)
        {
            if (cases.Count == 0)
            {
                return string.Empty;
            }
            CaseType @case = CasesUtil.GetCase((int) value);
            return (!cases.ContainsKey(@case) ? cases[CaseType.DEFAULT] : cases[@case]);
        }

        [OnEventFire]
        public void HideQuestsChangeMenu(HideQuestsChangeMenuEvent e, DailyQuestGUINode openingQuest, [JoinByUser, Combine] DailyQuestGUINode quest)
        {
            if (!quest.Entity.Id.Equals(openingQuest.Entity.Id))
            {
                quest.questItemGUI.RejectChangeQuest();
            }
        }

        [OnEventFire]
        public void MarkQuestResultAsResult(NodeAddedEvent e, QuestResultGUINode quest)
        {
            quest.questItemGUI.SetQuestResult(true);
        }

        [OnEventFire]
        public void SetCurrentQuestProgress(NodeAddedEvent e, QuestWithoutProgressNode quest)
        {
            quest.questProgressGUI.Initialize(quest.questProgress.CurrentValue, quest.questProgress.TargetValue);
        }

        [OnEventFire]
        public void SetDailyQuestConditions(NodeAddedEvent e, QuestConditionNode quest)
        {
            if ((quest.questCondition.Condition != null) && (quest.questCondition.Condition.Count != 0))
            {
                QuestConditionType key = quest.questCondition.Condition.First<KeyValuePair<QuestConditionType, long>>().Key;
                long id = quest.questCondition.Condition.First<KeyValuePair<QuestConditionType, long>>().Value;
                string str = (key == QuestConditionType.MODE) ? ((byte) id).ToString() : Flow.Current.EntityRegistry.GetEntity(id).GetComponent<DescriptionItemComponent>().Name;
                quest.questItemGUI.ConditionText = string.Format(quest.questConditionDescription.restrictionFormat, quest.questConditionDescription.restrictions[key], str);
            }
        }

        [OnEventFire]
        public void SetKillsEveryDayQuestConditionGUI(NodeAddedEvent e, KillsInOneBattleEveryDayQuestNode quest)
        {
            <SetKillsEveryDayQuestConditionGUI>c__AnonStorey2 storey = new <SetKillsEveryDayQuestConditionGUI>c__AnonStorey2 {
                quest = quest
            };
            KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest = storey.quest.killsInOneBattleEveryDayQuest;
            QuestParameters parameters = storey.quest.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            storey.quest.questItemGUI.TaskText = string.Format(storey.quest.questConditionDescription.condition.format, parameters.TargetValue, killsInOneBattleEveryDayQuest.Days);
        }

        [OnEventFire]
        public void SetKillsInManyBattlesQuestConditionGUI(NodeAddedEvent e, KillsInManyBattlesQuestNode quest)
        {
            <SetKillsInManyBattlesQuestConditionGUI>c__AnonStorey1 storey = new <SetKillsInManyBattlesQuestConditionGUI>c__AnonStorey1 {
                quest = quest
            };
            KillsInManyBattlesQuestComponent killsInManyBattlesQuest = storey.quest.killsInManyBattlesQuest;
            QuestParameters parameters = storey.quest.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            storey.quest.questItemGUI.TaskText = string.Format(storey.quest.questConditionDescription.condition.format, parameters.TargetValue, killsInManyBattlesQuest.Battles);
        }

        [OnEventFire]
        public void SetKillsInOneBattleQuestConditionGUI(NodeAddedEvent e, KillsInOneBattleQuestNode quest)
        {
            <SetKillsInOneBattleQuestConditionGUI>c__AnonStorey0 storey = new <SetKillsInOneBattleQuestConditionGUI>c__AnonStorey0 {
                quest = quest
            };
            QuestParameters parameters = storey.quest.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            storey.quest.questItemGUI.TaskText = string.Format(storey.quest.questConditionDescription.condition.format, parameters.TargetValue);
        }

        [OnEventFire]
        public void SetPreviousQuestProgress(NodeAddedEvent e, QuestWithProgressNode quest)
        {
            quest.questProgressGUI.Initialize(quest.questProgressAnimator.ProgressPrevValue, quest.questProgress.TargetValue);
        }

        [OnEventFire]
        public void SetQuestChangeAbility(NodeAddedEvent e, TakenQuestBonusNode bonus, [JoinByUser, Combine] QuestRarityNode quest)
        {
            quest.questItemGUI.SetChangeButtonActivity(false);
        }

        [OnEventFire]
        public void SetQuestChangeAbility(NodeAddedEvent e, QuestRarityNode quest, [JoinByUser] Optional<NotTakenQuestBonusNode> bonus, [JoinAll] SingleNode<QuestWindowComponent> questsScreen)
        {
            quest.questItemGUI.SetChangeButtonActivity((bonus.IsPresent() && !quest.questRarity.RarityType.Equals(QuestRarityType.PREMIUM)) && questsScreen.component.ShowOnMainScreen);
        }

        [OnEventFire]
        public void SetQuestCompleted(NodeAddedEvent e, CompleteQuestNode quest)
        {
            if (quest.questProgress.PrevValue.Equals(quest.questProgress.CurrentValue))
            {
                this.ShowCompletedQuest(quest);
            }
        }

        [OnEventFire]
        public void SetQuestCompleted(ShowQuestGUIAnimationEvent e, CompleteQuestWithProgressNode quest)
        {
            base.NewEvent<ShowQuestCompleteEvent>().Attach(quest).ScheduleDelayed(e.ProgressDelay);
        }

        [OnEventFire]
        public void SetQuestDescription(NodeAddedEvent e, QuestDescriptionNode quest)
        {
            string descriptionPart = this.GetDescriptionPart(quest.questConditionDescription.condition.cases, quest.questProgress.TargetValue);
            quest.questItemGUI.TaskText = string.Format(quest.questConditionDescription.condition.format, quest.questProgress.TargetValue, descriptionPart);
        }

        [OnEventFire]
        public void SetQuestRarity(NodeAddedEvent e, [Combine] QuestRarityNode quest, [JoinByUser, Context] SelfPremiumQuestUserNode user)
        {
            if (quest.questRarity.RarityType.Equals(QuestRarityType.PREMIUM))
            {
                Date endDate = user.premiumAccountQuest.EndDate;
                Date date = quest.questExpireDate.Date;
                int num = (date <= endDate) ? (((int) ((endDate - date) / 86400f)) + 1) : 0;
                int count = num + (!quest.Entity.HasComponent<CompleteQuestComponent>() ? 1 : 0);
                quest.questItemGUI.ShowPremiumBack(count);
            }
        }

        [OnEventFire]
        public void SetQuestReward(NodeAddedEvent e, DailyQuestGUINode quest)
        {
            if (quest.questReward.Reward != null)
            {
                this.SetQuestReward(quest.questRewardGUI, quest.questReward.Reward);
            }
        }

        [OnEventFire]
        public void SetQuestReward(NodeAddedEvent e, OldQuestGUINode quest)
        {
            <SetQuestReward>c__AnonStorey3 storey = new <SetQuestReward>c__AnonStorey3 {
                quest = quest
            };
            QuestParameters parameters = storey.quest.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            this.SetQuestReward(storey.quest.questRewardGUI, parameters.QuestReward);
        }

        private void SetQuestReward(QuestRewardGUIComponent questRewardGUI, Dictionary<long, int> reward)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(reward.First<KeyValuePair<long, int>>().Key);
            questRewardGUI.RewardInfoText = (reward.First<KeyValuePair<long, int>>().Value <= 1) ? entity.GetComponent<DescriptionItemComponent>().Name : reward.First<KeyValuePair<long, int>>().Value.ToString();
            string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
            if (!string.IsNullOrEmpty(spriteUid))
            {
                questRewardGUI.SetImage(spriteUid);
            }
        }

        [OnEventFire]
        public void SetTargetValue(NodeAddedEvent e, QuestGUINode quest)
        {
            quest.questProgressGUI.TargetProgressValue = quest.questProgress.TargetValue.ToString();
        }

        private void ShowCompletedQuest(QuestGUINode quest)
        {
            quest.questItemGUI.SetQuestCompleted(true);
        }

        [OnEventFire]
        public void ShowQuestComplete(ShowQuestCompleteEvent e, QuestGUINode quest)
        {
            quest.questItemGUI.CompeleQuest();
        }

        [OnEventFire]
        public void ShowQuestExpireDate(NodeAddedEvent e, QuestExpireDateNode date)
        {
            date.questExpireTimer.EndDate = new Date(date.questExpireDate.Date.UnityTime);
        }

        [OnEventFire]
        public void ShowQuestProgressAnimation(ShowQuestGUIAnimationEvent e, QuestWithProgressNode quest)
        {
            quest.questProgressGUI.DeltaProgressValue = (quest.questProgress.CurrentValue - quest.questProgressAnimator.ProgressPrevValue).ToString();
            quest.questProgressGUI.Set(quest.questProgress.CurrentValue, quest.questProgress.TargetValue);
            quest.Entity.RemoveComponent<QuestProgressAnimatorComponent>();
        }

        [CompilerGenerated]
        private sealed class <SetKillsEveryDayQuestConditionGUI>c__AnonStorey2
        {
            internal QuestItemGUISystem.KillsInOneBattleEveryDayQuestNode quest;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.quest.userRank.Rank);
        }

        [CompilerGenerated]
        private sealed class <SetKillsInManyBattlesQuestConditionGUI>c__AnonStorey1
        {
            internal QuestItemGUISystem.KillsInManyBattlesQuestNode quest;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.quest.userRank.Rank);
        }

        [CompilerGenerated]
        private sealed class <SetKillsInOneBattleQuestConditionGUI>c__AnonStorey0
        {
            internal QuestItemGUISystem.KillsInOneBattleQuestNode quest;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.quest.userRank.Rank);
        }

        [CompilerGenerated]
        private sealed class <SetQuestReward>c__AnonStorey3
        {
            internal QuestItemGUISystem.OldQuestGUINode quest;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.quest.userRank.Rank);
        }

        public class CompleteQuestNode : QuestItemGUISystem.QuestWithoutProgressNode
        {
            public CompleteQuestComponent completeQuest;
        }

        public class CompleteQuestWithProgressNode : QuestItemGUISystem.QuestWithProgressNode
        {
            public CompleteQuestComponent completeQuest;
        }

        public class DailyQuestGUINode : QuestItemGUISystem.DailyQuestNode
        {
            public QuestItemGUIComponent questItemGUI;
            public QuestProgressGUIComponent questProgressGUI;
            public QuestRewardGUIComponent questRewardGUI;
        }

        public class DailyQuestNode : QuestItemGUISystem.QuestNode
        {
            public QuestRewardComponent questReward;
        }

        public class KillsInManyBattlesQuestNode : QuestItemGUISystem.OldQuestGUINode
        {
            public KillsInManyBattlesQuestComponent killsInManyBattlesQuest;
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class KillsInOneBattleEveryDayQuestNode : QuestItemGUISystem.OldQuestGUINode
        {
            public KillsInOneBattleEveryDayQuestComponent killsInOneBattleEveryDayQuest;
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class KillsInOneBattleQuestNode : QuestItemGUISystem.OldQuestGUINode
        {
            public KillsInOneBattleQuestComponent killsInOneBattleQuest;
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        [Not(typeof(TakenBonusComponent))]
        public class NotTakenQuestBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public QuestExchangeBonusComponent questExchangeBonus;
        }

        public class OldQuestGUINode : QuestItemGUISystem.OldQuestNode
        {
            public QuestItemGUIComponent questItemGUI;
            public QuestProgressGUIComponent questProgressGUI;
            public QuestRewardGUIComponent questRewardGUI;
        }

        public class OldQuestNode : QuestItemGUISystem.QuestNode
        {
            public UserRankComponent userRank;
            public QuestVariationsComponent questVariations;
        }

        public class QuestConditionNode : QuestItemGUISystem.DailyQuestGUINode
        {
            public QuestConditionComponent questCondition;
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class QuestDescriptionNode : QuestItemGUISystem.DailyQuestGUINode
        {
            public QuestConditionDescriptionComponent questConditionDescription;
        }

        public class QuestExpireDateNode : Node
        {
            public QuestExpireDateComponent questExpireDate;
            public QuestExpireTimerComponent questExpireTimer;
        }

        public class QuestGUINode : QuestItemGUISystem.QuestNode
        {
            public QuestItemGUIComponent questItemGUI;
            public QuestProgressGUIComponent questProgressGUI;
            public QuestRewardGUIComponent questRewardGUI;
        }

        public class QuestImageNode : QuestItemGUISystem.QuestGUINode
        {
            public ImageItemComponent imageItem;
        }

        public class QuestNode : Node
        {
            public QuestComponent quest;
            public QuestProgressComponent questProgress;
        }

        public class QuestRarityNode : QuestItemGUISystem.DailyQuestGUINode
        {
            public QuestRarityComponent questRarity;
            public QuestExpireDateComponent questExpireDate;
        }

        public class QuestResultGUINode : QuestItemGUISystem.QuestNode
        {
            public QuestItemGUIComponent questItemGUI;
            public QuestResultComponent questResult;
        }

        [Not(typeof(QuestProgressAnimatorComponent))]
        public class QuestWithoutProgressNode : QuestItemGUISystem.QuestGUINode
        {
        }

        public class QuestWithProgressNode : QuestItemGUISystem.QuestGUINode
        {
            public QuestProgressAnimatorComponent questProgressAnimator;
        }

        public class SelfPremiumQuestUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public PremiumAccountQuestComponent premiumAccountQuest;
        }

        public class ShowQuestCompleteEvent : Event
        {
        }

        public class TakenQuestBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public QuestExchangeBonusComponent questExchangeBonus;
            public TakenBonusComponent takenBonus;
        }
    }
}

