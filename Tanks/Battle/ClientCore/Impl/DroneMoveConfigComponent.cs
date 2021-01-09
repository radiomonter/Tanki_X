namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x32139899b27L), Shared]
    public class DroneMoveConfigComponent : Component
    {
        private Vector3 spawnPosition;
        private Vector3 flyPosition;
        private float rotationSpeed;
        private float moveSpeed;
        private float acceleration;

        public float Acceleration
        {
            get => 
                this.acceleration;
            set => 
                this.acceleration = value;
        }

        public Vector3 SpawnPosition
        {
            get => 
                this.spawnPosition;
            set => 
                this.spawnPosition = value;
        }

        public Vector3 FlyPosition
        {
            get => 
                this.flyPosition;
            set => 
                this.flyPosition = value;
        }

        public float RotationSpeed
        {
            get => 
                this.rotationSpeed;
            set => 
                this.rotationSpeed = value;
        }

        public float MoveSpeed
        {
            get => 
                this.moveSpeed;
            set => 
                this.moveSpeed = value;
        }
    }
}

