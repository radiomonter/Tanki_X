namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FreeCameraComponent : Component
    {
        public bool fixedHeight;
        public float speedUp = 4f;
        public float slowDown = 0.5f;
        public float flySpeed = 13f;
        public float positionSmoothingSpeed = 5f;
        public float xRotationSpeed = 140f;
        public float yRotationSpeed = 140f;
        public float rotationSmoothingSpeed = 5f;
        public float xMinAngle = -86f;
        public float xMaxAngle = 86f;

        public TransformData Data { get; set; }
    }
}

