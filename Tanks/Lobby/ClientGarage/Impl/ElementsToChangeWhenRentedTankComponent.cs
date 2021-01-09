namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ElementsToChangeWhenRentedTankComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject[] elementsToHide;
        [SerializeField]
        private Button[] buttonsToDeactivate;
        [SerializeField]
        private GameObject[] elementsToActivate;

        public void ReturnScreenToNormalState()
        {
            foreach (GameObject obj2 in this.elementsToHide)
            {
                obj2.SetActive(true);
            }
            foreach (Button button in this.buttonsToDeactivate)
            {
                button.interactable = true;
            }
            foreach (GameObject obj3 in this.elementsToActivate)
            {
                obj3.SetActive(false);
            }
        }

        public void SetScreenToRentedTankState()
        {
            foreach (GameObject obj2 in this.elementsToHide)
            {
                obj2.SetActive(false);
            }
            foreach (Button button in this.buttonsToDeactivate)
            {
                button.interactable = false;
            }
            foreach (GameObject obj3 in this.elementsToActivate)
            {
                obj3.SetActive(true);
            }
        }
    }
}

