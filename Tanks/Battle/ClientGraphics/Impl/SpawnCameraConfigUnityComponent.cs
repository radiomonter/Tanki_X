namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class SpawnCameraConfigUnityComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float flyHeight = 30f;
        [SerializeField]
        private float flyTimeSec = 2f;

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

