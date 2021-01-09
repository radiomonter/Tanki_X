namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public static class MovementComparer
    {
        private const float NEAR_DISTANCE = 0.3f;
        private const float NEAR_DISTANCE_SQR = 0.09f;
        private const float NEAR_VELOCITY = 0.5f;
        private const float NEAR_VELOCITY_SQR = 0.25f;
        private const float NEAR_ORIENTATION_DEGREES = 4f;
        private const float NEAR_ORIENTATION_RADIANS = 0.06981317f;
        private const float NEAR_ROTATION_DEGREES = 10f;
        private const float NEAR_ROTATION_RADIANS = 0.1745329f;

        private static bool CheckAngularVelocity(ref Movement movement1, ref Movement movement2) => 
            MathUtil.NearlyEqual(movement2.AngularVelocity, movement1.AngularVelocity, 0.1745329f);

        private static bool CheckDistance(ref Movement movement1, ref Movement movement2) => 
            Vector3.SqrMagnitude(movement1.Position - movement2.Position) < 0.09f;

        private static bool CheckLinearVelocity(ref Movement movement1, ref Movement movement2) => 
            Vector3.SqrMagnitude(movement1.Velocity - movement2.Velocity) < 0.25f;

        private static bool CheckRotation(ref Movement movement1, ref Movement movement2) => 
            Quaternion.Angle(movement1.Orientation, movement2.Orientation) < 4f;

        public static bool IsMovementAlmostEqual(ref Movement movement1, ref Movement movement2) => 
            (CheckDistance(ref movement1, ref movement2) && (CheckRotation(ref movement1, ref movement2) && CheckLinearVelocity(ref movement1, ref movement2))) && CheckAngularVelocity(ref movement1, ref movement2);
    }
}

