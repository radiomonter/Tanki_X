namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal static class Vector3Extension
    {
        public static bool Approximately(Vector3 a_Vector1, Vector3 a_Vector2) => 
            (Mathf.Approximately(a_Vector1.x, a_Vector2.x) && Mathf.Approximately(a_Vector1.y, a_Vector2.y)) && Mathf.Approximately(a_Vector1.z, a_Vector2.z);

        public static bool Approximately(Vector3 a_Vector1, Vector3 a_Vector2, float a_MaximumAbsoluteError, float a_MaximumRelativeError) => 
            (MathfExtension.Approximately(a_Vector1.x, a_Vector2.x, a_MaximumAbsoluteError, a_MaximumRelativeError) && MathfExtension.Approximately(a_Vector1.y, a_Vector2.y, a_MaximumAbsoluteError, a_MaximumRelativeError)) && MathfExtension.Approximately(a_Vector1.z, a_Vector2.z, a_MaximumAbsoluteError, a_MaximumRelativeError);

        public static Vector3 MirrorAtPlane(this Vector3 a_Vector, Vector3 a_PlaneNormal)
        {
            Vector3 vector;
            float a = Vector3.Dot(a_Vector, a_PlaneNormal);
            if (Mathf.Approximately(a, 0f))
            {
                vector = a_Vector;
            }
            else
            {
                float num2 = Vector3.SqrMagnitude(a_PlaneNormal);
                float num3 = (-2f * a) / num2;
                vector = a_Vector + (num3 * a_PlaneNormal);
            }
            return vector;
        }
    }
}

