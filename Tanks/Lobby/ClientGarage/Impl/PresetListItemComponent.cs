namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PresetListItemComponent : UIBehaviour, Component
    {
        [SerializeField]
        private GameObject iconObject;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private Graphic bgGraphic;
        [SerializeField]
        private Color lockedColor;
        [SerializeField]
        private Color unlockedColor;
        private int rank;
        private bool locked;

        public Entity Preset { get; set; }

        public bool IsUserItem { get; set; }

        public bool IsOwned { get; set; }

        public string PresetName
        {
            get => 
                this.text.text;
            set => 
                this.text.text = value;
        }

        public int Rank
        {
            get => 
                this.rank;
            set => 
                this.rank = value;
        }

        public bool Locked
        {
            get => 
                this.locked;
            set
            {
                this.locked = value;
                this.iconObject.SetActive(value);
                this.bgGraphic.color = !value ? this.unlockedColor : this.lockedColor;
            }
        }
    }
}

