namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class MapViewBonusElement : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
    {
        [SerializeField]
        private GameObject inaccesible;
        [SerializeField]
        private UnityEngine.UI.Toggle accesible;
        [SerializeField]
        private UnityEngine.UI.Toggle epicAccesible;
        [SerializeField]
        private GameObject taken;
        [SerializeField]
        private GameObject epicTaken;
        [SerializeField]
        private LocalizedField crystalText;
        [SerializeField]
        private LocalizedField xCrystalText;
        [SerializeField]
        private LocalizedField chargesText;
        [SerializeField]
        private LocalizedField hiddenText;
        public DailyBonusData dailyBonusData;
        private UnityEngine.UI.Toggle toggle;
        private BonusElementState elementState;

        public static string FirstCharToUpper(string input) => 
            input.First<char>().ToString().ToUpper() + input.Substring(1).ToLower();

        private string GetTooltipText(DailyBonusData dailyBonusData, BonusElementState elementState)
        {
            if (elementState == BonusElementState.INACCESSIBLE)
            {
                return (string) this.hiddenText;
            }
            DailyBonusType dailyBonusType = dailyBonusData.DailyBonusType;
            switch (dailyBonusType)
            {
                case DailyBonusType.CRY:
                    return (FirstCharToUpper(this.crystalText.Value) + " x" + dailyBonusData.CryAmount);

                case DailyBonusType.XCRY:
                    return (FirstCharToUpper(this.xCrystalText.Value) + " x" + dailyBonusData.XcryAmount);

                case DailyBonusType.ENERGY:
                    return (FirstCharToUpper(this.chargesText.Value) + " x" + dailyBonusData.EnergyAmount);

                case DailyBonusType.CONTAINER:
                    return FirstCharToUpper(GarageItemsRegistry.GetItem<GarageItem>(dailyBonusData.ContainerReward.MarketItemId).Name);
            }
            return ((dailyBonusType == DailyBonusType.DETAIL) ? GarageItemsRegistry.GetItem<DetailItem>(dailyBonusData.DetailReward.MarketItemId).Name : string.Empty);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.elementState == BonusElementState.ACCESSIBLE)
            {
                UISoundEffectController.UITransformRoot.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayHover();
            }
        }

        public void OnValueChanged(Action<MapViewBonusElement, bool> onBonusElementClick)
        {
            <OnValueChanged>c__AnonStorey0 storey = new <OnValueChanged>c__AnonStorey0 {
                onBonusElementClick = onBonusElementClick,
                $this = this
            };
            this.accesible.onValueChanged.AddListener(new UnityAction<bool>(storey.<>m__0));
            this.epicAccesible.onValueChanged.AddListener(new UnityAction<bool>(storey.<>m__1));
        }

        public void SetToggleGroup(ToggleGroup toggleGroup)
        {
            toggleGroup.RegisterToggle(this.accesible);
            toggleGroup.RegisterToggle(this.epicAccesible);
        }

        public void UpdateView(DailyBonusData dailyBonusData, BonusElementState elementState)
        {
            this.dailyBonusData = dailyBonusData;
            this.elementState = elementState;
            this.inaccesible.SetActive(false);
            this.accesible.gameObject.SetActive(false);
            this.epicAccesible.gameObject.SetActive(false);
            this.taken.SetActive(false);
            this.epicTaken.SetActive(false);
            if (elementState == BonusElementState.INACCESSIBLE)
            {
                this.inaccesible.SetActive(true);
            }
            else if (elementState == BonusElementState.ACCESSIBLE)
            {
                this.toggle = !dailyBonusData.IsEpic() ? this.accesible : this.epicAccesible;
                this.toggle.gameObject.SetActive(true);
                this.toggle.isOn = false;
            }
            else if (elementState == BonusElementState.TAKEN)
            {
                if (dailyBonusData.IsEpic())
                {
                    this.epicTaken.SetActive(true);
                }
                else
                {
                    this.taken.SetActive(true);
                }
            }
            base.GetComponent<TooltipShowBehaviour>().TipText = this.GetTooltipText(dailyBonusData, elementState);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public int ZoneIndex { get; set; }

        public UnityEngine.UI.Toggle Toggle =>
            this.toggle;

        public bool Interactable
        {
            set
            {
                if (this.toggle != null)
                {
                    this.toggle.interactable = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnValueChanged>c__AnonStorey0
        {
            internal Action<MapViewBonusElement, bool> onBonusElementClick;
            internal MapViewBonusElement $this;

            internal void <>m__0(bool isChecked)
            {
                this.onBonusElementClick(this.$this, isChecked);
            }

            internal void <>m__1(bool isChecked)
            {
                this.onBonusElementClick(this.$this, isChecked);
            }
        }
    }
}

