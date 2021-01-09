namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ActiveTeleportView : MonoBehaviour
    {
        public Image fill;
        public TextMeshProUGUI text;
        public MultipleBonusView cryBonusView;
        public MultipleBonusView xCryBonusView;
        public MultipleBonusView energyBonusView;
        public ContainerBonusView containerBonusView;
        public DetailBonusView detailBonusView;
        private MapViewBonusElement currentBonusElement;
        private GameObject currentView;

        private void ActivateAll()
        {
            this.text.gameObject.SetActive(true);
            this.cryBonusView.gameObject.SetActive(true);
            this.xCryBonusView.gameObject.SetActive(true);
            this.energyBonusView.gameObject.SetActive(true);
            this.containerBonusView.gameObject.SetActive(true);
            this.detailBonusView.gameObject.SetActive(true);
        }

        private void HideCurrentView()
        {
            this.currentView.GetComponent<Animator>().SetTrigger("hide");
        }

        private void OnEnable()
        {
            this.UpdateView();
        }

        private void ShowView(GameObject view)
        {
            view.GetComponent<Animator>().SetTrigger("show");
            this.currentView = view;
        }

        public void UpdateView()
        {
            this.fill.gameObject.SetActive(true);
            this.fill.fillAmount = 1f;
            this.ActivateAll();
            this.ShowView(this.text.gameObject);
        }

        public void ViewBonus(MapViewBonusElement element)
        {
            this.currentBonusElement = element;
            this.currentView.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                if (this.currentBonusElement == null)
                {
                    this.ShowView(this.text.gameObject);
                }
                else
                {
                    DailyBonusType dailyBonusType = this.currentBonusElement.dailyBonusData.DailyBonusType;
                    switch (dailyBonusType)
                    {
                        case DailyBonusType.NONE:
                            throw new ArgumentException("Unexpected DailyBonusType.NONE ");

                        case DailyBonusType.CRY:
                            this.ShowView(this.cryBonusView.gameObject);
                            this.cryBonusView.UpdateView(this.currentBonusElement.dailyBonusData.CryAmount);
                            break;

                        case DailyBonusType.XCRY:
                            this.ShowView(this.xCryBonusView.gameObject);
                            this.xCryBonusView.UpdateView(this.currentBonusElement.dailyBonusData.XcryAmount);
                            break;

                        case DailyBonusType.ENERGY:
                            this.ShowView(this.energyBonusView.gameObject);
                            this.energyBonusView.UpdateView(this.currentBonusElement.dailyBonusData.EnergyAmount);
                            break;

                        case DailyBonusType.CONTAINER:
                            this.ShowView(this.containerBonusView.gameObject);
                            this.containerBonusView.UpdateView(this.currentBonusElement.dailyBonusData.ContainerReward);
                            break;

                        default:
                            if (dailyBonusType == DailyBonusType.DETAIL)
                            {
                                this.ShowView(this.detailBonusView.gameObject);
                                this.detailBonusView.UpdateView(this.currentBonusElement.dailyBonusData.DetailReward);
                            }
                            break;
                    }
                }
            });
            this.HideCurrentView();
        }
    }
}

