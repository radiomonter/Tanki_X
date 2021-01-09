namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UserCardsUIComponent : UIBehaviour, Component
    {
        [SerializeField]
        private Text[] resourceCountTexts;

        public void ResetCardsCount()
        {
            this.SetCardsCount(0L, 0L);
        }

        public void SetCardsCount(long type, long count)
        {
            int index = 0;
            this.resourceCountTexts[index].text = count.ToString();
        }
    }
}

