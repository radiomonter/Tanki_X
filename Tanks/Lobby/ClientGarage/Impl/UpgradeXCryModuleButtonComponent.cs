namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class UpgradeXCryModuleButtonComponent : UpgradeModuleBaseButtonComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private LocalizedField buyBlueprints;
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
            if ((moduleLevel < 0) || (maxCardCount == 0))
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                Color notEnoughColor;
                base.gameObject.SetActive(true);
                base.Activate();
                if (cardsCount < maxCardCount)
                {
                    base.titleText.text = this.buyBlueprints.Value;
                    Color white = Color.white;
                    white.a = 0f;
                    base.fill.color = white;
                    notEnoughColor = base.notEnoughColor;
                    base.titleText.color = notEnoughColor;
                    base.border.color = notEnoughColor;
                }
                else
                {
                    string activate;
                    bool flag2 = userXCryCount >= priceXCry;
                    if (moduleLevel <= -1)
                    {
                        activate = (string) base.activate;
                    }
                    else
                    {
                        string text1;
                        if (flag2)
                        {
                            text1 = priceXCry.ToString();
                        }
                        else
                        {
                            object[] objArray1 = new object[] { "<color=#", base.notEnoughTextColor.ToHexString(), ">", priceXCry, "</color>" };
                            text1 = string.Concat(objArray1);
                        }
                        activate = base.upgrade.Value + "  " + text1 + "<sprite=9>";
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

