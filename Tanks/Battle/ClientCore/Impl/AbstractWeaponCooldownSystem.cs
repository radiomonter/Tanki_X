namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public abstract class AbstractWeaponCooldownSystem : ECSSystem
    {
        protected AbstractWeaponCooldownSystem()
        {
        }

        protected void UpdateCooldownOnShot(CooldownTimerComponent cooldownTimerComponent, WeaponCooldownComponent weaponCooldownComponent)
        {
            float cooldownIntervalSec = weaponCooldownComponent.CooldownIntervalSec;
            cooldownTimerComponent.CooldownTimerSec = cooldownIntervalSec;
        }
    }
}

