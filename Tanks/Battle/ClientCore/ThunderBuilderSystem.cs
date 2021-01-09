namespace Tanks.Battle.ClientCore
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class ThunderBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, ThunderNode weaponNode, [JoinByBattle] SingleNode<CTFComponent> battle)
        {
            weaponNode.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, ThunderNode weaponNode, [JoinByBattle] SingleNode<TeamBattleComponent> battle)
        {
            weaponNode.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void BuildBot(NodeAddedEvent evt, AutopilotTankNode botTank, [Context, JoinByUser] ThunderNode weaponNode, [Context, JoinByUser] SingleNode<UserComponent> userNode)
        {
            this.BuildWeapon(botTank.Entity, weaponNode);
        }

        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] ThunderNode weaponNode, [JoinByUser] SingleNode<UserComponent> userNode)
        {
            this.BuildWeapon(selfTank.Entity, weaponNode);
        }

        private void BuildWeapon(Entity tank, ThunderNode weaponNode)
        {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<DiscreteWeaponControllerComponent>();
            entity.AddComponent<WeaponShotComponent>();
            entity.AddComponent<VerticalSectorsTargetingComponent>();
            entity.AddComponent<DirectionEvaluatorComponent>();
            entity.AddComponent<DistanceAndAngleTargetEvaluatorComponent>();
            entity.AddComponent(new TargetCollectorComponent(new TargetCollector(tank), new TargetValidator(tank)));
        }

        public class AutopilotTankNode : Node
        {
            public UserGroupComponent userGroup;
            public TankAutopilotComponent tankAutopilot;
            public AssembledTankComponent assembledTank;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public UserGroupComponent userGroup;
            public AssembledTankComponent assembledTank;
        }

        public class ThunderNode : Node
        {
            public ThunderComponent thunder;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }
    }
}

