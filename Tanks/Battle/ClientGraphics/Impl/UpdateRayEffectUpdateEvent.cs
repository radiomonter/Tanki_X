namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class UpdateRayEffectUpdateEvent : Event
    {
        public float[] speedMultipliers = new float[3];
        public float[] bezierPointsRandomness = new float[3];
    }
}

