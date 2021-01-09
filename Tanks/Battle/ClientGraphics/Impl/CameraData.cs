namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class CameraData
    {
        public float collisionDistanceRatio;
        public float linearSpeed;
        public float pitchSpeed;
        public float yawSpeed;
        public bool pitchCorrectionEnabled = true;
    }
}

