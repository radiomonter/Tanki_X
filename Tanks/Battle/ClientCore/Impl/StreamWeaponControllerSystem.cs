namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class StreamWeaponControllerSystem : ECSSystem
    {
        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SelfDeadTankNode deadTank, [JoinByTank] StreamWeaponNode idleWeapon)
        {
            Entity entity = idleWeapon.Entity;
            entity.RemoveComponentIfPresent<StreamWeaponIdleComponent>();
            entity.RemoveComponentIfPresent<StreamWeaponWorkingComponent>();
        }

        [OnEventFire]
        public void InitializeIdleState(NodeAddedEvent evt, SelfActiveTankNode selfActiveTank, [Context, JoinByUser] StreamWeaponShootableNode streamWeapon, [JoinByUser] ICollection<SelfUserNode> user)
        {
            Entity weapon = streamWeapon.Entity;
            if (streamWeapon.weaponEnergy.Energy <= 0f)
            {
                SwitchWorkingModeToIdleMode(weapon);
            }
            else if (InputManager.CheckAction(ShotActions.SHOT))
            {
                SwitchIdleModeToWorkingMode(weapon);
            }
            else
            {
                SwitchWorkingModeToIdleMode(weapon);
            }
        }

        [OnEventComplete]
        public void RunWorkingStream(EarlyUpdateEvent evt, StreamWeaponWorkingControllerNode workingWeapon, [JoinByTank] SelfActiveTankNode selfActiveTank)
        {
            Entity weapon = workingWeapon.Entity;
            CooldownTimerComponent cooldownTimer = workingWeapon.cooldownTimer;
            if (workingWeapon.weaponEnergy.Energy <= 0f)
            {
                SwitchWorkingModeToIdleMode(weapon);
            }
            else if (InputManager.GetActionKeyUp(ShotActions.SHOT))
            {
                SwitchWorkingModeToIdleMode(weapon);
            }
            else if ((cooldownTimer.CooldownTimerSec <= 0f) && workingWeapon.Entity.HasComponent<ShootableComponent>())
            {
                base.ScheduleEvent<BeforeShotEvent>(workingWeapon);
                base.ScheduleEvent<ShotPrepareEvent>(workingWeapon);
            }
        }

        [OnEventFire]
        public void StartStreamWorkingIfPossible(EarlyUpdateEvent evt, StreamWeaponIdleControllerNode idleWeapon, [JoinSelf] SingleNode<ShootableComponent> shootable, [JoinByTank] SelfActiveTankNode selfActiveTank)
        {
            Entity weapon = idleWeapon.Entity;
            if ((idleWeapon.weaponEnergy.Energy > 0f) && InputManager.GetActionKeyDown(ShotActions.SHOT))
            {
                SwitchIdleModeToWorkingMode(weapon);
            }
        }

        public static void SwitchIdleModeToWorkingMode(Entity weapon)
        {
            weapon.RemoveComponentIfPresent<StreamWeaponIdleComponent>();
            weapon.AddComponentIfAbsent<StreamWeaponWorkingComponent>();
        }

        [OnEventFire]
        public void SwitchToIdleWhenRemoveShootable(StreamWeaponResetStateEvent evt, StreamWeaponNode weaponNode)
        {
            SwitchWorkingModeToIdleMode(weaponNode.Entity);
        }

        [OnEventFire]
        public void SwitchToIdleWhenTankInactive(NodeRemoveEvent evt, SelfActiveTankNode selfActiveTank, [JoinByTank] StreamWeaponWorkingControllerNode workingWeapon)
        {
            SwitchWorkingModeToIdleMode(workingWeapon.Entity);
        }

        public static void SwitchWorkingModeToIdleMode(Entity weapon)
        {
            weapon.RemoveComponentIfPresent<StreamWeaponWorkingComponent>();
            weapon.AddComponentIfAbsent<StreamWeaponIdleComponent>();
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfActiveTankNode : StreamWeaponControllerSystem.SelfTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class SelfDeadTankNode : StreamWeaponControllerSystem.SelfTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public UserGroupComponent userGroup;
            public SelfTankComponent selfTank;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public SelfUserComponent selfUser;
        }

        public class StreamWeaponIdleControllerNode : StreamWeaponControllerSystem.StreamWeaponNode
        {
            public StreamWeaponIdleComponent streamWeaponIdle;
        }

        public class StreamWeaponNode : Node
        {
            public StreamWeaponControllerComponent streamWeaponController;
            public UserGroupComponent userGroup;
            public TankGroupComponent tankGroup;
            public StreamWeaponComponent streamWeapon;
            public WeaponEnergyComponent weaponEnergy;
            public CooldownTimerComponent cooldownTimer;
        }

        public class StreamWeaponShootableNode : StreamWeaponControllerSystem.StreamWeaponNode
        {
            public ShootableComponent shootable;
        }

        public class StreamWeaponWorkingControllerNode : StreamWeaponControllerSystem.StreamWeaponNode
        {
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }
    }
}

