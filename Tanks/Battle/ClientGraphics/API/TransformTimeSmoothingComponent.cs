namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TransformTimeSmoothingComponent : Component
    {
        public TransformTimeSmoothingComponent()
        {
        }

        public TransformTimeSmoothingComponent(UnityEngine.Transform transform)
        {
            this.Transform = transform;
        }

        public bool UseCorrectionByFrameLeader { get; set; }

        public UnityEngine.Transform Transform { get; set; }
    }
}

