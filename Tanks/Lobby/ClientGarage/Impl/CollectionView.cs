namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.UI;

    public class CollectionView : MonoBehaviour
    {
        public GameObject tierActiveCollectionViewPrefab;
        public GameObject tierPassiveCollectionViewPrefab;
        public GameObject collectionSlotPrefab;
        public SubCollectionView turretCollectionView;
        public SubCollectionView hullCollectionView;
        public Toggle turretToggle;
        public Toggle hullToggle;
        public static Dictionary<ModuleItem, CollectionSlotView> slots;
        [CompilerGenerated]
        private static Func<ModuleItem, bool> <>f__am$cache0;

        public void AddSlot(ModuleItem moduleItem)
        {
            GameObject activeTierCollectionList;
            GameObject tierActiveCollectionViewPrefab;
            SubCollectionView view = (moduleItem.TankPartModuleType != TankPartModuleType.WEAPON) ? this.hullCollectionView : this.turretCollectionView;
            if (moduleItem.ModuleBehaviourType == ModuleBehaviourType.ACTIVE)
            {
                activeTierCollectionList = view.activeTierCollectionList;
                tierActiveCollectionViewPrefab = this.tierActiveCollectionViewPrefab;
            }
            else
            {
                activeTierCollectionList = view.passiveTierCollectionList;
                tierActiveCollectionViewPrefab = this.tierPassiveCollectionViewPrefab;
            }
            for (int i = activeTierCollectionList.transform.childCount; i <= moduleItem.TierNumber; i++)
            {
                this.AddTierCollectionViewToList(activeTierCollectionList, tierActiveCollectionViewPrefab);
            }
            TierCollectionView component = activeTierCollectionList.transform.GetChild(moduleItem.TierNumber).GetComponent<TierCollectionView>();
            GameObject obj4 = Instantiate<GameObject>(this.collectionSlotPrefab);
            obj4.transform.SetParent(component.slotList.transform, false);
            CollectionSlotView componentInChildren = obj4.GetComponentInChildren<CollectionSlotView>();
            componentInChildren.Init(moduleItem);
            slots.Add(moduleItem, componentInChildren);
        }

        public void AddSlotItem(ModuleItem moduleItem, SlotItemView slotItemView)
        {
            slots[moduleItem].SetItem(slotItemView);
        }

        private void AddTierCollectionViewToList(GameObject targetTierCollectionList, GameObject tierCollectionPrefab)
        {
            Instantiate<GameObject>(tierCollectionPrefab).transform.SetParent(targetTierCollectionList.transform, false);
        }

        private void CreateSlots()
        {
            slots = new Dictionary<ModuleItem, CollectionSlotView>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = mi => mi.IsMutable();
            }
            List<ModuleItem> list = GarageItemsRegistry.Modules.Where<ModuleItem>(<>f__am$cache0).ToList<ModuleItem>();
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                this.AddSlot(list[i]);
            }
        }

        public void SwitchMode(TankPartModuleType mode)
        {
            if (mode == TankPartModuleType.WEAPON)
            {
                this.turretCollectionView.gameObject.SetActive(true);
                this.turretToggle.isOn = true;
                this.turretToggle.interactable = false;
                this.hullCollectionView.gameObject.SetActive(false);
                this.hullToggle.isOn = false;
                this.hullToggle.interactable = true;
            }
            else
            {
                this.hullCollectionView.gameObject.SetActive(true);
                this.hullToggle.isOn = true;
                this.hullToggle.interactable = false;
                this.turretCollectionView.gameObject.SetActive(false);
                this.turretToggle.isOn = false;
                this.turretToggle.interactable = true;
            }
        }

        public void UpdateView()
        {
            if (slots == null)
            {
                this.CreateSlots();
            }
            foreach (CollectionSlotView view in slots.Values)
            {
                view.UpdateView();
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

