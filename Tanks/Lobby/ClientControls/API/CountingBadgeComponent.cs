namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class CountingBadgeComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI counter;

        public void SetActive(bool active)
        {
            base.gameObject.SetActive(active);
        }

        public int Count
        {
            set => 
                this.counter.text = value.ToString();
        }
    }
}

