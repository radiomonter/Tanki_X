namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class GoldBonusCounterComponent : BehaviourComponent
    {
        [SerializeField]
        private Text count;

        public void SetCount(long count)
        {
            this.count.text = count.ToString();
        }
    }
}

