namespace CurvedUI
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class CurvedUIExtensionMethods
    {
        public static int Abs(this int value) => 
            Mathf.Abs(value);

        public static float Abs(this float value) => 
            Mathf.Abs(value);

        public static T AddComponentIfMissing<T>(this Component go) where T: Component => 
            go.gameObject.AddComponentIfMissing<T>();

        public static T AddComponentIfMissing<T>(this GameObject go) where T: Component => 
            (go.GetComponent<T>() != null) ? go.GetComponent<T>() : go.AddComponent<T>();

        public static bool AlmostEqual(this Vector3 a, Vector3 b, double accuracy = 0.01) => 
            Vector3.SqrMagnitude(a - b) < accuracy;

        public static int CeilToInt(this float value) => 
            Mathf.FloorToInt(value);

        public static float Clamp(this int value, int min, int max) => 
            (float) Mathf.Clamp(value, min, max);

        public static float Clamp(this float value, float min, float max) => 
            Mathf.Clamp(value, min, max);

        public static int FloorToInt(this float value) => 
            Mathf.FloorToInt(value);

        public static Vector2 ModifyVectorX(this Vector2 trans, float newVal)
        {
            trans = new Vector3(newVal, trans.y);
            return trans;
        }

        public static Vector2 ModifyVectorY(this Vector2 trans, float newVal)
        {
            trans = new Vector3(trans.x, newVal);
            return trans;
        }

        public static Vector3 ModifyX(this Vector3 trans, float newVal)
        {
            trans = new Vector3(newVal, trans.y, trans.z);
            return trans;
        }

        public static Vector3 ModifyY(this Vector3 trans, float newVal)
        {
            trans = new Vector3(trans.x, newVal, trans.z);
            return trans;
        }

        public static Vector3 ModifyZ(this Vector3 trans, float newVal)
        {
            trans = new Vector3(trans.x, trans.y, newVal);
            return trans;
        }

        public static double Remap(this double value, double from1, double to1, double from2, double to2) => 
            (((value - from1) / (to1 - from1)) * (to2 - from2)) + from2;

        public static float Remap(this int value, float from1, float to1, float from2, float to2) => 
            (((value - from1) / (to1 - from1)) * (to2 - from2)) + from2;

        public static float Remap(this float value, float from1, float to1, float from2, float to2) => 
            (((value - from1) / (to1 - from1)) * (to2 - from2)) + from2;

        public static float RemapAndClamp(this float value, float from1, float to1, float from2, float to2) => 
            value.Remap(from1, to1, from2, to2).Clamp(from2, to2);

        public static void ResetTransform(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        public static int ToInt(this float value) => 
            Mathf.RoundToInt(value);
    }
}

