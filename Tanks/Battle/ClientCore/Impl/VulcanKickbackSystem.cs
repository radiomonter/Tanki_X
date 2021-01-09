namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class VulcanKickbackSystem : ECSSystem
    {
        [OnEventFire]
        public void ApplyKickback(FixedUpdateEvent evt, KickbackNode weapon, [JoinByTank] TankNode tank)
        {
            KickbackComponent kickback = weapon.kickback;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            float deltaTime = evt.DeltaTime;
            VulcanPhysicsUtils.ApplyVulcanForce(((-accessor.GetFireDirectionWorld() * kickback.KickbackForce) * WeaponConstants.WEAPON_FORCE_MULTIPLIER) * deltaTime, tank.rigidbody.Rigidbody, accessor.GetWorldMiddlePosition(), tank.tankFalling, tank.track);
        }

        public class KickbackNode : Node
        {
            public KickbackComponent kickback;
            public MuzzlePointComponent muzzlePoint;
            public WeaponStreamShootingComponent weaponStreamShooting;
            public WeaponInstanceComponent weaponInstance;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
            public TrackComponent track;
            public TankFallingComponent tankFalling;
        }
    }
}

