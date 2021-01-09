namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class UpgradeModuleButtonComponent : UpgradeModuleBaseButtonComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private LocalizedField notEnoughBlueprints;
        [SerializeField]
        private GameObject content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.content != null)
            {
                foreach (ModulePropertyView view in this.content.GetComponentsInChildren<ModulePropertyView>())
                {
                    view.FillNext.SetActive(true);
                    view.NextString.SetActive(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.content != null)
            {
                foreach (ModulePropertyView view in this.content.GetComponentsInChildren<ModulePropertyView>())
                {
                    view.FillNext.SetActive(false);
                    view.NextString.SetActive(false);
                }
            }
        }

        public override void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount)
        {
            if (moduleLevel < 0)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                if (maxCardCount == 0)
                {
                    base.FullUpgraded();
                }
                else
                {
                    Color notEnoughColor;
                    base.Activate();
                    if (cardsCount < maxCardCount)
                    {
                        base.GetComponent<Button>().interactable = false;
                        base.titleText.text = this.notEnoughBlueprints.Value;
                        base.fill.color = base.notEnoughFillColor;
                        notEnoughColor = base.notEnoughColor;
                        base.titleText.color = notEnoughColor;
                        base.border.color = notEnoughColor;
                    }
                    else
                    {
                        string activate;
                        base.GetComponent<Button>().interactable = true;
                        bool flag2 = userCryCount >= price;
                        if (moduleLevel <= -1)
                        {
                            activate = (string) base.activate;
                        }
                        else
                        {
                            string text1;
                            if (flag2)
                            {
                                text1 = price.ToString();
                            }
                            else
                            {
                                object[] objArray1 = new object[] { "<color=#", base.notEnoughTextColor.ToHexString(), ">", price, "</color>" };
                                text1 = string.Concat(objArray1);
                            }
                            activate = base.upgrade.Value + "  " + text1 + "<sprite=8>";
                        }
                        base.titleText.text = activate;
                        notEnoughColor = !flag2 ? base.notEnoughColor : base.enoughColor;
                        base.titleText.color = notEnoughColor;
                        base.fill.color = notEnoughColor;
                        base.border.color = notEnoughColor;
                    }
                }
            }
        }
    }
}

