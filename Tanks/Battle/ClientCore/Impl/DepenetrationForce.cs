namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class DepenetrationForce
    {
        public static float ABSORTION_KOEF = 1f;
        public static float VISUAL_DELTA = 5f;
        public static float DEP_VELOCITY = 2f;
        public static float VERT_DEP_VELOCITY = 0f;
        private static Vector3 forcePoint = Vector3.zero;
        private static Vector3 forceDir = Vector3.zero;
        private static Vector3 vertforceDir = Vector3.zero;
        private static List<Edge> edges1 = new List<Edge>();
        private static List<Edge> edges2 = new List<Edge>();
        private static List<Vector2> contacts = new List<Vector2>();
        private static float largestSectionSq = 0f;
        private static Vector2 SectionFrom = Vector2.zero;
        private static Vector2 SectionTo = Vector2.zero;

        public static bool ApplyDepenetrationForce(Rigidbody body1, BoxCollider collider1, Rigidbody body2, BoxCollider collider2)
        {
            if (!CalculateForcePointAndDir(body1, collider1, body2, collider2))
            {
                return false;
            }
            float num = body2.mass / body1.mass;
            Vector3 zero = Vector3.zero;
            Vector3 vector2 = Vector3.zero;
            Vector3 lhs = Vector3.Project(-body1.GetPointVelocity(forcePoint), forceDir.normalized);
            if (Vector3.Dot(lhs, forceDir) > 0f)
            {
                zero = ((lhs * 0.5f) * ABSORTION_KOEF) * num;
                vector2 = ((-lhs * 0.5f) * ABSORTION_KOEF) / num;
            }
            float magnitude = lhs.magnitude;
            float num3 = num * DEP_VELOCITY;
            if (magnitude < num3)
            {
                zero += forceDir * (num3 - magnitude);
            }
            body1.AddForceAtPositionSafe((zero * body1.mass) / Time.fixedDeltaTime, forcePoint);
            body2.AddForceAtPositionSafe((vector2 * body2.mass) / Time.fixedDeltaTime, forcePoint);
            lhs = Vector3.Project(-body1.GetPointVelocity(forcePoint), vertforceDir.normalized);
            if (Vector3.Dot(lhs, vertforceDir) > 0f)
            {
                zero = (lhs * 0.5f) * 0.1f;
                body1.AddForceAtPositionSafe((zero * body1.mass) / Time.fixedDeltaTime, forcePoint);
            }
            else
            {
                magnitude = lhs.magnitude;
                if (magnitude < VERT_DEP_VELOCITY)
                {
                    zero = vertforceDir * (VERT_DEP_VELOCITY - magnitude);
                    body1.AddForceAtPositionSafe((zero * body1.mass) / Time.fixedDeltaTime, forcePoint);
                }
            }
            return true;
        }

        private static bool CalculateForcePointAndDir(Rigidbody body1, BoxCollider collider1, Rigidbody body2, BoxCollider collider2)
        {
            edges1.Clear();
            edges2.Clear();
            CollectBoxColliderEdges(collider1, edges1);
            CollectBoxColliderEdges(collider2, edges2);
            if (!FindSectionXZSpace())
            {
                return false;
            }
            Vector3 vector = new Vector3(SectionFrom.x, VISUAL_DELTA, SectionFrom.y);
            Vector3 vector2 = new Vector3(SectionTo.x, VISUAL_DELTA, SectionTo.y);
            forceDir = Vector3.Cross((vector2 - vector).normalized, Vector3.up);
            forcePoint = (vector2 + vector) * 0.5f;
            if (!FindSectionXYSpace())
            {
                return false;
            }
            if (!FindSectionZYSpace())
            {
                return false;
            }
            if (Vector3.Dot((forcePoint - body1.position).normalized, forceDir) > 0f)
            {
                forceDir = -forceDir;
            }
            forcePoint.y = (SectionFrom.y + SectionTo.y) * 0.5f;
            Vector3 normalized = (forcePoint - body1.position).normalized;
            vector = new Vector3(0f, SectionFrom.y, SectionFrom.x);
            vector2 = new Vector3(0f, SectionTo.y, SectionTo.x);
            vertforceDir = Vector3.Cross((vector2 - vector).normalized, Vector3.left);
            if (Vector3.Dot(normalized, vertforceDir) > 0f)
            {
                vertforceDir = -vertforceDir;
            }
            return true;
        }

        public static unsafe void CollectBoxColliderEdges(BoxCollider collider, List<Edge> edges)
        {
            Vector3 vector = (collider.size * 0.5f) * 0.9f;
            Vector3* vectorPtr1 = &vector;
            vectorPtr1->y *= 0.7f;
            Vector3 vector2 = collider.center - vector;
            Vector3 vector3 = collider.center + vector;
            Transform t = collider.transform;
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector3.y, vector2.z), TransfromPoint(t, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector3.y, vector2.z), TransfromPoint(t, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector3.y, vector3.z), TransfromPoint(t, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector3.y, vector3.z), TransfromPoint(t, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector2.y, vector2.z), TransfromPoint(t, vector2.x, vector2.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector2.y, vector2.z), TransfromPoint(t, vector3.x, vector2.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector2.y, vector3.z), TransfromPoint(t, vector3.x, vector2.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector2.y, vector3.z), TransfromPoint(t, vector2.x, vector2.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector2.y, vector2.z), TransfromPoint(t, vector2.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector2.x, vector2.y, vector3.z), TransfromPoint(t, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector2.y, vector2.z), TransfromPoint(t, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(t, vector3.x, vector2.y, vector3.z), TransfromPoint(t, vector3.x, vector3.y, vector3.z)));
        }

        public static float Cross2D(Vector2 v1, Vector2 v2) => 
            (v1.x * v2.y) - (v1.y * v2.x);

        private static void Find2DSection()
        {
            contacts.Clear();
            foreach (Edge edge in edges1)
            {
                foreach (Edge edge2 in edges2)
                {
                    Vector2 vector;
                    if (LineSegementsIntersect(edge.from2D, edge.to2D, edge2.from2D, edge2.to2D, out vector, false))
                    {
                        contacts.Add(vector);
                    }
                }
            }
            largestSectionSq = 0f;
            SectionFrom = Vector2.zero;
            SectionTo = Vector2.zero;
            foreach (Vector2 vector2 in contacts)
            {
                foreach (Vector2 vector3 in contacts)
                {
                    if (vector2 != vector3)
                    {
                        float sqrMagnitude = (vector2 - vector3).sqrMagnitude;
                        if (sqrMagnitude > largestSectionSq)
                        {
                            largestSectionSq = sqrMagnitude;
                            SectionFrom = vector2;
                            SectionTo = vector3;
                        }
                    }
                }
            }
        }

        private static bool FindSectionXYSpace()
        {
            foreach (Edge edge in edges1)
            {
                edge.ToXYSpace();
            }
            foreach (Edge edge2 in edges2)
            {
                edge2.ToXYSpace();
            }
            Find2DSection();
            return (largestSectionSq > 0f);
        }

        private static bool FindSectionXZSpace()
        {
            Find2DSection();
            return (largestSectionSq > 0f);
        }

        private static bool FindSectionZYSpace()
        {
            foreach (Edge edge in edges1)
            {
                edge.ToZYSpace();
            }
            foreach (Edge edge2 in edges2)
            {
                edge2.ToZYSpace();
            }
            Find2DSection();
            return (largestSectionSq > 0f);
        }

        public static bool LineSegementsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Vector2();
            Vector2 vector = p2 - p;
            Vector2 vector2 = q2 - q;
            float a = Cross2D(vector, vector2);
            float num2 = Cross2D(q - p, vector);
            if (Mathf.Approximately(a, 0f) && Mathf.Approximately(num2, 0f))
            {
                return (considerCollinearOverlapAsIntersect && (((0f > Vector2.Dot(q - p, vector)) || (Vector2.Dot(q - p, vector) > Vector2.Dot(vector, vector))) ? ((0f <= Vector2.Dot(p - q, vector2)) && (Vector2.Dot(p - q, vector2) <= Vector2.Dot(vector2, vector2))) : true));
            }
            if (Mathf.Approximately(a, 0f) && !Mathf.Approximately(num2, 0f))
            {
                return false;
            }
            float num3 = Cross2D(q - p, vector2) / a;
            float num4 = Cross2D(q - p, vector) / a;
            if (Mathf.Approximately(a, 0f) || ((0f > num3) || ((num3 > 1f) || ((0f > num4) || (num4 > 1f)))))
            {
                return false;
            }
            intersection = p + (num3 * vector);
            return true;
        }

        public static Vector3 TransfromPoint(Transform t, float x, float y, float z) => 
            t.TransformPoint(new Vector3(x, y, z));

        public class Edge
        {
            public Vector3 from3D;
            public Vector3 to3D;
            public Vector2 from2D;
            public Vector2 to2D;

            public Edge(Vector3 from, Vector3 to)
            {
                this.from3D = from;
                this.to3D = to;
                this.ToXZSpace();
            }

            public void ToXYSpace()
            {
                this.from2D = new Vector2(this.from3D.x, this.from3D.y);
                this.to2D = new Vector2(this.to3D.x, this.to3D.y);
            }

            public void ToXZSpace()
            {
                this.from2D = new Vector2(this.from3D.x, this.from3D.z);
                this.to2D = new Vector2(this.to3D.x, this.to3D.z);
            }

            public void ToZYSpace()
            {
                this.from2D = new Vector2(this.from3D.z, this.from3D.y);
                this.to2D = new Vector2(this.to3D.z, this.to3D.y);
            }
        }
    }
}

