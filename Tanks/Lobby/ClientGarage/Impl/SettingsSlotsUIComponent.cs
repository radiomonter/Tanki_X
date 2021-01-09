namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class SettingsSlotsUIComponent : BehaviourComponent
    {
        [SerializeField]
        private List<SettingsSlotUIItem> slots;

        public SettingsSlotUIComponent GetSlot(Slot slot)
        {
            <GetSlot>c__AnonStorey0 storey = new <GetSlot>c__AnonStorey0 {
                slot = slot
            };
            return (!this.slots.Any<SettingsSlotUIItem>(new Func<SettingsSlotUIItem, bool>(storey.<>m__0)) ? null : this.slots.First<SettingsSlotUIItem>(new Func<SettingsSlotUIItem, bool>(storey.<>m__1)).settingsSlotUI);
        }

        [CompilerGenerated]
        private sealed class <GetSlot>c__AnonStorey0
        {
            internal Slot slot;

            internal bool <>m__0(SettingsSlotsUIComponent.SettingsSlotUIItem s) => 
                s.slot.Equals(this.slot);

            internal bool <>m__1(SettingsSlotsUIComponent.SettingsSlotUIItem s) => 
                s.slot.Equals(this.slot);
        }

        [Serializable]
        public class SettingsSlotUIItem
        {
            public Slot slot;
            public SettingsSlotUIComponent settingsSlotUI;
        }
    }
}

