namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class ObjectDeactivator : MonoBehaviour
    {
        public Quality.QualityLevel maxQualityForDeactivating;

        private void Awake()
        {
            if (QualitySettings.GetQualityLevel() <= this.maxQualityForDeactivating)
            {
                base.gameObject.SetActive(false);
            }
        }
    }
}

