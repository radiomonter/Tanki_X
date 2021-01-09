namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ModuleCardItemUIComponent : BehaviourComponent, AttachToEntityListener
    {
        public ModuleBehaviourType Type;
        public TankPartModuleType TankPart;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private TextMeshProUGUI levelLabel;
        [SerializeField]
        private TextMeshProUGUI moduleName;
        [SerializeField]
        private GameObject selectBorder;
        [SerializeField]
        private Slider upgradeSlider;
        [SerializeField]
        private TextMeshProUGUI upgradeLabel;
        [SerializeField]
        private GameObject activeBorder;
        [SerializeField]
        private GameObject passiveBorder;
        [SerializeField]
        private LocalizedField activateAvailableLocalizedField;
        [SerializeField]
        private LocalizedField upgradeAvailableLocalizedField;
        [SerializeField]
        private TextMeshProUGUI upgradeAvailableText;
        [SerializeField]
        private float notMountableAlpha = 0.3f;
        private Entity moduleEntity;
        private bool upgradeAvailable;
        private int level = -1;
        private int maxCardsCount;
        private int cardsCount;
        public bool mountable;

        public void AttachedToEntity(Entity entity)
        {
            this.moduleEntity = entity;
        }

        private void OnDestroy()
        {
            if ((this.moduleEntity != null) && ClientUnityIntegrationUtils.HasEngine())
            {
                this.moduleEntity.RemoveComponent<ModuleCardItemUIComponent>();
            }
        }

        public void Select()
        {
            this.Selected = true;
        }

        public void SetCardsCount(int value, bool activate)
        {
        }

        private void SetUpgradeAvailableText(bool available, string text)
        {
            this.upgradeAvailableText.gameObject.SetActive(available);
            this.upgradeAvailableText.text = text;
            if (available)
            {
                this.Mountable ??= true;
            }
        }

        public Entity ModuleEntity =>
            this.moduleEntity;

        public long MarketGroupId =>
            ((this.moduleEntity == null) || !this.moduleEntity.HasComponent<MarketItemGroupComponent>()) ? 0L : this.moduleEntity.GetComponent<MarketItemGroupComponent>().Key;

        public string ModuleName
        {
            set => 
                this.moduleName.text = value;
        }

        public bool Active
        {
            set
            {
                this.activeBorder.SetActive(value);
                this.passiveBorder.SetActive(!value);
            }
        }

        public bool UpgradeAvailable
        {
            get => 
                this.upgradeAvailable;
            set
            {
                this.upgradeAvailable = value;
                this.SetUpgradeAvailableText(value, this.upgradeAvailableLocalizedField.Value);
            }
        }

        public bool ActivateAvailable
        {
            get => 
                this.upgradeAvailable;
            set
            {
                this.upgradeAvailable = value;
                this.SetUpgradeAvailableText(value, this.activateAvailableLocalizedField.Value);
            }
        }

        public int Level
        {
            get => 
                this.level;
            set
            {
                this.level = value;
                this.levelLabel.text = (this.level != -1) ? this.level.ToString() : "0";
                this.Mountable = (this.level > 0) || this.UpgradeAvailable;
            }
        }

        public string Icon
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public bool Selected
        {
            set => 
                this.selectBorder.SetActive(value);
        }

        public int MaxCardsCount
        {
            get => 
                this.maxCardsCount;
            set
            {
                this.maxCardsCount = value;
                this.upgradeSlider.maxValue = this.maxCardsCount;
                this.CardsCount = this.cardsCount;
                this.upgradeSlider.transform.parent.gameObject.SetActive(this.maxCardsCount > 0);
            }
        }

        public int CardsCount
        {
            get => 
                this.cardsCount;
            set
            {
                this.cardsCount = value;
                this.upgradeLabel.text = (this.MaxCardsCount <= 0) ? "MAX" : (this.cardsCount + "/" + this.MaxCardsCount);
                this.upgradeSlider.value = this.cardsCount;
                if (this.level > 0)
                {
                    this.UpgradeAvailable = (this.cardsCount >= this.maxCardsCount) && (this.maxCardsCount > 0);
                }
                else
                {
                    this.ActivateAvailable = (this.cardsCount >= this.maxCardsCount) && (this.maxCardsCount > 0);
                }
            }
        }

        public bool Mountable
        {
            get => 
                this.mountable;
            set
            {
                this.mountable = value;
                base.GetComponent<CanvasGroup>().alpha = !value ? this.notMountableAlpha : 1f;
                bool flag = !value;
                this.upgradeAvailableText.GetComponent<CanvasGroup>().ignoreParentGroups = flag;
                this.selectBorder.GetComponent<CanvasGroup>().ignoreParentGroups = flag;
            }
        }
    }
}

