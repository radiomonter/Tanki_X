namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeLevelRestrictionBadgeGUIComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text restrictionValueText;

        public string RestrictionValue
        {
            set => 
                this.restrictionValueText.text = value;
        }
    }
}

