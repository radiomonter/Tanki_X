namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftStateControllerSystem : ECSSystem
    {
        private HashSet<Type> weaponStates = new HashSet<Type>();

        public ShaftStateControllerSystem()
        {
            this.weaponStates.Add(typeof(ShaftIdleStateComponent));
            this.weaponStates.Add(typeof(ShaftWaitingStateComponent));
            this.weaponStates.Add(typeof(ShaftAimingWorkActivationStateComponent));
            this.weaponStates.Add(typeof(ShaftAimingWorkingStateComponent));
            this.weaponStates.Add(typeof(ShaftAimingWorkFinishStateComponent));
        }

        private bool CheckHandleWeaponIntersectionStatus(Entity weapon)
        {
            bool flag = weapon.HasComponent<WeaponUndergroundComponent>();
            if (flag)
            {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon, this.weaponStates);
            }
            return flag;
        }

        [OnEventFire]
        public void CheckWaitingState(TimeUpdateEvent evt, ShaftWaitingWeaponControllerNode weapon)
        {
            if (InputManager.CheckAction(ShotActions.SHOT))
            {
                this.StartWorkActivationStateIfPossible(weapon, evt.DeltaTime);
            }
            else
            {
                this.StartQuickShotIfPossible(weapon);
            }
        }

        [OnEventFire]
        public void CheckWeaponStateOnInactiveTank(NodeRemoveEvent evt, SelfActiveTankNode tank, [JoinByTank] ShaftWeaponNode weapon)
        {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, this.weaponStates);
        }

        [OnEventFire]
        public void CheckWorkActivationState(TimeUpdateEvent evt, ShaftAimingWorkActivationWeaponControllerNode weapon)
        {
            if (!this.CheckHandleWeaponIntersectionStatus(weapon.Entity))
            {
                if (!InputManager.CheckAction(ShotActions.SHOT))
                {
                    this.MakeQuickShot(weapon.Entity);
                }
                else if (weapon.shaftAimingWorkActivationState.ActivationTimer < weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec)
                {
                    weapon.shaftAimingWorkActivationState.ActivationTimer += evt.DeltaTime;
                }
                else
                {
                    MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
                    ShaftAimingWorkingStateComponent component = new ShaftAimingWorkingStateComponent {
                        InitialEnergy = weapon.weaponEnergy.Energy,
                        WorkingDirection = accessor.GetFireDirectionWorld()
                    };
                    StateUtils.SwitchEntityState(weapon.Entity, component, this.weaponStates);
                }
            }
        }

        [OnEventFire]
        public void CheckWorkFinishState(TimeUpdateEvent evt, ShaftAimingWorkFinishWeaponControllerNode weapon)
        {
            if (weapon.shaftAimingWorkFinishState.FinishTimer < weapon.shaftStateConfig.FinishToIdleTransitionTimeSec)
            {
                weapon.shaftAimingWorkFinishState.FinishTimer += evt.DeltaTime;
            }
            else
            {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, this.weaponStates);
            }
        }

        [OnEventFire]
        public void CheckWorkingState(EarlyUpdateEvent evt, ShaftAimingWorkingWeaponControllerNode weapon)
        {
            if (!this.CheckHandleWeaponIntersectionStatus(weapon.Entity) && !InputManager.CheckAction(ShotActions.SHOT))
            {
                this.MakeAimingShot(weapon.Entity, weapon.shaftAimingWorkingState.WorkingDirection);
            }
        }

        [OnEventFire]
        public void InitIdleState(NodeAddedEvent evt, ShaftWeaponNode weapon)
        {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, this.weaponStates);
        }

        [OnEventFire]
        public void InitWaitingStateOnInput(TimeUpdateEvent evt, ShaftIdleWeaponControllerNode weapon, [JoinByTank] SelfActiveTankNode activeTank)
        {
            this.StartWaitingStateIfPossible(weapon.Entity);
        }

        private void MakeAimingShot(Entity weapon, Vector3 workingDir)
        {
            StateUtils.SwitchEntityState<ShaftAimingWorkFinishStateComponent>(weapon, this.weaponStates);
            if (weapon.HasComponent<ShootableComponent>())
            {
                base.ScheduleEvent<BeforeShotEvent>(weapon);
                base.ScheduleEvent(new ShaftAimingShotPrepareEvent(workingDir), weapon);
            }
        }

        private void MakeQuickShot(Entity weapon)
        {
            StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon, this.weaponStates);
            if (weapon.HasComponent<ShootableComponent>())
            {
                base.ScheduleEvent<BeforeShotEvent>(weapon);
                base.ScheduleEvent<ShotPrepareEvent>(weapon);
            }
        }

        private void StartQuickShotIfPossible(ShaftWaitingWeaponControllerNode weapon)
        {
            CooldownTimerComponent cooldownTimer = weapon.cooldownTimer;
            if (weapon.weaponEnergy.Energy < weapon.shaftEnergy.UnloadEnergyPerQuickShot)
            {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, this.weaponStates);
            }
            else if (cooldownTimer.CooldownTimerSec > 0f)
            {
                StateUtils.SwitchEntityState<ShaftIdleStateComponent>(weapon.Entity, this.weaponStates);
            }
            else
            {
                this.MakeQuickShot(weapon.Entity);
            }
        }

        private void StartWaitingStateIfPossible(Entity weapon)
        {
            if (InputManager.CheckAction(ShotActions.SHOT))
            {
                StateUtils.SwitchEntityState<ShaftWaitingStateComponent>(weapon, this.weaponStates);
            }
        }

        private void StartWorkActivationStateIfPossible(ShaftWaitingWeaponControllerNode weapon, float dt)
        {
            if (!weapon.Entity.HasComponent<WeaponUndergroundComponent>())
            {
                if (weapon.shaftWaitingState.WaitingTimer < weapon.shaftStateConfig.WaitingToActivationTransitionTimeSec)
                {
                    weapon.shaftWaitingState.WaitingTimer += dt;
                }
                else if (weapon.weaponEnergy.Energy >= 1f)
                {
                    StateUtils.SwitchEntityState<ShaftAimingWorkActivationStateComponent>(weapon.Entity, this.weaponStates);
                }
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingWorkActivationWeaponControllerNode : ShaftStateControllerSystem.ShaftWeaponNode
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftAimingWorkFinishWeaponControllerNode : ShaftStateControllerSystem.ShaftWeaponNode
        {
            public ShaftAimingWorkFinishStateComponent shaftAimingWorkFinishState;
        }

        public class ShaftAimingWorkingWeaponControllerNode : ShaftStateControllerSystem.ShaftWeaponNode
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ShaftIdleWeaponControllerNode : ShaftStateControllerSystem.ShaftWeaponNode
        {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftWaitingWeaponControllerNode : ShaftStateControllerSystem.ShaftWeaponNode
        {
            public ShaftWaitingStateComponent shaftWaitingState;
            public ShaftEnergyComponent shaftEnergy;
        }

        public class ShaftWeaponNode : Node
        {
            public WeaponEnergyComponent weaponEnergy;
            public CooldownTimerComponent cooldownTimer;
            public DiscreteWeaponComponent discreteWeapon;
            public ShaftStateConfigComponent shaftStateConfig;
            public ShaftStateControllerComponent shaftStateController;
        }
    }
}

