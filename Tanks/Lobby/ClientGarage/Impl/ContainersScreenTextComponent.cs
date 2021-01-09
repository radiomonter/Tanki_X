namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ContainersScreenTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text containersButtonText;
        [SerializeField]
        private Text openContainerButtonText;
        [SerializeField]
        private Text openAllContainerButtonText;
        [SerializeField]
        private Text emptyListText;

        public virtual string ContainersButtonText
        {
            set => 
                this.containersButtonText.text = value;
        }

        public virtual string OpenContainerButtonText
        {
            set => 
                this.openContainerButtonText.text = value;
        }

        public virtual string OpenAllContainerButtonText
        {
            set => 
                this.openAllContainerButtonText.text = value;
        }

        public virtual string EmptyListText
        {
            set => 
                this.emptyListText.text = value;
        }
    }
}

