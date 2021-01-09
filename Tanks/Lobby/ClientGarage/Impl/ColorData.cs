namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ColorData
    {
        [SerializeField]
        public UnityEngine.Color color = UnityEngine.Color.white;
        [SerializeField]
        public UnityEngine.Color hardlightColor = UnityEngine.Color.green;
        [SerializeField]
        public UnityEngine.Material material;
        [SerializeField]
        public bool defaultColor;

        public UnityEngine.Color Color
        {
            get => 
                this.color;
            set => 
                this.color = value;
        }

        public UnityEngine.Color HardlightColor
        {
            get => 
                this.hardlightColor;
            set => 
                this.hardlightColor = value;
        }

        public UnityEngine.Material Material
        {
            get => 
                this.material;
            set => 
                this.material = value;
        }

        public bool DefaultColor
        {
            get => 
                this.defaultColor;
            set => 
                this.defaultColor = value;
        }
    }
}

