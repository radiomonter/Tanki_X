namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class DisplayDescriptionItemComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TopTextDescriptionItem description;

        public void SetDescription(string text)
        {
            this.description.text = text;
        }
    }
}

