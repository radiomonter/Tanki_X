namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class IsisSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableStreamHit(NodeRemoveEvent e, WorkingNode weapon, [Context, JoinByTank] SelfTankNode tank)
        {
            weapon.Entity.RemoveComponentIfPresent<StreamHitCheckingComponent>();
        }

        [OnEventFire]
        public void EnableStreamHit(NodeAddedEvent e, WorkingNode weapon, [Context, JoinByTank] SelfTankNode tank)
        {
            weapon.Entity.AddComponentIfAbsent<StreamHitCheckingComponent>();
        }

        [OnEventFire]
        public void SetMaskForIsisTargeting(NodeAddedEvent evt, IsisTargetCollectorNode weapon)
        {
            weapon.targetCollector.TargetValidator.LayerMask = LayerMasks.GUN_TARGETING_WITHOUT_DEAD_UNITS;
        }

        public class IsisTargetCollectorNode : Node
        {
            public IsisComponent isis;
            public TargetCollectorComponent targetCollector;
        }

        public class SelfTankNode : Node
        {
            public UserGroupComponent userGroup;
            public SelfTankComponent selfTank;
        }

        public class WorkingNode : Node
        {
            public IsisComponent isis;
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

