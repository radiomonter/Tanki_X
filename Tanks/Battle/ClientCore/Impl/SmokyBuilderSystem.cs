namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class SmokyBuilderSystem : ECSSystem
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
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context, JoinByUser] WeaponNode weaponNode, [Context, JoinByUser] UserNode user)
        {
            this.BuildSelf(weaponNode.Entity, botTank.Entity);
        }

        public void BuildSelf(Entity weapon, Entity tank)
        {
            weapon.AddComponent<CooldownTimerComponent>();
            weapon.AddComponent<DiscreteWeaponControllerComponent>();
            weapon.AddComponent<VerticalSectorsTargetingComponent>();
            weapon.AddComponent<DirectionEvaluatorComponent>();
            weapon.AddComponent<WeaponShotComponent>();
            weapon.AddComponent(new WeaponHitComponent(true, false));
            weapon.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            TargetCollectorComponent component = new TargetCollectorComponent(new TargetCollector(tank), new TargetValidator(tank));
            weapon.AddComponent(component);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode, [Context, JoinByUser] UserNode user)
        {
            this.BuildSelf(weaponNode.Entity, selfTank.Entity);
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

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
        }

        public class WeaponNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
            public SmokyComponent smoky;
        }
    }
}

