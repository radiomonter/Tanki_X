namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class VulcanWeaponControllerSystem : ECSSystem
    {
        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SelfTankDeadState deadTankNode, [JoinByTank] SingleNode<VulcanComponent> vulcan)
        {
            Entity entity = vulcan.Entity;
            entity.RemoveComponentIfPresent<VulcanIdleComponent>();
            entity.RemoveComponentIfPresent<VulcanSlowDownComponent>();
            entity.RemoveComponentIfPresent<WeaponStreamShootingComponent>();
            entity.RemoveComponentIfPresent<VulcanSpeedUpComponent>();
        }

        [OnEventFire]
        public void DefineFirstStateOnSelfTank(NodeAddedEvent evt, SelfActiveTankNode selfActiveTank, [Context, JoinByTank] VulcanWeaponNode vulcan)
        {
            vulcan.Entity.AddComponentIfAbsent<VulcanIdleComponent>();
        }

        [OnEventFire]
        public void InitShootingStateOnAnyTank(NodeAddedEvent evt, VulcanWeaponShootingNode vulcanShooting)
        {
            vulcanShooting.vulcanWeaponState.State = 1f;
        }

        [OnEventFire]
        public void ResetState(VulcanResetStateEvent e, SingleNode<VulcanComponent> vulcan)
        {
            Entity entity = vulcan.Entity;
            entity.RemoveComponentIfPresent<VulcanSlowDownComponent>();
            entity.RemoveComponentIfPresent<WeaponStreamShootingComponent>();
            entity.RemoveComponentIfPresent<VulcanSpeedUpComponent>();
            entity.AddComponentIfAbsent<VulcanIdleComponent>();
        }

        [OnEventFire]
        public void ScheduleEventTankHeatingOnSelfTank(NodeAddedEvent evt, VulcanWeaponStreamHitShootingControllerNode vulcanShooting)
        {
            base.ScheduleEvent<BeforeShotEvent>(vulcanShooting);
            base.ScheduleEvent<ShotPrepareEvent>(vulcanShooting);
        }

        [OnEventFire]
        public void StartHitTargetCycleOnSelfTank(NodeAddedEvent evt, VulcanWeaponShootingControllerNode vulcanShooting)
        {
            vulcanShooting.Entity.AddComponent<StreamHitCheckingComponent>();
        }

        [OnEventFire]
        public void StartIdleStateOnAnyTank(NodeAddedEvent evt, VulcanWeaponIdleNode vulcanIdle)
        {
            vulcanIdle.vulcanWeaponState.State = 0f;
        }

        [OnEventFire]
        public void StopHitTargetCycleOnSelfTank(NodeRemoveEvent evt, VulcanWeaponShootingControllerNode vulcanShooting)
        {
            vulcanShooting.Entity.RemoveComponent<StreamHitCheckingComponent>();
        }

        [OnEventComplete]
        public void SwitchFromShootingToIdleWhenSelfTankInactive(NodeRemoveEvent evt, SelfActiveTankNode selfActiveTank, [JoinByTank] VulcanWeaponShootingControllerNode vulcanShooting)
        {
            Entity entity = vulcanShooting.Entity;
            entity.RemoveComponent<WeaponStreamShootingComponent>();
            entity.AddComponent<VulcanIdleComponent>();
        }

        [OnEventComplete]
        public void SwitchFromSlowDownToIdleWhenSelfTankInactive(NodeRemoveEvent evt, SelfActiveTankNode selfActiveTank, [JoinByTank] VulcanWeaponSlowDownControllerNode vulcanSlowDown)
        {
            Entity entity = vulcanSlowDown.Entity;
            entity.RemoveComponent<VulcanSlowDownComponent>();
            entity.AddComponent<VulcanIdleComponent>();
        }

        [OnEventFire]
        public void SwitchFromSpeedUpToIdleWhenSelfTankInactive(NodeRemoveEvent evt, SelfActiveTankNode selfActiveTank, [JoinByTank] VulcanWeaponSpeedUpControllerNode vulcanSpeedUp)
        {
            Entity entity = vulcanSpeedUp.Entity;
            entity.RemoveComponent<VulcanSpeedUpComponent>();
            entity.AddComponent<VulcanIdleComponent>();
        }

        private void SwitchVulcanFromIdleToSpeedUp(Entity weapon)
        {
            weapon.RemoveComponent<VulcanIdleComponent>();
            weapon.AddComponent<VulcanSpeedUpComponent>();
        }

        private void SwitchVulcanFromShootingToSlowDown(Entity weapon)
        {
            weapon.RemoveComponent<WeaponStreamShootingComponent>();
            weapon.AddComponent(new VulcanSlowDownComponent(true));
        }

        private void SwitchVulcanFromSlowDownToIdle(Entity weapon)
        {
            weapon.RemoveComponent<VulcanSlowDownComponent>();
            weapon.AddComponent<VulcanIdleComponent>();
        }

        private void SwitchVulcanFromSpeedUpToSlowDown(Entity weapon)
        {
            weapon.RemoveComponent<VulcanSpeedUpComponent>();
            weapon.AddComponent(new VulcanSlowDownComponent(false));
        }

        private void SwitchVulcanFroomSpeedUpToShooting(Entity weapon)
        {
            weapon.RemoveComponent<VulcanSpeedUpComponent>();
            weapon.AddComponent(new WeaponStreamShootingComponent(Date.Now));
        }

        [OnEventFire]
        public void UpdateIdleStateOnSelfTank(TimeUpdateEvent evt, VulcanWeaponIdleControllerNode vulcanIdle, [JoinSelf] SingleNode<ShootableComponent> node, [JoinByTank] SelfActiveTankNode tank)
        {
            if (InputManager.CheckAction(ShotActions.SHOT))
            {
                this.SwitchVulcanFromIdleToSpeedUp(vulcanIdle.Entity);
            }
        }

        [OnEventFire]
        public void UpdateSlowDownStateOnAnyTank(TimeUpdateEvent evt, VulcanWeaponSlowDownNode vulcanSlowDown)
        {
            VulcanWeaponStateComponent component;
            float num = 1f / vulcanSlowDown.vulcanWeapon.SlowDownTime;
            vulcanSlowDown.vulcanWeaponState.State = component.State - (num * evt.DeltaTime);
        }

        [OnEventComplete]
        public void UpdateSlowDownStateOnSelfTank(EarlyUpdateEvent evt, VulcanWeaponSlowDownControllerNode vulcanSlowDown, [JoinByTank] SelfActiveTankNode tank)
        {
            Entity weapon = vulcanSlowDown.Entity;
            if (vulcanSlowDown.vulcanWeaponState.State <= 0f)
            {
                this.SwitchVulcanFromSlowDownToIdle(weapon);
            }
        }

        [OnEventComplete]
        public void UpdateSpeedStateUpOnSelfTank(EarlyUpdateEvent evt, VulcanWeaponSpeedUpControllerNode vulcanSpeedUp, [JoinByTank] SelfActiveTankNode tank)
        {
            Entity weapon = vulcanSpeedUp.Entity;
            if (!InputManager.CheckAction(ShotActions.SHOT))
            {
                this.SwitchVulcanFromSpeedUpToSlowDown(weapon);
            }
            else if (vulcanSpeedUp.vulcanWeaponState.State >= 1f)
            {
                this.SwitchVulcanFroomSpeedUpToShooting(weapon);
            }
        }

        [OnEventComplete]
        public void UpdateSpeedStateUpOnSelfTank(ApplicationFocusEvent evt, VulcanWeaponSpeedUpControllerNode vulcanSpeedUp, [JoinByTank] SelfActiveTankNode tank)
        {
            Entity weapon = vulcanSpeedUp.Entity;
            if (!evt.IsFocused)
            {
                this.SwitchVulcanFromSpeedUpToSlowDown(weapon);
            }
        }

        [OnEventFire]
        public void UpdateSpeedUpStateOnAnyTank(TimeUpdateEvent e, VulcanWeaponSpeedUpNode vulcanSpeedUp)
        {
            VulcanWeaponStateComponent component;
            float num = 1f / vulcanSpeedUp.vulcanWeapon.SpeedUpTime;
            vulcanSpeedUp.vulcanWeaponState.State = component.State + (e.DeltaTime * num);
        }

        [OnEventFire]
        public void UpdateVulcanShootingOnSelfTank(EarlyUpdateEvent evt, VulcanWeaponShootingControllerNode vulcanShooting, [JoinByTank] SelfActiveTankNode tank)
        {
            if (!InputManager.CheckAction(ShotActions.SHOT))
            {
                this.SwitchVulcanFromShootingToSlowDown(vulcanShooting.Entity);
            }
        }

        [OnEventFire]
        public void UpdateVulcanShootingOnSelfTank(EarlyUpdateEvent evt, VulcanWeaponStreamHitShootingControllerNode vulcanShooting, [JoinByTank] SelfActiveTankNode tank)
        {
            if (vulcanShooting.cooldownTimer.CooldownTimerSec <= 0f)
            {
                base.ScheduleEvent<BeforeShotEvent>(vulcanShooting);
                base.ScheduleEvent<ShotPrepareEvent>(vulcanShooting);
            }
        }

        [OnEventFire]
        public void UpdateVulcanShootingOnSelfTank(ApplicationFocusEvent evt, VulcanWeaponShootingControllerNode vulcanShooting, [JoinByTank] SelfActiveTankNode tank)
        {
            if (!evt.IsFocused)
            {
                this.SwitchVulcanFromShootingToSlowDown(vulcanShooting.Entity);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
            public SelfTankComponent selfTank;
        }

        public class SelfTankDeadState : Node
        {
            public SelfTankComponent selfTank;
            public TankDeadStateComponent tankDeadState;
        }

        public class VulcanWeaponIdleControllerNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanIdleComponent vulcanIdle;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class VulcanWeaponIdleNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanWeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponShootingControllerNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public WeaponStreamShootingComponent weaponStreamShooting;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class VulcanWeaponShootingNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public WeaponStreamShootingComponent weaponStreamShooting;
            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponSlowDownControllerNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanSlowDownComponent vulcanSlowDown;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class VulcanWeaponSlowDownNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanSlowDownComponent vulcanSlowDown;
            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponSpeedUpControllerNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanSpeedUpComponent vulcanSpeedUp;
            public VulcanWeaponStateComponent vulcanWeaponState;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class VulcanWeaponSpeedUpNode : Node
        {
            public VulcanWeaponComponent vulcanWeapon;
            public VulcanSpeedUpComponent vulcanSpeedUp;
            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanWeaponStreamHitShootingControllerNode : VulcanWeaponControllerSystem.VulcanWeaponShootingControllerNode
        {
            public StreamHitComponent streamHit;
        }
    }
}

