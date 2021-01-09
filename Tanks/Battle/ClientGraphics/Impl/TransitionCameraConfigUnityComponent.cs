namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TransitionCameraConfigUnityComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float flyHeight = 1f;
        [SerializeField]
        private float flyTimeSec = 0.55f;

        public float FlyHeight
        {
            get => 
                this.flyHeight;
            set => 
                this.flyHeight = value;
        }

        public float FlyTimeSec
        {
            get => 
                this.flyTimeSec;
            set => 
                this.flyTimeSec = value;
        }
    }
}

