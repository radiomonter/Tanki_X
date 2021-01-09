namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class GarageSlotsUIPanelComponent : BehaviourComponent
    {
        [SerializeField]
        private SlotUIComponent[] slots;

        public SlotUIComponent GetSlot(Slot slot)
        {
            int index = (int) slot;
            return ((index < this.slots.Length) ? this.slots[index] : null);
        }
    }
}

