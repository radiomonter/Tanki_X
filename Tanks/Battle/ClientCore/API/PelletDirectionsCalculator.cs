namespace Tanks.Battle.ClientCore.API
{
    using System;
    using UnityEngine;

    public static class PelletDirectionsCalculator
    {
        public static Vector3[] GetRandomDirections(HammerPelletConeComponent config, Quaternion worldRotation, Vector3 localDirection)
        {
            int num = Mathf.FloorToInt((float) config.PelletCount);
            Vector3[] vectorArray = new Vector3[num];
            int seed = Random.seed;
            Random.seed = config.ShotSeed;
            for (int i = 0; i < num; i++)
            {
                Vector3 insideUnitCircle = (Vector3) Random.insideUnitCircle;
                insideUnitCircle.x *= config.HorizontalConeHalfAngle;
                insideUnitCircle.y *= config.VerticalConeHalfAngle;
                vectorArray[i] = (Vector3) ((worldRotation * Quaternion.Euler(insideUnitCircle)) * localDirection);
            }
            Random.seed = seed;
            return vectorArray;
        }
    }
}

