namespace Tanks.Lobby.ClientQuests.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientQuests.API;

    public class QuestsRewardBufferSystem : ECSSystem
    {
        [OnEventFire]
        public void AddQuestRewardToBuffer(NodeAddedEvent e, RewardedNotResultNewQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals)
        {
            this.AddToBuffer(questNode.questReward.Reward, user, crystals, xCrystals);
        }

        [OnEventFire]
        public void AddQuestRewardToBuffer(NodeAddedEvent e, RewardedNotResultOldQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals)
        {
            <AddQuestRewardToBuffer>c__AnonStorey1 storey = new <AddQuestRewardToBuffer>c__AnonStorey1 {
                questNode = questNode
            };
            QuestParameters parameters = storey.questNode.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            this.AddToBuffer(parameters.QuestReward, user, crystals, xCrystals);
        }

        private void AddToBuffer(Dictionary<long, int> reward, SelfUserNode user, CrystalMarketItemNode crystals, XCrystalMarketItemNode xCrystals)
        {
            ChangeUserMoneyBufferEvent eventInstance = new ChangeUserMoneyBufferEvent {
                Crystals = this.GetItemCount(reward, crystals.Entity.Id),
                XCrystals = this.GetItemCount(reward, xCrystals.Entity.Id)
            };
            base.NewEvent(eventInstance).Attach(user).Schedule();
        }

        private void DeleteFromBuffer(Dictionary<long, int> reward, SelfUserNode user, CrystalMarketItemNode crystals, XCrystalMarketItemNode xCrystals)
        {
            ChangeUserMoneyBufferEvent eventInstance = new ChangeUserMoneyBufferEvent {
                Crystals = -this.GetItemCount(reward, crystals.Entity.Id),
                XCrystals = -this.GetItemCount(reward, xCrystals.Entity.Id)
            };
            base.NewEvent(eventInstance).Attach(user).Schedule();
        }

        [OnEventFire]
        public void DeleteQuestRewardFromBuffer(QuestsScreenSystem.TryShowQuestRewardNotification e, RewardedNewQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals)
        {
            this.DeleteFromBuffer(questNode.questReward.Reward, user, crystals, xCrystals);
        }

        [OnEventFire]
        public void DeleteQuestRewardFromBuffer(QuestsScreenSystem.TryShowQuestRewardNotification e, RewardedOldQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals)
        {
            <DeleteQuestRewardFromBuffer>c__AnonStorey0 storey = new <DeleteQuestRewardFromBuffer>c__AnonStorey0 {
                questNode = questNode
            };
            QuestParameters parameters = storey.questNode.questVariations.Quests.Find(new Predicate<QuestParameters>(storey.<>m__0));
            this.DeleteFromBuffer(parameters.QuestReward, user, crystals, xCrystals);
        }

        private int GetItemCount(Dictionary<long, int> items, long itemId) => 
            ((items == null) || !items.ContainsKey(itemId)) ? 0 : items[itemId];

        [CompilerGenerated]
        private sealed class <AddQuestRewardToBuffer>c__AnonStorey1
        {
            internal QuestsRewardBufferSystem.RewardedNotResultOldQuestNode questNode;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.questNode.userRank.Rank);
        }

        [CompilerGenerated]
        private sealed class <DeleteQuestRewardFromBuffer>c__AnonStorey0
        {
            internal QuestsRewardBufferSystem.RewardedOldQuestNode questNode;

            internal bool <>m__0(QuestParameters r) => 
                IndexRange.ParseString(r.Range).Contains(this.questNode.userRank.Rank);
        }

        public class CrystalMarketItemNode : Node
        {
            public CrystalItemComponent crystalItem;
            public MarketItemComponent marketItem;
        }

        public class RewardedNewQuestNode : QuestsRewardBufferSystem.RewardedQuestNode
        {
            public QuestRewardComponent questReward;
        }

        [Not(typeof(QuestResultComponent))]
        public class RewardedNotResultNewQuestNode : QuestsRewardBufferSystem.RewardedNewQuestNode
        {
        }

        [Not(typeof(QuestResultComponent))]
        public class RewardedNotResultOldQuestNode : QuestsRewardBufferSystem.RewardedOldQuestNode
        {
        }

        public class RewardedOldQuestNode : QuestsRewardBufferSystem.RewardedQuestNode
        {
            public UserRankComponent userRank;
            public QuestVariationsComponent questVariations;
        }

        public class RewardedQuestNode : Node
        {
            public QuestComponent quest;
            public QuestOrderComponent questOrder;
            public QuestProgressComponent questProgress;
            public RewardedQuestComponent rewardedQuest;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfComponent self;
            public UserMoneyComponent userMoney;
        }

        public class XCrystalMarketItemNode : Node
        {
            public XCrystalItemComponent xCrystalItem;
            public MarketItemComponent marketItem;
        }
    }
}

