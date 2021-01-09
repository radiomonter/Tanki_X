namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, WeaponNode weaponNode, [JoinByBattle] SingleNode<CTFComponent> battle)
        {
            weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, WeaponNode weaponNode, [JoinByBattle] SingleNode<TeamBattleComponent> battle)
        {
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Build(NodeAddedEvent evt, TankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode)
        {
            TargetCollectorComponent component = new TargetCollectorComponent(new TargetCollector(selfTank.Entity), new TargetValidator(selfTank.Entity));
            weaponNode.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode, [Context, JoinByUser] UserNode user)
        {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<ShaftStateControllerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
        }

        public class CTFBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public CTFComponent ctf;
        }

        public class DMBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public DMComponent dm;
        }

        public class SelfTankNode : Node
        {
            public UserGroupComponent userGroup;
            public SelfTankComponent selfTank;
            public AssembledTankComponent assembledTank;
        }

        public class TankNode : Node
        {
            public UserGroupComponent userGroup;
            public TankComponent tank;
            public AssembledTankComponent assembledTank;
        }

        public class TDMBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public TDMComponent tdm;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
        }

        public class WeaponNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
            public ShaftComponent shaft;
        }
    }
}

