namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class HammerBuilderSystem : ECSSystem
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
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context, JoinByUser] WeaponNode weaponNode)
        {
            this.BuildWeapon(botTank.Entity, weaponNode);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode)
        {
            this.BuildWeapon(selfTank.Entity, weaponNode);
        }

        private void BuildWeapon(Entity tank, WeaponNode weaponNode)
        {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            entity.AddComponent(new WeaponHitComponent(true, false));
            entity.AddComponent(new HammerTargetCollectorComponent(new TargetCollector(tank), new TargetValidator(tank)));
        }

        public class AutopilotTankNode : Node
        {
            public UserGroupComponent userGroup;
            public TankAutopilotComponent tankAutopilot;
            public AssembledTankComponent assembledTank;
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

        public class WeaponNode : Node
        {
            public UserGroupComponent userGroup;
            public HammerComponent hammer;
            public DiscreteWeaponComponent discreteWeapon;
            public BattleGroupComponent battleGroup;
        }
    }
}

