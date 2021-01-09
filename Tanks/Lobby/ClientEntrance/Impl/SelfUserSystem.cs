namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class SelfUserSystem : ECSSystem
    {
        [OnEventFire]
        public void DoNothing(NodeAddedEvent e, SingleNode<LobbyComponent> n)
        {
        }

        [OnEventFire]
        public void MarkSelf(NodeAddedEvent e, [Combine] UserGroupNode userGroup, [Context, JoinByUser] SelfUserNode selfUser)
        {
            if (!userGroup.Entity.HasComponent<SelfComponent>())
            {
                userGroup.Entity.AddComponent<SelfComponent>();
            }
        }

        [OnEventFire]
        public void MarkSelfUser(NodeAddedEvent e, UserNode user, [Context, JoinByUser] ClientSessionNode clientSession)
        {
            user.Entity.AddComponent<SelfUserComponent>();
        }

        [OnEventFire]
        public void RemoveSelfUser(NodeRemoveEvent e, UserNode user, [JoinByUser] ClientSessionNode clientSession)
        {
            throw new WhiteWalkerException();
        }

        public class ClientSessionNode : Node
        {
            public UserGroupComponent userGroup;
            public ClientSessionComponent clientSession;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }

        public class UserGroupNode : Node
        {
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }
    }
}

