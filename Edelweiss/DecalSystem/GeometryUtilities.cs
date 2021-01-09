namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class GeometryUtilities
    {
        private const float c_Epsilon = 1E-10f;

        public static bool AreLinesParallel(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start, Vector3 a_Line2End)
        {
            bool flag = false;
            Vector3 lhs = a_Line1End - a_Line1Start;
            Vector3 rhs = a_Line2End - a_Line2Start;
            lhs.Normalize();
            rhs.Normalize();
            if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(lhs, rhs)), 0f))
            {
                flag = true;
            }
            return flag;
        }

        private static float FactorOfPointOnLine(Vector3 a_Point, Vector3 a_LineStart, Vector3 a_LineEnd)
        {
            float x;
            float y;
            float z;
            Vector3 vector = a_LineEnd - a_LineStart;
            Vector3 vector2 = vector;
            vector2.x = Mathf.Abs(vector2.x);
            vector2.y = Mathf.Abs(vector2.y);
            vector2.z = Mathf.Abs(vector2.z);
            if ((vector2.x > vector2.y) && (vector2.x > vector2.z))
            {
                x = a_Point.x;
                y = a_LineStart.x;
                z = vector.x;
            }
            else if ((vector2.y > vector2.x) && (vector2.y > vector2.z))
            {
                x = a_Point.y;
                y = a_LineStart.y;
                z = vector.y;
            }
            else
            {
                x = a_Point.z;
                y = a_LineStart.z;
                z = vector.z;
            }
            return ((x - y) / z);
        }

        public static bool IsPointOnLine(Vector3 a_Point, Vector3 a_LineStart, Vector3 a_LineEnd)
        {
            bool flag = false;
            Vector3 lhs = a_Point - a_LineStart;
            Vector3 rhs = a_Point - a_LineEnd;
            if (Vector3.Cross(lhs, rhs).sqrMagnitude < 1E-10f)
            {
                float num2 = Vector3.Dot(lhs, rhs);
                if ((num2 <= 0f) && (num2 <= (a_LineEnd - a_LineStart).sqrMagnitude))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static bool IsQuadrangleConvex(Vector3 a_Vertex1, Vector3 a_Vertex2, Vector3 a_Vertex3, Vector3 a_Vertex4)
        {
            Vector3 vector;
            return LineIntersection(a_Vertex1, a_Vertex3, a_Vertex2, a_Vertex4, out vector);
        }

        public static bool LineIntersection(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start, Vector3 a_Line2End, out Vector3 a_IntersectionPoint)
        {
            bool flag;
            return LineIntersection(a_Line1Start, a_Line1End, a_Line2Start, a_Line2End, out a_IntersectionPoint, out flag);
        }

        public static bool LineIntersection(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start, Vector3 a_Line2End, out Vector3 a_IntersectionPoint, out bool a_IsUnique)
        {
            bool flag = false;
            a_IntersectionPoint = Vector3.zero;
            a_IsUnique = false;
            Vector3 lhs = a_Line1End - a_Line1Start;
            Vector3 rhs = a_Line2End - a_Line2Start;
            Vector3 vector3 = a_Line2Start - a_Line1Start;
            Vector3 vector4 = Vector3.Cross(lhs, rhs);
            if (Mathf.Approximately(Vector3.Dot(vector3, vector4), 0f))
            {
                float sqrMagnitude = vector4.sqrMagnitude;
                if (!Mathf.Approximately(sqrMagnitude, 0f))
                {
                    float num5 = Vector3.Dot(Vector3.Cross(vector3, rhs), vector4) / sqrMagnitude;
                    if ((0f <= num5) && (num5 <= 1f))
                    {
                        flag = true;
                        a_IntersectionPoint = a_Line1Start + (num5 * lhs);
                        a_IsUnique = true;
                    }
                }
                else if (Vector3Extension.Approximately(lhs, Vector3.zero))
                {
                    if (IsPointOnLine(a_Line1Start, a_Line2Start, a_Line2End))
                    {
                        flag = true;
                        a_IsUnique = true;
                        a_IntersectionPoint = a_Line1Start;
                    }
                }
                else if (Vector3Extension.Approximately(rhs, Vector3.zero))
                {
                    if (IsPointOnLine(a_Line2Start, a_Line1Start, a_Line1End))
                    {
                        flag = true;
                        a_IsUnique = true;
                        a_IntersectionPoint = a_Line2Start;
                    }
                }
                else
                {
                    float num3 = FactorOfPointOnLine(a_Line2Start, a_Line1Start, a_Line1End);
                    float num4 = FactorOfPointOnLine(a_Line2End, a_Line1Start, a_Line1End);
                    if ((0f <= num3) && (num3 <= 1f))
                    {
                        flag = true;
                        a_IntersectionPoint = a_Line2Start;
                    }
                    else if ((0f <= num4) && (num4 <= 1f))
                    {
                        flag = true;
                        a_IntersectionPoint = a_Line2End;
                    }
                    else if (((num3 < 0f) && (num4 > 1f)) || ((num4 < 0f) && (num3 > 1f)))
                    {
                        flag = true;
                        a_IntersectionPoint = a_Line1Start;
                    }
                }
            }
            return flag;
        }

        public static Vector3 TriangleNormal(Vector3 a_Vertex1, Vector3 a_Vertex2, Vector3 a_Vertex3)
        {
            Vector3 lhs = a_Vertex2 - a_Vertex1;
            Vector3 rhs = a_Vertex3 - a_Vertex2;
            lhs.Normalize();
            rhs.Normalize();
            Vector3 vector3 = Vector3.Cross(lhs, rhs);
            vector3.Normalize();
            return vector3;
        }
    }
}

