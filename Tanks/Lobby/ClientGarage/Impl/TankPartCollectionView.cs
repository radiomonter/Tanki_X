namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.System;
    using TMPro;
    using UnityEngine;

    public class TankPartCollectionView : MonoBehaviour
    {
        public TankSlotView activeSlot;
        public TankSlotView activeSlot2;
        public TankSlotView passiveSlot;
        public GameObject tankPartView;
        public ImageSkin preview;
        public TextMeshProUGUI partName;
        public LineCollectionView lineCollectionView;
        public Entity marketEntity;
        public CanvasGroup slotContainer;
        [SerializeField]
        private UpgradeStars upgradeStars;
        [SerializeField]
        private TextMeshProUGUI bonusFromModules;
        [SerializeField]
        private TextMeshProUGUI basePartParam;
        [SerializeField]
        private TextMeshProUGUI partLevel;
        [SerializeField]
        private LocalizedField basePartParamName;

        private void CancelHighlight(TankSlotView slot)
        {
            slot.TurnOffRenderAboveAll();
            slot.CancelHighlightForDrop();
        }

        public void CancelHighlightForDrop()
        {
            this.activeSlot.GetComponent<DragAndDropCell>().enabled = true;
            this.activeSlot2.GetComponent<DragAndDropCell>().enabled = true;
            this.passiveSlot.GetComponent<DragAndDropCell>().enabled = true;
            this.CancelHighlight(this.activeSlot);
            this.CancelHighlight(this.activeSlot2);
            this.CancelHighlight(this.passiveSlot);
        }

        public TankSlotView GetSlotBySlotEntity(Entity slotEntity) => 
            !this.activeSlot.SlotNode.Entity.Equals(slotEntity) ? (!this.activeSlot2.SlotNode.Entity.Equals(slotEntity) ? (!this.passiveSlot.SlotNode.Entity.Equals(slotEntity) ? null : this.passiveSlot) : this.activeSlot2) : this.activeSlot;

        public TankSlotView GetSlotForDrop(ModuleBehaviourType type) => 
            (type != ModuleBehaviourType.PASSIVE) ? ((!this.activeSlot.Locked || !this.activeSlot2.Locked) ? (!this.activeSlot.Locked ? (!this.activeSlot2.Locked ? ((!this.activeSlot.HasItem() || !this.activeSlot2.HasItem()) ? (!this.activeSlot.HasItem() ? this.activeSlot : this.activeSlot2) : this.activeSlot) : this.activeSlot) : this.activeSlot2) : null) : (!this.passiveSlot.Locked ? this.passiveSlot : null);

        public void SetStars(float coef)
        {
            this.upgradeStars.SetPower(coef);
        }

        public void TurnOffSlots()
        {
            this.activeSlot.GetComponent<DragAndDropCell>().enabled = false;
            this.activeSlot2.GetComponent<DragAndDropCell>().enabled = false;
            this.passiveSlot.GetComponent<DragAndDropCell>().enabled = false;
        }

        private void TurnOn(TankSlotView slot)
        {
            slot.GetComponent<DragAndDropCell>().enabled = true;
            slot.TurnOnRenderAboveAll();
        }

        private void TurnOnAndHighlight(TankSlotView slot)
        {
            this.TurnOn(slot);
            slot.HighlightForDrop();
        }

        public void TurnOnSlotsByTypeAndHighlightForDrop(ModuleBehaviourType type)
        {
            if (type == ModuleBehaviourType.PASSIVE)
            {
                if (!this.passiveSlot.Locked)
                {
                    this.TurnOnAndHighlight(this.passiveSlot);
                }
            }
            else
            {
                if (!this.activeSlot.Locked)
                {
                    this.TurnOn(this.activeSlot);
                    if (this.activeSlot2.Locked)
                    {
                        this.activeSlot.HighlightForDrop();
                    }
                    else if (!this.activeSlot.HasItem() || (this.activeSlot.HasItem() && this.activeSlot2.HasItem()))
                    {
                        this.activeSlot.HighlightForDrop();
                    }
                }
                if (!this.activeSlot2.Locked)
                {
                    this.TurnOn(this.activeSlot2);
                    if (this.activeSlot.Locked)
                    {
                        this.activeSlot2.HighlightForDrop();
                    }
                    else if (!this.activeSlot2.HasItem() || (this.activeSlot.HasItem() && this.activeSlot2.HasItem()))
                    {
                        this.activeSlot2.HighlightForDrop();
                    }
                }
            }
        }

        public void UpdateView(TankPartItem tankPart)
        {
            this.activeSlot.UpdateView();
            this.activeSlot2.UpdateView();
            this.passiveSlot.UpdateView();
            CalculateTankPartUpgradeCoeffEvent eventInstance = new CalculateTankPartUpgradeCoeffEvent();
            tankPart.MarketItem.ScheduleEvent(eventInstance);
            this.SetStars(eventInstance.UpgradeCoeff);
            VisualProperty property = tankPart.Properties[0];
            this.BasePartParam = $"{property.InitialValue}";
            this.BonusFromModules = (int) (property.GetValue(eventInstance.UpgradeCoeff) - property.InitialValue);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public float BonusFromModules
        {
            set
            {
                this.bonusFromModules.gameObject.SetActive(value > 0f);
                this.bonusFromModules.text = $"{"+ " + value}";
            }
        }

        public string BasePartParam
        {
            set => 
                this.basePartParam.text = $"{this.basePartParamName.Value} {value}";
        }

        public string PartLevel
        {
            set => 
                this.partLevel.text = value;
        }
    }
}

