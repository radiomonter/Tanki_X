namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaftAimingRendererEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private int transparentRenderQueue = 0xbb8;
        [SerializeField]
        private float alphaRecoveringSpeed = 2f;

        public float AlphaRecoveringSpeed
        {
            get => 
                this.alphaRecoveringSpeed;
            set => 
                this.alphaRecoveringSpeed = value;
        }

        public int TransparentRenderQueue
        {
            get => 
                this.transparentRenderQueue;
            set => 
                this.transparentRenderQueue = value;
        }
    }
}

