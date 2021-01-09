namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ApplyCameraTransformEvent : Event
    {
        private static ApplyCameraTransformEvent INSTANCE = new ApplyCameraTransformEvent();
        private bool positionSmoothingRatioValid;
        private bool rotationSmoothingRatioValid;
        private bool deltaTimeValid;
        private float positionSmoothingRatio;
        private float rotationSmoothingRatio;
        private float deltaTime;

        private ApplyCameraTransformEvent()
        {
            this.ResetFields();
        }

        public static ApplyCameraTransformEvent ResetApplyCameraTransformEvent()
        {
            INSTANCE.ResetFields();
            return INSTANCE;
        }

        private void ResetFields()
        {
            this.positionSmoothingRatioValid = false;
            this.rotationSmoothingRatioValid = false;
            this.deltaTimeValid = false;
        }

        public float PositionSmoothingRatio
        {
            get => 
                this.positionSmoothingRatio;
            set
            {
                this.positionSmoothingRatioValid = true;
                this.positionSmoothingRatio = value;
            }
        }

        public float RotationSmoothingRatio
        {
            get => 
                this.rotationSmoothingRatio;
            set
            {
                this.rotationSmoothingRatioValid = true;
                this.rotationSmoothingRatio = value;
            }
        }

        public float DeltaTime
        {
            get => 
                this.deltaTime;
            set
            {
                this.deltaTimeValid = true;
                this.deltaTime = value;
            }
        }

        public bool PositionSmoothingRatioValid =>
            this.positionSmoothingRatioValid;

        public bool RotationSmoothingRatioValid =>
            this.rotationSmoothingRatioValid;

        public bool DeltaTimeValid =>
            this.deltaTimeValid;
    }
}

