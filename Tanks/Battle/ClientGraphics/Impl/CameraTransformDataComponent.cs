namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CameraTransformDataComponent : Component
    {
        public TransformData Data { get; set; }
    }
}

