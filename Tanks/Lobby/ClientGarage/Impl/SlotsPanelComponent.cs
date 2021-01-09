namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SlotsPanelComponent : UIBehaviour, Component
    {
        [SerializeField]
        private GameObject slotPrefab;
        [SerializeField]
        private string[] slotSpriteUids;
        [SerializeField]
        private GameObject[] slots;

        public string GetIconByType(ModuleBehaviourType moduleBehaviourType) => 
            this.slotSpriteUids[(int) moduleBehaviourType];

        public GameObject SetSlot(Slot slot) => 
            this.slots[(int) slot];
    }
}

