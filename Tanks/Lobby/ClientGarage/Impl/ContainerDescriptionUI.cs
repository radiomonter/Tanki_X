namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class ContainerDescriptionUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;

        public TextMeshProUGUI Title
        {
            get => 
                this.title;
            set => 
                this.title = value;
        }

        public TextMeshProUGUI Description
        {
            get => 
                this.description;
            set => 
                this.description = value;
        }
    }
}

