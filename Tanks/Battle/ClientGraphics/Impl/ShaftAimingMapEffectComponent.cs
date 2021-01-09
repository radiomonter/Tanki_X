namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaftAimingMapEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float shrubsHidingRadiusMin = 20f;
        [SerializeField]
        private float shrubsHidingRadiusMax = 80f;
        [SerializeField]
        private Shader hidingLeavesShader;
        [SerializeField]
        private Shader defaultLeavesShader;
        [SerializeField]
        private Shader hidingBillboardTreesShader;
        [SerializeField]
        private Shader defaultBillboardTreesShader;

        public float ShrubsHidingRadiusMin
        {
            get => 
                this.shrubsHidingRadiusMin;
            set => 
                this.shrubsHidingRadiusMin = value;
        }

        public float ShrubsHidingRadiusMax
        {
            get => 
                this.shrubsHidingRadiusMax;
            set => 
                this.shrubsHidingRadiusMax = value;
        }

        public Shader HidingLeavesShader
        {
            get => 
                this.hidingLeavesShader;
            set => 
                this.hidingLeavesShader = value;
        }

        public Shader DefaultLeavesShader
        {
            get => 
                this.defaultLeavesShader;
            set => 
                this.defaultLeavesShader = value;
        }

        public Shader HidingBillboardTreesShader
        {
            get => 
                this.hidingBillboardTreesShader;
            set => 
                this.hidingBillboardTreesShader = value;
        }

        public Shader DefaultBillboardTreesShader
        {
            get => 
                this.defaultBillboardTreesShader;
            set => 
                this.defaultBillboardTreesShader = value;
        }
    }
}

