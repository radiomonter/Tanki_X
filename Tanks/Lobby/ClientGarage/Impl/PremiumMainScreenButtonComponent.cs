namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class PremiumMainScreenButtonComponent : BehaviourComponent
    {
        public GameObject activePremiumIcon;
        public GameObject inactivePremiumIcon;

        public void ActivatePremium()
        {
            this.ActivatePremium(true);
        }

        private void ActivatePremium(bool val)
        {
            this.activePremiumIcon.SetActive(val);
            this.inactivePremiumIcon.SetActive(!val);
        }

        public void DeactivatePremium()
        {
            this.ActivatePremium(false);
        }
    }
}

