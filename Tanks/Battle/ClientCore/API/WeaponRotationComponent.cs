namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x14d9915e4b6L)]
    public class WeaponRotationComponent : Component
    {
        private float speed;
        private float acceleration;
        private float baseSpeed;

        public float Speed
        {
            get => 
                this.speed;
            set => 
                this.speed = value;
        }

        public float Acceleration
        {
            get => 
                this.acceleration;
            set => 
                this.acceleration = value;
        }

        public float BaseSpeed
        {
            get => 
                this.baseSpeed;
            set => 
                this.baseSpeed = value;
        }
    }
}

