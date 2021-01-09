namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class KalmanFilter
    {
        public static readonly float MEASUREMENT_NOISE = 2f;
        public static readonly float ENVIRONMENT_NOISE = 20f;
        public static readonly float FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE = 1f;
        public static readonly float FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE = 1f;
        public static readonly float INIT_COVARIANCE = 0.1f;
        private Vector3 predictedState;
        private float predictedCovariance;
        private float covariance;

        public KalmanFilter(Vector3 initState)
        {
            this.Reset(initState);
        }

        public void Correct(Vector3 data)
        {
            this.TimeUpdatePrediction();
            this.MeasurementUpdateCorrection(data);
        }

        private void MeasurementUpdateCorrection(Vector3 data)
        {
            float val = (FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE * this.predictedCovariance) / (((FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE * this.predictedCovariance) * FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE) + ENVIRONMENT_NOISE);
            if (!PhysicsUtil.IsValidFloat(val))
            {
                this.Reset(this.State);
            }
            else
            {
                this.State = this.predictedState + (val * (data - (FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE * this.predictedState)));
                this.covariance = (1f - (val * FACTOR_MEASURED_VALUE_TO_PREVIOUS_VALUE)) * this.predictedCovariance;
            }
        }

        public void Reset(Vector3 initState)
        {
            this.State = initState;
            this.covariance = INIT_COVARIANCE;
        }

        private void TimeUpdatePrediction()
        {
            this.predictedState = (Vector3) (FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE * this.State);
            this.predictedCovariance = ((FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE * this.covariance) * FACTOR_REAL_VALUE_TO_PREVIOUS_VALUE) + MEASUREMENT_NOISE;
        }

        public Vector3 State { get; private set; }
    }
}

