namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class ForceFieldSlotActivationValidatorSystem : ECSSystem
    {
        private void DisableActivation(Entity inventory)
        {
            inventory.AddComponentIfAbsent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        private void EnableActivation(Entity inventory)
        {
            inventory.RemoveComponentIfPresent<InventorySlotTemporaryBlockedByClientComponent>();
        }

        [OnEventFire]
        public void MarkModuleAsForceField(NodeAddedEvent e, ModuleUserItemNode module, [JoinByMarketItem, Context] ForceFieldModuleUpgradeInfoNode info)
        {
            module.Entity.AddComponent<ForceFieldModuleComponent>();
        }

        [OnEventFire]
        public void UpdateActivatePossibility(UpdateEvent e, WeaponNode weaponNode, [JoinByTank, Combine] SlotNode slot, [JoinByModule] ForceFieldModuleNode module)
        {
            if (ForceFieldTransformUtil.CanFallToTheGround(weaponNode.weaponInstance.WeaponInstance.transform))
            {
                this.EnableActivation(slot.Entity);
            }
            else
            {
                this.DisableActivation(slot.Entity);
            }
        }

        public class ForceFieldModuleNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public ForceFieldModuleComponent forceFieldModule;
            public ModuleEffectsComponent moduleEffects;
        }

        public class ForceFieldModuleUpgradeInfoNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public ForceFieldModuleComponent forceFieldModule;
        }

        [Not(typeof(ForceFieldModuleComponent))]
        public class ModuleUserItemNode : Node
        {
            public ModuleItemComponent moduleItem;
            public UserItemComponent userItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class SlotNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public SelfComponent self;
        }
    }
}

