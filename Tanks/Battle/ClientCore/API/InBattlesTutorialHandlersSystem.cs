namespace Tanks.Battle.ClientCore.API
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class InBattlesTutorialHandlersSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckForBattleResultReward(NodeAddedEvent e, SingleNode<BattleResultRewardCheckComponent> node, [JoinAll] ICollection<TutorialNode> tutorials, [JoinAll] SelfBattleUserNode selfBattleUser, [JoinByUser] SelfRoundUser selfRoundUser)
        {
            CheckForQuickGameEvent eventInstance = new CheckForQuickGameEvent();
            base.ScheduleEvent(eventInstance, node);
            if (eventInstance.IsQuickGame)
            {
                long quickBattleEndTutorialId = node.component.QuickBattleEndTutorialId;
                bool flag = false;
                foreach (TutorialNode node2 in tutorials)
                {
                    if ((node2.tutorialData.TutorialId == quickBattleEndTutorialId) && !node2.Entity.HasComponent<TutorialCompleteComponent>())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    base.ScheduleEvent<TutorialTriggeredEvent>(selfRoundUser);
                }
            }
        }

        public class SelfBattleUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class SelfRoundUser : Node
        {
            public RoundUserStatisticsComponent roundUserStatistics;
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
        }

        public class TutorialNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialGroupComponent tutorialGroup;
            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }
    }
}

