namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class FreezeBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildSelf(NodeAddedEvent evt, SelfTankNode selfTank, [Context, JoinByUser] WeaponNode weaponNode, [Context, JoinByUser] UserNode user)
        {
            Entity entity = weaponNode.Entity;
            entity.AddComponent<CooldownTimerComponent>();
            entity.AddComponent<StreamWeaponControllerComponent>();
            entity.AddComponent<ConicTargetingComponent>();
            entity.AddComponent(new WeaponHitComponent(false, true));
            entity.AddComponent(new StreamWeaponSimpleFeedbackControllerComponent());
            entity.AddComponent(new TargetCollectorComponent(new TargetCollector(selfTank.Entity), new PenetrationTargetValidator(selfTank.Entity)));
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
            public FreezeComponent freeze;
            public BattleGroupComponent battleGroup;
        }
    }
}

