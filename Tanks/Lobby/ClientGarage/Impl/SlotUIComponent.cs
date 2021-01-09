namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SlotUIComponent : UIBehaviour, Component, AttachToEntityListener, IPointerClickHandler, IEventSystemHandler
    {
        [SerializeField]
        private ImageSkin moduleIconImageSkin;
        [SerializeField]
        private PaletteColorField exceptionalColor;
        [SerializeField]
        private PaletteColorField epicColor;
        [SerializeField]
        private Image moduleIcon;
        [SerializeField]
        private Image selectionImage;
        [SerializeField]
        private Image lockIcon;
        [SerializeField]
        private Image upgradeIcon;
        [SerializeField]
        private TextMeshProUGUI slotName;
        [SerializeField]
        private LocalizedField slotNameLocalization;
        private TankPartModuleType tankPart;
        private bool locked;
        private int rank;
        private Entity slotEntity;

        public void AttachedToEntity(Entity entity)
        {
            this.slotEntity = entity;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if ((this.slotEntity != null) && (ClientUnityIntegrationUtils.HasEngine() && this.slotEntity.HasComponent<ModuleCardItemUIComponent>()))
            {
                this.slotEntity.RemoveComponent<ModuleCardItemUIComponent>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.locked)
            {
                base.GetComponent<SlotTooltipShowComponent>().ShowTooltip(Input.mousePosition);
            }
        }

        public Tanks.Lobby.ClientGarage.API.Slot Slot
        {
            set => 
                this.slotName.text = this.slotNameLocalization.Value + " " + ((int) ((byte) (((int) value) + 1)));
        }

        public TankPartModuleType TankPart
        {
            get => 
                this.tankPart;
            set => 
                this.tankPart = value;
        }

        public ImageSkin ModuleIconImageSkin =>
            this.moduleIconImageSkin;

        public Image ModuleIcon
        {
            get => 
                this.moduleIcon;
            set => 
                this.moduleIcon = value;
        }

        public Image SelectionImage
        {
            get => 
                this.selectionImage;
            set => 
                this.selectionImage = value;
        }

        public Image UpgradeIcon
        {
            get => 
                this.upgradeIcon;
            set => 
                this.upgradeIcon = value;
        }

        public Color ExceptionalColor =>
            (Color) this.exceptionalColor;

        public Color EpicColor =>
            (Color) this.epicColor;

        public bool Locked
        {
            get => 
                this.locked;
            set
            {
                this.lockIcon.gameObject.SetActive(value);
                this.locked = value;
                base.GetComponent<CanvasGroup>().alpha = !this.locked ? 1f : 0.6f;
                Toggle component = base.GetComponent<Toggle>();
                if (component != null)
                {
                    component.interactable = !this.locked;
                }
            }
        }

        public int Rank
        {
            get => 
                this.rank;
            set => 
                this.rank = value;
        }

        public Entity SlotEntity =>
            this.slotEntity;
    }
}

