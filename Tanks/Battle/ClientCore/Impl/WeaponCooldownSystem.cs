namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponCooldownSystem : AbstractWeaponCooldownSystem
    {
        [OnEventFire]
        public void DecreaseCooldownTimer(TimeUpdateEvent evt, CooldownNode cooldown)
        {
            cooldown.cooldownTimer.CooldownTimerSec = Mathf.Max((float) (cooldown.cooldownTimer.CooldownTimerSec - evt.DeltaTime), (float) 0f);
        }

        [OnEventFire]
        public void DefineCooldownTimerForNextPossibleShot(ShotPrepareEvent evt, CooldownNode cooldown)
        {
            base.UpdateCooldownOnShot(cooldown.cooldownTimer, cooldown.weaponCooldown);
        }

        [OnEventFire]
        public void ResetOnActivate(NodeAddedEvent e, ActiveTankNode activeTank, [JoinByTank] CooldownNode cooldown)
        {
            cooldown.cooldownTimer.CooldownTimerSec = 0f;
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class CooldownNode : Node
        {
            public CooldownTimerComponent cooldownTimer;
            public WeaponCooldownComponent weaponCooldown;
        }
    }
}

