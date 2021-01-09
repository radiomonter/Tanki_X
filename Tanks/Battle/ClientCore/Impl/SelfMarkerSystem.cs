namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class SelfMarkerSystem : ECSSystem
    {
        [OnEventFire]
        public void MarkBattle(NodeAddedEvent e, SelfBattleUserNode battleUser, [JoinByBattle] BattleNode battle)
        {
            battle.Entity.AddComponent<SelfComponent>();
        }

        [OnEventFire]
        public void MarkSelfBattleUser(NodeAddedEvent e, BattleUserNode battleUser, [Context, JoinByUser] SelfUserNode user)
        {
            battleUser.Entity.AddComponent<SelfBattleUserComponent>();
        }

        [OnEventFire]
        public void MarkTank(NodeAddedEvent e, TankNode tank, [Context, JoinByUser] UserNode user)
        {
            if (user.Entity.HasComponent<SelfComponent>())
            {
                tank.Entity.AddComponent<SelfTankComponent>();
            }
            else
            {
                tank.Entity.AddComponent<RemoteTankComponent>();
            }
        }

        [OnEventFire]
        public void UnmarkBattle(NodeRemoveEvent e, SelfBattleUserNode battleUser, [JoinByBattle] BattleNode battle)
        {
            battle.Entity.RemoveComponent<SelfComponent>();
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public SelfComponent self;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class SelfUserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
            public SelfComponent self;
        }

        public class TankNode : Node
        {
            public UserGroupComponent userGroup;
            public TankComponent tank;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
        }
    }
}

