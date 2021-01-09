namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class InventoryHUDComponent : BehaviourComponent, AttachToEntityListener
    {
        [SerializeField]
        private List<SlotUIItem> slots;
        [SerializeField]
        private EntityBehaviour modulePrefab;
        [SerializeField]
        private GameObject goldBonusCounterPrefab;
        private List<GameObject> modules = new List<GameObject>();

        public void AttachedToEntity(Entity entity)
        {
            base.gameObject.SetActive(false);
            foreach (GameObject obj2 in this.modules)
            {
                DestroyImmediate(obj2);
            }
            this.modules.Clear();
            foreach (SlotUIItem item in this.slots)
            {
                item.slotRectTransform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void CreateGoldBonusesCounter(EntityBehaviour module)
        {
            Instantiate<GameObject>(this.goldBonusCounterPrefab, module.transform, false);
        }

        public EntityBehaviour CreateModule(Slot slot)
        {
            base.gameObject.SetActive(true);
            RectTransform slotRectTransform = this.GetSlotRectTransform(slot);
            EntityBehaviour behaviour = this.Instantiate<EntityBehaviour>(this.modulePrefab, slotRectTransform);
            base.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
            return behaviour;
        }

        public string GetKeyBindForItem(ItemButtonComponent item)
        {
            string[] strArray = new string[] { InventoryAction.INVENTORY_SLOT1, InventoryAction.INVENTORY_SLOT2, InventoryAction.INVENTORY_SLOT3, InventoryAction.INVENTORY_SLOT4, InventoryAction.INVENTORY_GOLDBOX };
            Transform parent = item.transform.parent.parent;
            for (int i = 0; i < strArray.Length; i++)
            {
                Transform child = parent.GetChild(i);
                if (item.transform.parent == child)
                {
                    InputAction action = InputManager.GetAction(new InputActionId("Tanks.Battle.ClientCore.Impl.InventoryAction", strArray[i]), new InputActionContextId("Tanks.Battle.ClientCore.Impl.BasicContexts"));
                    return (((action == null) || (action.keys.Length == 0)) ? string.Empty : KeyboardSettingsUtil.KeyCodeToString(action.keys[0]));
                }
            }
            return string.Empty;
        }

        private RectTransform GetSlotRectTransform(Slot slot)
        {
            <GetSlotRectTransform>c__AnonStorey0 storey = new <GetSlotRectTransform>c__AnonStorey0 {
                slot = slot
            };
            return this.slots.First<SlotUIItem>(new Func<SlotUIItem, bool>(storey.<>m__0)).slotRectTransform;
        }

        private T Instantiate<T>(T prefab, RectTransform parent) where T: Component
        {
            parent.GetChild(0).gameObject.SetActive(false);
            T local = Instantiate<T>(prefab, parent, false);
            this.modules.Add(local.gameObject);
            RectTransform transform = (RectTransform) local.transform;
            Vector2 vector = new Vector2();
            transform.anchorMin = vector;
            transform.anchorMax = new Vector2(1f, 1f);
            Vector2 vector2 = new Vector2();
            transform.anchoredPosition = vector2;
            Vector2 vector3 = new Vector2();
            transform.sizeDelta = vector3;
            return local;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [CompilerGenerated]
        private sealed class <GetSlotRectTransform>c__AnonStorey0
        {
            internal Slot slot;

            internal bool <>m__0(InventoryHUDComponent.SlotUIItem s) => 
                s.slot.Equals(this.slot);
        }

        [Serializable]
        public class SlotUIItem
        {
            public Slot slot;
            public RectTransform slotRectTransform;
        }
    }
}

