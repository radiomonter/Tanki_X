namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ViewUserEmailScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text yourEmailReplaced;
        [SerializeField]
        private Color emailColor = Color.green;

        public string YourEmailReplaced
        {
            set => 
                this.yourEmailReplaced.text = value;
        }

        public string YourEmail { get; set; }

        public Color EmailColor =>
            this.emailColor;
    }
}

