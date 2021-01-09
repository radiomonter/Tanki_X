namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TopPanelConstructorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private bool showBackground;
        [SerializeField]
        private bool showBackButton;
        [SerializeField]
        private bool showHeader;

        public bool ShowBackground =>
            this.showBackground;

        public bool ShowBackButton =>
            this.showBackButton;

        public bool ShowHeader =>
            this.showHeader;
    }
}

