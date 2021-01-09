namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WeaponRotationControlComponent : Component
    {
        private bool forceGyroscopeEnabled;

        public bool IsRotating() => 
            this.EffectiveControl != 0f;

        public float Control { get; set; }

        public bool CenteringControl { get; set; }

        public float EffectiveControl { get; set; }

        public bool Centering { get; set; }

        public float PrevEffectiveControl { get; set; }

        public float PrevDeltaRotaion { get; set; }

        public float Rotation { get; set; }

        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public float MouseRotationCumulativeHorizontalAngle { get; set; }

        public float MouseRotationCumulativeVerticalAngle { get; set; }

        public int ShaftElevationDirectionByKeyboard { get; set; }

        public double PrevControlChangedTime { get; set; }

        public float MouseShaftAimCumulativeVerticalAngle { get; set; }

        public bool ForceGyroscopeEnabled
        {
            get => 
                this.forceGyroscopeEnabled;
            set => 
                this.forceGyroscopeEnabled = value;
        }

        public bool BlockRotate { get; set; }
    }
}

