namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class VulcanBuilderSystem : ECSSystem
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
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode, [Context, JoinByUser] UserNode user)
        {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent(new WeaponHitComponent(false, false));
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            entity.AddComponent(new TargetCollectorComponent(new TargetCollector(selfTank.Entity), new TargetValidator(selfTank.Entity)));
        }

        public class SelfTankNode : Node
        {
            public UserGroupComponent userGroup;
            public SelfTankComponent selfTank;
            public AssembledTankComponent assembledTank;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
        }

        public class WeaponNode : Node
        {
            public UserGroupComponent userGroup;
            public VulcanComponent vulcan;
            public BattleGroupComponent battleGroup;
        }
    }
}

