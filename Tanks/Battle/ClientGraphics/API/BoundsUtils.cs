namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class BoundsUtils
    {
        public static void DebugBounds(Bounds bounds, Color color)
        {
            DebugBounds(bounds, color, Matrix4x4.identity);
        }

        public static void DebugBounds(Bounds bounds, Color color, Matrix4x4 boundsSpaceToWorld)
        {
            DebugBounds(bounds.min, bounds.max, color, boundsSpaceToWorld);
        }

        public static void DebugBounds(Vector3 min, Vector3 max, Color color, Matrix4x4 boundsSpaceToWorld)
        {
            Vector3 v = new Vector3(min.x, min.y, min.z);
            Vector3 vector2 = new Vector3(max.x, min.y, min.z);
            Vector3 vector3 = new Vector3(max.x, max.y, min.z);
            Vector3 vector4 = new Vector3(min.x, max.y, min.z);
            Vector3 vector5 = new Vector3(min.x, min.y, max.z);
            Vector3 vector6 = new Vector3(max.x, min.y, max.z);
            Vector3 vector7 = new Vector3(max.x, max.y, max.z);
            Vector3 vector8 = new Vector3(min.x, max.y, max.z);
            v = boundsSpaceToWorld.MultiplyPoint3x4(v);
            vector2 = boundsSpaceToWorld.MultiplyPoint3x4(vector2);
            vector3 = boundsSpaceToWorld.MultiplyPoint3x4(vector3);
            vector4 = boundsSpaceToWorld.MultiplyPoint3x4(vector4);
            vector5 = boundsSpaceToWorld.MultiplyPoint3x4(vector5);
            vector6 = boundsSpaceToWorld.MultiplyPoint3x4(vector6);
            vector7 = boundsSpaceToWorld.MultiplyPoint3x4(vector7);
            vector8 = boundsSpaceToWorld.MultiplyPoint3x4(vector8);
            Debug.DrawLine(v, vector2, color, 0f);
            Debug.DrawLine(v, vector2, color, 0f);
            Debug.DrawLine(vector2, vector3, color, 0f);
            Debug.DrawLine(vector3, vector4, color, 0f);
            Debug.DrawLine(vector4, v, color, 0f);
            Debug.DrawLine(vector5, vector6, color, 0f);
            Debug.DrawLine(vector6, vector7, color, 0f);
            Debug.DrawLine(vector7, vector8, color, 0f);
            Debug.DrawLine(vector8, vector5, color, 0f);
            Debug.DrawLine(v, vector5, color, 0f);
            Debug.DrawLine(vector2, vector6, color, 0f);
            Debug.DrawLine(vector4, vector8, color, 0f);
            Debug.DrawLine(vector3, vector7, color, 0f);
        }

        public static void DrawBoundsGizmo(Bounds bounds, Color color, Matrix4x4 boundsSpaceToWorld)
        {
            DrawBoundsGizmo(bounds.min, bounds.max, color, boundsSpaceToWorld);
        }

        public static void DrawBoundsGizmo(Vector3 min, Vector3 max, Color color, Matrix4x4 boundsSpaceToWorld)
        {
            Vector3 v = new Vector3(min.x, min.y, min.z);
            Vector3 vector2 = new Vector3(max.x, min.y, min.z);
            Vector3 vector3 = new Vector3(max.x, max.y, min.z);
            Vector3 vector4 = new Vector3(min.x, max.y, min.z);
            Vector3 vector5 = new Vector3(min.x, min.y, max.z);
            Vector3 vector6 = new Vector3(max.x, min.y, max.z);
            Vector3 vector7 = new Vector3(max.x, max.y, max.z);
            Vector3 vector8 = new Vector3(min.x, max.y, max.z);
            v = boundsSpaceToWorld.MultiplyPoint3x4(v);
            vector2 = boundsSpaceToWorld.MultiplyPoint3x4(vector2);
            vector3 = boundsSpaceToWorld.MultiplyPoint3x4(vector3);
            vector4 = boundsSpaceToWorld.MultiplyPoint3x4(vector4);
            vector5 = boundsSpaceToWorld.MultiplyPoint3x4(vector5);
            vector6 = boundsSpaceToWorld.MultiplyPoint3x4(vector6);
            vector7 = boundsSpaceToWorld.MultiplyPoint3x4(vector7);
            vector8 = boundsSpaceToWorld.MultiplyPoint3x4(vector8);
            Gizmos.color = color;
            Gizmos.DrawLine(v, vector2);
            Gizmos.DrawLine(vector2, vector3);
            Gizmos.DrawLine(vector3, vector4);
            Gizmos.DrawLine(vector4, v);
            Gizmos.DrawLine(vector5, vector6);
            Gizmos.DrawLine(vector6, vector7);
            Gizmos.DrawLine(vector7, vector8);
            Gizmos.DrawLine(vector8, vector5);
            Gizmos.DrawLine(v, vector5);
            Gizmos.DrawLine(vector2, vector6);
            Gizmos.DrawLine(vector4, vector8);
            Gizmos.DrawLine(vector3, vector7);
        }

        public static Bounds TransformBounds(Bounds bounds, Matrix4x4 matrix)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Bounds bounds2 = new Bounds(matrix.MultiplyPoint3x4(min), Vector3.zero);
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, min.y, max.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, max.y, min.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(min.x, max.y, max.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, min.y, min.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, min.y, max.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(new Vector3(max.x, max.y, min.z)));
            bounds2.Encapsulate(matrix.MultiplyPoint3x4(max));
            return bounds2;
        }
    }
}

