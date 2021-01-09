namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class KickbackSystem : ECSSystem
    {
        [OnEventFire]
        public void StartKickback(BaseShotEvent evt, KickbackNode weapon, [JoinByTank] TankNode tank)
        {
            KickbackComponent kickback = weapon.kickback;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            Vector3 vector = -accessor.GetFireDirectionWorld() * kickback.KickbackForce;
            Vector3 worldPosition = accessor.GetWorldPosition();
            tank.rigidbody.Rigidbody.AddForceAtPositionSafe(vector * WeaponConstants.WEAPON_FORCE_MULTIPLIER, worldPosition);
        }

        public class KickbackNode : Node
        {
            public KickbackComponent kickback;
            public TankGroupComponent tankGroup;
            public MuzzlePointComponent muzzlePoint;
            public DiscreteWeaponComponent discreteWeapon;
            public WeaponInstanceComponent weaponInstance;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
        }
    }
}

