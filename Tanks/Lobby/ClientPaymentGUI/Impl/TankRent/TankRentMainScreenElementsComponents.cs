namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class TankRentMainScreenElementsComponents : BehaviourComponent
    {
        [SerializeField]
        private GameObject tankRentScreen;
        public Button tankRentButton;
        public GameObject tankRentOffer;

        public void HideTankRentScreen()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.Main);
            this.tankRentScreen.SetActive(false);
        }

        private void OnDisable()
        {
            this.tankRentButton.gameObject.SetActive(false);
        }

        public void SetButtonToOfferDisplayState()
        {
            this.tankRentButton.onClick.RemoveAllListeners();
            this.tankRentButton.onClick.AddListener(() => this.tankRentOffer.SetActive(true));
        }

        public void SetButtonToScreenDisplayState()
        {
            this.tankRentButton.onClick.RemoveAllListeners();
            this.tankRentButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
            this.tankRentButton.onClick.AddListener(new UnityAction(this.ShowTankRentScreen));
        }

        public void ShowTankRentScreen()
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.HideTankRentScreen));
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.TankRent);
            this.tankRentScreen.SetActive(true);
        }

        private void Update()
        {
            if (InputMapping.Cancel && this.tankRentScreen.activeSelf)
            {
                this.HideTankRentScreen();
            }
        }
    }
}

