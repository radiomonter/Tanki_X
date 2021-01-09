namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class CustomBattlesScreenComponent : BehaviourComponent
    {
        public GameObject GameModeItemPrefab;
        public GameObject GameModesContainer;

        public void ScrollToTheLeft()
        {
            base.GetComponentInChildren<ScrollRect>().horizontalNormalizedPosition = 0f;
        }
    }
}

