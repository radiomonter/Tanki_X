namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShaftAimingColorEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Color redColor = new Color(255f, 0f, 0f);
        [SerializeField]
        private Color blueColor = new Color(0f, 187f, 255f);

        public Color RedColor
        {
            get => 
                this.redColor;
            set => 
                this.redColor = value;
        }

        public Color BlueColor
        {
            get => 
                this.blueColor;
            set => 
                this.blueColor = value;
        }

        public Color ChoosenColor { get; set; }
    }
}

