namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e12d0505daL)]
    public class ShaftAimingReticleEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject assetReticle;

        public GameObject AssetReticle
        {
            get => 
                this.assetReticle;
            set => 
                this.assetReticle = value;
        }

        public Animator ReticleAnimator { get; set; }

        public GameObject InstanceReticle { get; set; }

        public Transform DynamicReticle { get; set; }

        public CanvasGroup ReticleGroup { get; set; }

        public Tanks.Battle.ClientGraphics.Impl.ShaftReticleSpotBehaviour ShaftReticleSpotBehaviour { get; set; }

        public Material ReticleSpotMaterialInstance { get; set; }

        public Vector2 CanvasSize { get; set; }
    }
}

