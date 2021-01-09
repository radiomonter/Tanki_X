namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class AngleValues
    {
        private static readonly float PI = 180f;
        private float currentAngle;
        private float totalAngle;
        private float angularAcceleration;
        private float angularSpeed;
        private float angleDirection;

        public AngleValues(float startAngle, float targetAngle, float accelerationCoeff)
        {
            this.CalculateAngleAndDirection(startAngle, targetAngle);
            this.CalculateShortestAngle();
            this.CalculateAccelerationAndSpeed(accelerationCoeff);
        }

        private void CalculateAccelerationAndSpeed(float accelerationCoeff)
        {
            this.angularAcceleration = accelerationCoeff * this.totalAngle;
            this.angularSpeed = 0f;
            this.currentAngle = 0f;
        }

        private void CalculateAngleAndDirection(float startAngle, float targetAngle)
        {
            this.totalAngle = targetAngle - startAngle;
            if (this.totalAngle >= 0f)
            {
                this.angleDirection = 1f;
            }
            else
            {
                this.totalAngle = -this.totalAngle;
                this.angleDirection = -1f;
            }
        }

        private void CalculateShortestAngle()
        {
            if (this.totalAngle > PI)
            {
                this.angleDirection = -this.angleDirection;
                this.totalAngle = (2f * PI) - this.totalAngle;
            }
        }

        public void ReverseAcceleration()
        {
            this.angularAcceleration = -this.angularAcceleration;
        }

        public float Update(float dt)
        {
            if (this.currentAngle >= this.totalAngle)
            {
                return 0f;
            }
            float num = this.angularAcceleration * dt;
            float num2 = (this.angularSpeed + (0.5f * num)) * dt;
            this.angularSpeed += num;
            float num3 = this.totalAngle - this.currentAngle;
            if (num3 < num2)
            {
                num2 = num3;
            }
            this.currentAngle += num2;
            return (num2 * this.angleDirection);
        }
    }
}

