namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class MapPhysicsSystem : ECSSystem
    {
        [OnEventFire]
        public void InitGravity(NodeAddedEvent e, SelfBattleUserNode selfBattleUser, [JoinByBattle, Mandatory] BattleNode battle)
        {
            Physics.gravity = Vector3.down * battle.gravity.Gravity;
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public GravityComponent gravity;
            public MapGroupComponent mapGroup;
            public BattleGroupComponent battleGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfComponent self;
            public BattleUserComponent battleUser;
            public BattleGroupComponent battleGroup;
        }
    }
}

