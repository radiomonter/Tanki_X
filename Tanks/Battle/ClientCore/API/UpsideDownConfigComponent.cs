namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UpsideDownConfigComponent : Component
    {
        private float angleCosToCheck;
        private float angleToNormal;

        public float GetUpsideDownCosToCheck() => 
            this.angleCosToCheck;

        public float UpsideDownAngleToNormal
        {
            get => 
                this.angleToNormal;
            set
            {
                this.angleToNormal = value;
                this.angleCosToCheck = (float) -Math.Cos((double) (value * 0.01745329f));
            }
        }

        public float DetectionPauseSec { get; set; }

        public int MaxRankForMessage { get; set; }
    }
}

