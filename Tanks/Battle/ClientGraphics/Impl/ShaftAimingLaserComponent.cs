namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e12a21e31aL)]
    public class ShaftAimingLaserComponent : BehaviourComponent
    {
        [SerializeField]
        private float maxLength = 1000f;
        [SerializeField]
        private float minLength = 8f;
        [SerializeField]
        private GameObject asset;
        [SerializeField]
        private float interpolationCoeff = 0.333f;
        public readonly List<ShaftAimingLaserBehaviour> EffectInstances = new List<ShaftAimingLaserBehaviour>();
        [CompilerGenerated]
        private static Predicate<ShaftAimingLaserBehaviour> <>f__am$cache0;

        public ShaftAimingLaserBehaviour EffectInstance
        {
            get => 
                (this.EffectInstances.Count <= 0) ? null : this.EffectInstances[0];
            set
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = item => item == null;
                }
                this.EffectInstances.RemoveAll(<>f__am$cache0);
                this.EffectInstances.Add(value);
            }
        }

        public GameObject Asset
        {
            get => 
                this.asset;
            set => 
                this.asset = value;
        }

        public float MaxLength
        {
            get => 
                this.maxLength;
            set => 
                this.maxLength = value;
        }

        public float MinLength
        {
            get => 
                this.minLength;
            set => 
                this.minLength = value;
        }

        public float InterpolationCoeff
        {
            get => 
                this.interpolationCoeff;
            set => 
                this.interpolationCoeff = value;
        }

        public Vector3 CurrentLaserDirection { get; set; }
    }
}

