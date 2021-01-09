namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class StreamWeaponCooldownSystem : ECSSystem
    {
        [OnEventFire]
        public void DefineCooldownTimerForTheFirstTickInWorkingState(NodeAddedEvent evt, StreamWeaponCooldownNode weapon)
        {
            weapon.cooldownTimer.CooldownTimerSec = weapon.weaponCooldown.CooldownIntervalSec;
        }

        public class StreamWeaponCooldownNode : Node
        {
            public CooldownTimerComponent cooldownTimer;
            public WeaponCooldownComponent weaponCooldown;
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public StreamWeaponComponent streamWeapon;
        }
    }
}

