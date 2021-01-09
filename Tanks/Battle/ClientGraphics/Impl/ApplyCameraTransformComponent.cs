namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ApplyCameraTransformComponent : Component
    {
        public float positionSmoothingRatio = 1f;
        public float rotationSmoothingRatio = 1f;
    }
}

