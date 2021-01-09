namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftAimingShotSystem : ECSSystem
    {
        [OnEventFire]
        public void SendShot(ShaftAimingShotPrepareEvent evt, BlockedShaftNode weapon)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            base.ScheduleEvent(new SelfShotEvent(accessor.GetFireDirectionWorld()), weapon);
        }

        [OnEventFire]
        public void SendShot(ShaftAimingShotPrepareEvent evt, UndergroundShaftNode weapon)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            base.ScheduleEvent(new SelfShotEvent(accessor.GetFireDirectionWorld()), weapon);
        }

        public class BlockedShaftNode : ShaftAimingShotSystem.ShaftNode
        {
            public WeaponBlockedComponent weaponBlocked;
        }

        public class ShaftNode : Node
        {
            public WeaponShotComponent weaponShot;
            public MuzzlePointComponent muzzlePoint;
            public ShaftComponent shaft;
            public WeaponInstanceComponent weaponInstance;
        }

        public class UndergroundShaftNode : ShaftAimingShotSystem.ShaftNode
        {
            public WeaponUndergroundComponent weaponUnderground;
        }
    }
}

