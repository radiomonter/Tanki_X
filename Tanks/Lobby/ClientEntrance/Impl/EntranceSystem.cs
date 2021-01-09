namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class EntranceSystem : ECSSystem
    {
        [OnEventFire]
        public void SendUserOnlineEvent(NodeAddedEvent e, UserOnlineNode userOnline, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent<UserOnlineEvent>(session);
        }

        [OnEventFire]
        public void SendUserQuestReadyEvent(NodeAddedEvent e, UserQuestReadyNode userNode, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent<UserQuestReadyEvent>(session);
        }

        public class SessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }

        public class UserOnlineNode : Node
        {
            public SelfUserComponent selfUser;
            public UserOnlineComponent userOnline;
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class UserQuestReadyNode : EntranceSystem.UserOnlineNode
        {
            public QuestReadyComponent questReady;
        }
    }
}

