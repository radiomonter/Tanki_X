namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class MineActivateValidationSystem : ECSSystem
    {
        [OnEventFire]
        public void CTFActivation(UpdateEvent e, SelfTankNode tank, [JoinByTank, Combine] MineSlotNode slot, [JoinByModule] MineModuleNode module, [JoinAll] ICollection<SingleNode<FlagPedestalComponent>> flagPedestals, [JoinAll] CTFBattleNode battle)
        {
            Vector3 position = tank.hullInstance.HullInstance.transform.position;
            if (this.HasActivationMine(position, flagPedestals, battle))
            {
                this.EnableActivation(slot.Entity);
            }
            else
            {
                this.DisableActivation(slot.Entity);
            }
        }

        private void DisableActivation(Entity inventory)
        {
            inventory.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        [OnEventFire]
        public void DMActivation(NodeAddedEvent e, SelfTankNode tank, [Context, JoinByTank, Combine] MineSlotNode slot, [Context, JoinByModule] MineModuleNode module)
        {
            this.EnableActivation(slot.Entity);
        }

        private void EnableActivation(Entity inventory)
        {
            inventory.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        private bool HasActivationMine(Vector3 tankPosition, ICollection<SingleNode<FlagPedestalComponent>> flagPedestals, CTFBattleNode battle)
        {
            RaycastHit hit;
            bool flag;
            if (!Physics.Raycast(tankPosition + Vector3.up, Vector3.down, out hit, MineUtil.TANK_MINE_RAYCAST_DISTANCE, LayerMasks.STATIC))
            {
                return false;
            }
            using (IEnumerator<SingleNode<FlagPedestalComponent>> enumerator = flagPedestals.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        SingleNode<FlagPedestalComponent> current = enumerator.Current;
                        Vector3 position = current.component.Position;
                        Vector3 vector2 = position - hit.point;
                        if (vector2.magnitude >= battle.ctfConfig.minDistanceFromMineToBase)
                        {
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag;
        }

        public class CTFBattleNode : Node
        {
            public SelfComponent self;
            public CTFComponent ctf;
            public CTFConfigComponent ctfConfig;
        }

        public class MineModuleNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public StaticMineModuleComponent staticMineModule;
        }

        public class MineSlotNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : Node
        {
            public BattleGroupComponent battleGroup;
            public TankGroupComponent tankGroup;
            public SelfTankComponent selfTank;
            public HullInstanceComponent hullInstance;
        }
    }
}

