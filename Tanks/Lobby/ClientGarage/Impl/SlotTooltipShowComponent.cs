namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class SlotTooltipShowComponent : TooltipShowBehaviour
    {
        [SerializeField]
        private GameObject slotLockedTooltip;
        [SerializeField]
        private GameObject moduleTooltip;
        [SerializeField]
        private LocalizedField slotLockedTitle;
        [SerializeField]
        private LocalizedField weaponSlotLocked;
        [SerializeField]
        private LocalizedField hullSlotLocked;
        [SerializeField]
        private LocalizedField emptySlot;
        [SerializeField]
        private PaletteColorField lockedHeaderColor;

        public void ShowEmptySlotTooltip()
        {
            TooltipController.Instance.ShowTooltip(Input.mousePosition, this.emptySlot.Value, null, true);
        }

        private void ShowLockedModuleTooltip()
        {
            string[] textArray1 = new string[] { "<color=#", this.lockedHeaderColor.Color.ToHexString(), ">", this.slotLockedTitle.Value, "</color>" };
            string str = string.Concat(textArray1);
            string str3 = ((this.slot.TankPart != TankPartModuleType.TANK) ? this.weaponSlotLocked.Value : this.hullSlotLocked.Value).Replace("{0}", this.slot.Rank.ToString());
            string[] data = new string[] { str, str3 };
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, this.slotLockedTooltip, true);
        }

        public void ShowModuleTooltip(object data)
        {
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, this.moduleTooltip, true);
        }

        public override void ShowTooltip(Vector3 mousePosition)
        {
            Engine engine = TooltipShowBehaviour.EngineService.Engine;
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            engine.ScheduleEvent(eventInstance, TooltipShowBehaviour.EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                base.tooltipShowed = true;
                if (this.slot.Locked)
                {
                    this.ShowLockedModuleTooltip();
                }
                else if (this.slot.SlotEntity != null)
                {
                    engine.ScheduleEvent<ModuleTooltipShowEvent>(this.slot.SlotEntity);
                }
            }
        }

        private SlotUIComponent slot =>
            base.GetComponent<SlotUIComponent>();
    }
}

