namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleResultSpecialOfferUiComponent : ItemContainerComponent
    {
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private GameObject smile;
        [SerializeField]
        private SpecialOfferPriceButtonComponent priceButton;
        [SerializeField]
        private SpecialOfferCrystalButtonComponent crystalButton;
        [SerializeField]
        private SpecialOfferUseDiscountComponent useDiscountButton;
        [SerializeField]
        private SpecialOfferTakeRewardButtonComponent takeRewardButton;
        [SerializeField]
        private Button tutorialRewardButton;
        [SerializeField]
        private SpecialOfferOpenContainerButton openButton;
        [SerializeField]
        private SpecialOfferWorthItComponent worthIt;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private LocalizedField tutorialCongratulationLocalizedField;
        private bool xBonus;

        public void Appear()
        {
            this.animator.SetTrigger("Appear");
        }

        private void Awake()
        {
            this.tutorialRewardButton.onClick.AddListener(delegate {
                this.DeactivateAllButtons();
                this.ShowSmile(this.tutorialCongratulationLocalizedField.Value);
            });
        }

        public void CoolAppear()
        {
            this.animator.SetTrigger("CoolAppear");
        }

        public void DeactivateAllButtons()
        {
            this.xBonus = false;
            this.priceButton.gameObject.SetActive(false);
            this.useDiscountButton.gameObject.SetActive(false);
            this.takeRewardButton.gameObject.SetActive(false);
            this.crystalButton.gameObject.SetActive(false);
            this.openButton.gameObject.SetActive(false);
            this.tutorialRewardButton.gameObject.SetActive(false);
            this.animator.SetTrigger("ButtonFlash");
        }

        public void Disappear()
        {
            this.animator.SetTrigger("Disappear");
        }

        public void HideDiscountButton()
        {
            this.useDiscountButton.gameObject.SetActive(false);
        }

        public void SetCrystalButton(int discountPrice, int regularPrice, int labelPercentage, bool xCry)
        {
            this.DeactivateAllButtons();
            this.worthIt.SetLabel(labelPercentage);
            this.crystalButton.gameObject.SetActive(true);
            this.crystalButton.SetPrice(regularPrice, discountPrice, xCry);
        }

        public void SetOpenButton(long containerId, int quantity, Action onOpen)
        {
            this.DeactivateAllButtons();
            this.worthIt.SetLabel(0);
            this.openButton.containerId = containerId;
            this.openButton.quantity = quantity;
            this.openButton.onOpen = onOpen;
            this.openButton.gameObject.SetActive(true);
        }

        public void SetPriceButton(int discount, double regularPrice, int labelPercentage, string currency)
        {
            this.DeactivateAllButtons();
            this.worthIt.SetLabel(labelPercentage);
            this.priceButton.gameObject.SetActive(true);
            this.priceButton.SetPrice(regularPrice, discount, currency);
        }

        public void SetTakeRewardButton()
        {
            this.DeactivateAllButtons();
            this.worthIt.SetLabel(0);
            this.takeRewardButton.gameObject.SetActive(true);
        }

        public void SetTutorialRewardsButton()
        {
            this.DeactivateAllButtons();
            this.worthIt.SetLabel(0);
            this.tutorialRewardButton.gameObject.SetActive(true);
        }

        public void SetUseDiscountButton()
        {
            this.DeactivateAllButtons();
            this.xBonus = true;
            this.worthIt.SetLabel(0);
            this.useDiscountButton.gameObject.SetActive(true);
        }

        public void ShowContent(string titleText, string descriptionText, List<SpecialOfferItem> items)
        {
            this.titleText.text = titleText;
            this.descriptionText.text = descriptionText;
            this.smile.SetActive(false);
            base.ClearItems();
            base.InstantiateItems(items);
        }

        public void ShowDiscountButtonIfXBonus()
        {
            if (this.xBonus)
            {
                this.useDiscountButton.gameObject.SetActive(true);
            }
        }

        public void ShowSmile(string titleText)
        {
            this.DeactivateAllButtons();
            base.ClearItems();
            this.worthIt.SetLabel(0);
            this.titleText.text = titleText;
            this.descriptionText.text = string.Empty;
            this.smile.SetActive(true);
        }
    }
}

