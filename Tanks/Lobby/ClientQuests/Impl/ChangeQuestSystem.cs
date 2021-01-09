namespace Tanks.Lobby.ClientQuests.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientQuests.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class ChangeQuestSystem : ECSSystem
    {
        [OnEventFire]
        public void ChangeQuests(ChangeQuestEvent e, QuestNode quest, [JoinByUser] QuestBonusNode bonus)
        {
            base.NewEvent<UseBonusEvent>().Attach(bonus).Attach(quest).Schedule();
        }

        [Not(typeof(TakenBonusComponent))]
        public class QuestBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public QuestExchangeBonusComponent questExchangeBonus;
        }

        public class QuestNode : Node
        {
            public QuestComponent quest;
            public QuestRarityComponent questRarity;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public LeagueGroupComponent leagueGroup;
        }
    }
}

