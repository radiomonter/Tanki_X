namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class ProjectorRotationUtility
    {
        private static readonly Quaternion s_RotationOffset = Quaternion.Euler(-90f, 0f, 0f);

        public static Quaternion ProjectorRotation(Vector3 a_ProjectionDirection, Vector3 a_ProjectionUpDirection) => 
            Quaternion.LookRotation(a_ProjectionDirection, a_ProjectionUpDirection) * s_RotationOffset;
    }
}

