namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class TankSlotView : SlotView
    {
        public Image lockIcon;
        public LocalizedField emptySlotTooltipText;
        [SerializeField]
        private LocalizedField lockedSlotTooltipText;
        [SerializeField]
        private LocalizedField turretLockedSlotText;
        [SerializeField]
        private LocalizedField hullLockedSlotText;
        [SerializeField]
        private GameObject border;

        public override void SetItem(SlotItemView item)
        {
            if (!item.ModuleItem.Slot.Equals(this.SlotNode.Entity))
            {
                throw new ArgumentException($"Screen slot entity {this.SlotNode.Entity.Id} doesn't match module item slot entity {item.ModuleItem.Slot.Id}");
            }
            base.SetItem(item);
        }

        private void UpdateTooltip()
        {
            if (this.Locked)
            {
                string str = ((this.SlotNode.slotTankPart.TankPart != TankPartModuleType.TANK) ? this.turretLockedSlotText.Value : this.hullLockedSlotText.Value).Replace("{0}", this.SlotNode.slotUserItemInfo.UpgradeLevel.ToString());
                base.tooltip.TipText = ((string) this.lockedSlotTooltipText) + "\n" + str;
            }
            else if (!base.HasItem())
            {
                base.tooltip.TipText = this.emptySlotTooltipText.Value;
            }
            else
            {
                base.tooltip.TipText = string.Empty;
            }
        }

        public void UpdateView()
        {
            this.Locked = this.SlotNode.Entity.HasComponent<SlotLockedComponent>();
            this.lockIcon.gameObject.SetActive(this.Locked);
            this.UpdateTooltip();
            this.border.SetActive(!base.HasItem());
        }

        public bool Locked { get; private set; }

        public Tanks.Lobby.ClientGarage.Impl.NewModulesScreenSystem.SlotNode SlotNode { get; set; }
    }
}

