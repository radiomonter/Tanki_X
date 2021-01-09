namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientGarage.API;

    public class EnergyInjectionEffectSystem : ECSSystem
    {
        private bool CheckBlock(CooldownTimerComponent cooldownTimerComponent, Entity slot)
        {
            if (cooldownTimerComponent.CooldownTimerSec <= 0f)
            {
                return true;
            }
            slot.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
            return false;
        }

        private bool CheckBlock(WeaponEnergyComponent weaponEnergyComponent, EnergyInjectionModuleReloadEnergyComponent energyInjectionModuleReloadEnergyComponent, Entity slot)
        {
            if (weaponEnergyComponent.Energy >= energyInjectionModuleReloadEnergyComponent.ReloadEnergyPercent)
            {
                return true;
            }
            slot.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
            return false;
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, DiscreteWeaponEnergyNode weapon, [JoinByTank] ObservationSlotNode slot, [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module)
        {
            if (this.CheckBlock(weapon.weaponEnergy, module.component, slot.Entity) && this.CheckBlock(weapon.cooldownTimer, slot.Entity))
            {
                slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
            }
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, MagazineNode weapon, [JoinByTank] ObservationSlotNode slot, [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module)
        {
            if (this.CheckBlock(weapon.cooldownTimer, slot.Entity))
            {
                if (weapon.magazineStorage.CurrentCartridgeCount < weapon.magazineWeapon.MaxCartridgeCount)
                {
                    slot.Entity.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
                }
                else
                {
                    slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
                }
            }
        }

        [OnEventFire]
        public void CheckEnergyInjectionSlot(EarlyUpdateEvent e, StreamWeaponEnergyNode weapon, [JoinByTank] ObservationSlotNode slot, [JoinByModule] SingleNode<EnergyInjectionModuleReloadEnergyComponent> module)
        {
            if (this.CheckBlock(weapon.weaponEnergy, module.component, slot.Entity))
            {
                slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
            }
        }

        [OnEventFire]
        public void CleanStopObservation(NodeRemoveEvent e, SingleNode<TankGroupComponent> node, [JoinSelf] SingleNode<EnergyInjectionSlotStopObservationComponent> slot)
        {
            slot.Entity.RemoveComponent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventComplete]
        public void CleanStopObservation(UpdateEvent e, SingleNode<EnergyInjectionSlotStopObservationComponent> slot, [JoinByTank] ActiveTankNode tank)
        {
            slot.Entity.RemoveComponent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventComplete]
        public void ExecuteEnergyInjection(ExecuteEnergyInjectionEvent e, SingleNode<CooldownTimerComponent> weapon)
        {
            if (!weapon.Entity.HasComponent<MagazineWeaponComponent>())
            {
                weapon.component.CooldownTimerSec = 0f;
            }
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SingleNode<EnergyInjectionSlotStopObservationComponent> slot)
        {
            slot.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, ActiveTankNode tank, [JoinByTank, Context] SlotNode slot, [JoinByModule, Context] ModuleNode module)
        {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, DeadTankNode tank, [JoinByTank, Context] SlotNode slot, [JoinByModule, Context] ModuleNode module)
        {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, NewTankNode tank, [JoinByTank, Context] SlotNode slot, [JoinByModule, Context] ModuleNode module)
        {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SemiActiveTankNode tank, [JoinByTank, Context] SlotNode slot, [JoinByModule, Context] ModuleNode module)
        {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventFire]
        public void StopObservation(NodeAddedEvent e, SpawnTankNode tank, [JoinByTank, Context] SlotNode slot, [JoinByModule, Context] ModuleNode module)
        {
            slot.Entity.AddComponentIfAbsent<EnergyInjectionSlotStopObservationComponent>();
        }

        [OnEventComplete]
        public void SwitchStreamWeaponToWorking(ExecuteEnergyInjectionEvent e, SingleNode<StreamWeaponComponent> weapon, [JoinSelf] SingleNode<ShootableComponent> node, [JoinByTank] SingleNode<TankActiveStateComponent> tank)
        {
            if (InputManager.CheckAction(ShotActions.SHOT))
            {
                StreamWeaponControllerSystem.SwitchIdleModeToWorkingMode(weapon.Entity);
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class ActiveTankNode : EnergyInjectionEffectSystem.TankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class DeadTankNode : EnergyInjectionEffectSystem.TankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class DiscreteWeaponEnergyNode : EnergyInjectionEffectSystem.WeaponEnergyNode
        {
            public DiscreteWeaponComponent discreteWeapon;
        }

        public class EnergyInjectionSlotStopObservationComponent : Component
        {
        }

        public class MagazineNode : EnergyInjectionEffectSystem.WeaponNode
        {
            public CooldownTimerComponent cooldownTimer;
            public MagazineStorageComponent magazineStorage;
            public MagazineWeaponComponent magazineWeapon;
        }

        public class ModuleNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public EnergyInjectionModuleReloadEnergyComponent energyInjectionModuleReloadEnergy;
        }

        public class NewTankNode : EnergyInjectionEffectSystem.TankNode
        {
            public TankNewStateComponent tankNewState;
        }

        [Not(typeof(EnergyInjectionEffectSystem.EnergyInjectionSlotStopObservationComponent))]
        public class ObservationSlotNode : EnergyInjectionEffectSystem.SlotNode
        {
        }

        public class SemiActiveTankNode : EnergyInjectionEffectSystem.TankNode
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class SlotNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
            public TankGroupComponent tankGroup;
        }

        public class SpawnTankNode : EnergyInjectionEffectSystem.TankNode
        {
            public TankSpawnStateComponent tankSpawnState;
        }

        public class StreamWeaponEnergyNode : EnergyInjectionEffectSystem.WeaponEnergyNode
        {
            public StreamWeaponComponent streamWeapon;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }

        public class WeaponEnergyNode : EnergyInjectionEffectSystem.WeaponNode
        {
            public WeaponEnergyComponent weaponEnergy;
            public CooldownTimerComponent cooldownTimer;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
        }
    }
}

