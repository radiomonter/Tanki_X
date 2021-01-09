namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class HomeScreenComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text uidText;
        [SerializeField]
        private GameObject cbqBadge;
        [SerializeField]
        private GameObject battleLobbyScreen;

        public virtual string UidText
        {
            get => 
                this.uidText.text;
            set => 
                this.uidText.text = value;
        }

        public virtual GameObject CbqBadge =>
            this.cbqBadge;

        public virtual GameObject BattleLobbyScreen =>
            this.battleLobbyScreen;
    }
}

