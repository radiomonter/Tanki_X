namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x3a1ad35ab81924dbL)]
    public class ShaftAimingWorkingStateComponent : TimeValidateComponent
    {
        public ShaftAimingWorkingStateComponent()
        {
            this.VerticalAngle = 0f;
            this.VerticalSpeed = 0f;
            this.VerticalElevationDir = 0;
            this.InitialEnergy = 0f;
            this.ExhaustedEnergy = 0f;
            this.WorkingDirection = Vector3.zero;
            this.IsActive = false;
        }

        public float InitialEnergy { get; set; }

        public float ExhaustedEnergy { get; set; }

        public float VerticalAngle { get; set; }

        public Vector3 WorkingDirection { get; set; }

        public float VerticalSpeed { get; set; }

        public int VerticalElevationDir { get; set; }

        public bool IsActive { get; set; }
    }
}

