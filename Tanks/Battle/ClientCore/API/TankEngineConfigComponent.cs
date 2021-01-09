namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TankEngineConfigComponent : MonoBehaviour, Component
    {
        [SerializeField, Range(0f, 1f)]
        private float minEngineMovingBorder;
        [SerializeField, Range(0f, 1f)]
        private float maxEngineMovingBorder;
        [SerializeField, Range(0f, 1f)]
        private float engineTurningBorder;
        [SerializeField]
        private float engineCollisionIntervalSec = 0.5f;

        public float EngineCollisionIntervalSec
        {
            get => 
                this.engineCollisionIntervalSec;
            set => 
                this.engineCollisionIntervalSec = value;
        }

        public float MinEngineMovingBorder
        {
            get => 
                this.minEngineMovingBorder;
            set => 
                this.minEngineMovingBorder = value;
        }

        public float MaxEngineMovingBorder
        {
            get => 
                this.maxEngineMovingBorder;
            set => 
                this.maxEngineMovingBorder = value;
        }

        public float EngineTurningBorder
        {
            get => 
                this.engineTurningBorder;
            set => 
                this.engineTurningBorder = value;
        }
    }
}

