namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class LinkButtonComponent : UIBehaviour, Component
    {
        [SerializeField]
        private string link;

        public string Link
        {
            get => 
                this.link;
            set => 
                this.link = value;
        }
    }
}

