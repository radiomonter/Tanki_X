namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class LockSlotByEMPSystem : ECSSystem
    {
        [OnEventFire]
        public void LockSlot(NodeAddedEvent e, SlotLockedNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud)
        {
            hud.component.LockByEMP(true);
        }

        [OnEventFire]
        public void UnlockSlot(NodeRemoveEvent e, SlotLockedNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud)
        {
            hud.component.LockByEMP(false);
        }

        public class ItemButtonNode : Node
        {
            public ItemButtonComponent itemButton;
            public ModuleGroupComponent moduleGroup;
        }

        public class SlotLockedNode : Node
        {
            public SlotLockedByEMPComponent slotLockedByEMP;
        }
    }
}

