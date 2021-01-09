namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftAimingCooldownSystem : AbstractWeaponCooldownSystem
    {
        [OnEventFire]
        public void DefineCooldownTimerForNextPossibleShot(ShaftAimingShotPrepareEvent evt, CooldownNode cooldown)
        {
            base.UpdateCooldownOnShot(cooldown.cooldownTimer, cooldown.weaponCooldown);
        }

        public class CooldownNode : Node
        {
            public CooldownTimerComponent cooldownTimer;
            public WeaponCooldownComponent weaponCooldown;
        }
    }
}

