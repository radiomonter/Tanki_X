namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UserEnergyCellUIComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private TextMeshProUGUI nickname;
        [SerializeField]
        private TextMeshProUGUI energyValue;
        [SerializeField]
        private TextMeshProUGUI energyGiftText;
        [SerializeField]
        private Color notEnoughColor;
        [SerializeField]
        private Image borederImage;
        [SerializeField]
        private GameObject enoughView;
        [SerializeField]
        private GameObject notEnoughView;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private LocalizedField shareEnergyText;
        [SerializeField]
        private LocalizedField buyEnergyText;
        [SerializeField]
        private GameObject shareButton;
        [SerializeField]
        private GameObject line;
        [SerializeField]
        private LocalizedField chargesAmountSingularText;
        [SerializeField]
        private LocalizedField chargesAmountPlural1Text;
        [SerializeField]
        private LocalizedField chargesAmountPlural2Text;
        [SerializeField]
        private LocalizedField fromText;
        private bool enoughEnergyForEnterToBattle;
        private long shareEnergyValue;
        private bool buy;

        public void HideShareButton()
        {
            base.GetComponent<Animator>().SetBool("showShareButton", false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Entity entity = base.GetComponent<EntityBehaviour>().Entity;
            entity.RemoveComponentIfPresent<AdditionalTeleportEnergyPreviewComponent>();
            entity.AddComponent<AdditionalTeleportEnergyPreviewComponent>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            base.GetComponent<EntityBehaviour>().Entity.RemoveComponentIfPresent<AdditionalTeleportEnergyPreviewComponent>();
        }

        private string Pluralize(int amount)
        {
            CaseType @case = CasesUtil.GetCase(amount);
            string str = "<color=#84F6F6FF>" + amount + "</color>";
            if (@case == CaseType.DEFAULT)
            {
                return string.Format(this.chargesAmountPlural1Text.Value, str);
            }
            if (@case == CaseType.ONE)
            {
                return string.Format(this.chargesAmountSingularText.Value, str);
            }
            if (@case != CaseType.TWO)
            {
                throw new Exception("ivnalid case");
            }
            return string.Format(this.chargesAmountPlural2Text.Value, str);
        }

        public void SetGiftValue(long value, List<string> uids = null)
        {
            if (value == 0L)
            {
                this.energyGiftText.text = string.Empty;
            }
            else
            {
                this.energyGiftText.text = $"{this.Pluralize((int) value)} {this.fromText.Value}
";
                if (uids != null)
                {
                    for (int i = 0; i < uids.Count; i++)
                    {
                        string str = uids[i];
                        this.energyGiftText.text = this.energyGiftText.text + str;
                        if (i != (uids.Count - 1))
                        {
                            this.energyGiftText.text = this.energyGiftText.text + ", ";
                        }
                    }
                }
            }
        }

        public void SetShareEnergyText(long value, bool buy)
        {
            this.shareEnergyValue = value;
            this.buy = buy;
            string str = this.Pluralize((int) value);
            this.text.text = string.Format(!buy ? ((string) this.shareEnergyText) : ((string) this.buyEnergyText), str);
        }

        public void Setup(string nickname, long energyValue, long energyCost)
        {
            this.nickname.text = nickname;
            string str = $"{Mathf.Min((float) energyCost, (float) energyValue)}/{energyCost}";
            if (this.energyValue.text != str)
            {
                this.energyValue.text = str;
                this.HideShareButton();
            }
            this.enoughEnergyForEnterToBattle = energyValue >= energyCost;
            this.energyValue.color = !this.enoughEnergyForEnterToBattle ? this.notEnoughColor : Color.white;
            this.borederImage.color = this.energyValue.color;
            this.enoughView.SetActive(this.enoughEnergyForEnterToBattle);
            this.notEnoughView.SetActive(!this.enoughEnergyForEnterToBattle);
        }

        public void ShowShareButton()
        {
            if (this.shareButtonIsOpen)
            {
                this.HideShareButton();
            }
            else
            {
                EngineService.Engine.ScheduleEvent<HideAllShareButtonsEvent>(new EntityStub());
                base.GetComponent<Animator>().SetBool("showShareButton", true);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        private bool shareButtonIsOpen =>
            base.GetComponent<Animator>().GetBool("showShareButton");

        public bool CellIsFirst
        {
            set
            {
                this.line.SetActive(!value);
                base.GetComponent<LayoutElement>().preferredWidth = !value ? ((float) 150) : ((float) 70);
            }
        }

        public bool Ready =>
            this.enoughEnergyForEnterToBattle;

        public long ShareEnergyValue =>
            this.shareEnergyValue;

        public bool Buy =>
            this.buy;
    }
}

