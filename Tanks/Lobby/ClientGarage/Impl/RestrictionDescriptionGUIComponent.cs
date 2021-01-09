namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class RestrictionDescriptionGUIComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text description;

        protected RestrictionDescriptionGUIComponent()
        {
        }

        public string Description
        {
            set => 
                this.description.text = value;
        }
    }
}

