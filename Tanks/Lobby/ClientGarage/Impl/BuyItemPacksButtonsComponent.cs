namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BuyItemPacksButtonsComponent : BehaviourComponent
    {
        [SerializeField]
        private List<EntityBehaviour> buyButtons;
        [CompilerGenerated]
        private static Action<EntityBehaviour> <>f__am$cache0;

        public void SetBuyButtonsInactive()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = button => button.gameObject.SetActive(false);
            }
            this.buyButtons.ForEach(<>f__am$cache0);
        }

        public List<EntityBehaviour> BuyButtons =>
            this.buyButtons;
    }
}

