namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GenericDecalsMeshCutter<D, P, DM> where D: GenericDecals<D, P, DM> where P: GenericDecalProjector<D, P, DM> where DM: GenericDecalsMesh<D, P, DM>
    {
        internal DM m_DecalsMesh;
        internal P m_ActiveProjector;
        internal List<RelativeVertexLocation> m_RelativeVertexLocations;
        internal List<int> m_ObsoleteTriangleIndices;
        internal CutEdges m_CutEdges;
        internal RemovedIndices m_RemovedIndices;
        internal CutEdgeDelegate m_GetCutEdgeDelegate;
        internal CutEdgeDelegate m_CreateCutEdgeDelegate;

        protected GenericDecalsMeshCutter()
        {
            this.m_RelativeVertexLocations = new List<RelativeVertexLocation>();
            this.m_ObsoleteTriangleIndices = new List<int>();
            this.m_CutEdges = new CutEdges();
            this.m_RemovedIndices = new RemovedIndices();
        }

        public void CutDecalsPlanes(DM a_DecalsMesh)
        {
            if (a_DecalsMesh == null)
            {
                throw new ArgumentNullException("The decals mesh argument is null!");
            }
            if (a_DecalsMesh.ActiveDecalProjector == null)
            {
                throw new NullReferenceException("The active decal projector of the decals mesh is null!");
            }
            this.m_DecalsMesh = a_DecalsMesh;
            this.m_ActiveProjector = a_DecalsMesh.ActiveDecalProjector;
            this.InitializeDelegates();
            Matrix4x4 matrixx = a_DecalsMesh.Decals.CachedTransform.worldToLocalMatrix * this.m_ActiveProjector.ProjectorToWorldMatrix;
            Matrix4x4 transpose = matrixx.inverse.transpose;
            Plane plane = new Plane(transpose.MultiplyVector(new Vector3(1f, 0f, 0f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(0.5f, 0f, 0f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
            plane = new Plane(transpose.MultiplyVector(new Vector3(-1f, 0f, 0f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(-0.5f, 0f, 0f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
            plane = new Plane(transpose.MultiplyVector(new Vector3(0f, 0f, 1f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(0f, 0f, 0.5f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
            plane = new Plane(transpose.MultiplyVector(new Vector3(0f, 0f, -1f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(0f, 0f, -0.5f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
            plane = new Plane(transpose.MultiplyVector(new Vector3(0f, 1f, 0f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(0f, 0f, 0f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
            plane = new Plane(transpose.MultiplyVector(new Vector3(0f, -1f, 0f)).normalized, -matrixx.MultiplyPoint3x4(new Vector3(0f, -1f, 0f)));
            this.PlaneDecalsMeshCutter(a_DecalsMesh, plane);
        }

        internal int CutEdge(int a_RelativeVertexLocationsOffset, int a_IndexA, int a_IndexB, Vector3 a_VertexA, Vector3 a_VertexB, bool a_IsVertexAInside, float a_IntersectionFactorAB)
        {
            Edelweiss.DecalSystem.CutEdge edge = new Edelweiss.DecalSystem.CutEdge(a_IndexA, a_IndexB);
            return (!this.m_CutEdges.HasEdge(edge) ? this.m_CreateCutEdgeDelegate(a_RelativeVertexLocationsOffset, a_IndexA, a_IndexB, a_VertexA, a_VertexB, a_IsVertexAInside, a_IntersectionFactorAB) : this.m_CutEdges[edge].ModifiedIndex);
        }

        internal abstract void InitializeDelegates();
        private bool Intersect(RelativeVertexLocation a_VertexLocation1, RelativeVertexLocation a_VertexLocation2) => 
            ((a_VertexLocation1 != RelativeVertexLocation.OnPlane) && (a_VertexLocation2 != RelativeVertexLocation.OnPlane)) && (a_VertexLocation1 != a_VertexLocation2);

        private float IntersectionFactor(DecalRay a_Ray, Plane a_Plane) => 
            (a_Plane.distance - Vector3.Dot(a_Plane.normal, a_Ray.origin)) / Vector3.Dot(a_Plane.normal, a_Ray.direction);

        internal void PlaneDecalsMeshCutter(DM a_DecalsMesh, Plane a_Plane)
        {
            this.m_RelativeVertexLocations.Clear();
            this.m_ObsoleteTriangleIndices.Clear();
            this.m_CutEdges.Clear();
            this.m_RemovedIndices.Clear();
            int decalsMeshLowerVertexIndex = this.m_ActiveProjector.DecalsMeshLowerVertexIndex;
            int decalsMeshUpperVertexIndex = this.m_ActiveProjector.DecalsMeshUpperVertexIndex;
            int num3 = decalsMeshLowerVertexIndex;
            Vector3 vector = this.PlaneOrigin(a_Plane);
            Vector3 normal = a_Plane.normal;
            for (int i = decalsMeshLowerVertexIndex; i <= decalsMeshUpperVertexIndex; i++)
            {
                Vector3 vector3 = a_DecalsMesh.Vertices[i];
                RelativeVertexLocation item = this.VertexLocationRelativeToPlane(vector, normal, vector3);
                this.m_RelativeVertexLocations.Add(item);
            }
            int count = a_DecalsMesh.Triangles.Count;
            int decalsMeshLowerTriangleIndex = this.m_ActiveProjector.DecalsMeshLowerTriangleIndex;
            for (int j = decalsMeshLowerTriangleIndex; j < count; j += 3)
            {
                int num8 = a_DecalsMesh.Triangles[j];
                int num9 = a_DecalsMesh.Triangles[j + 1];
                int num10 = a_DecalsMesh.Triangles[j + 2];
                RelativeVertexLocation location2 = this.m_RelativeVertexLocations[num8 - num3];
                RelativeVertexLocation location3 = this.m_RelativeVertexLocations[num9 - num3];
                RelativeVertexLocation location4 = this.m_RelativeVertexLocations[num10 - num3];
                if ((location2 != RelativeVertexLocation.Inside) && ((location3 != RelativeVertexLocation.Inside) && (location4 != RelativeVertexLocation.Inside)))
                {
                    this.m_ObsoleteTriangleIndices.Add(j);
                }
                else
                {
                    Vector3 vector4 = a_DecalsMesh.Vertices[num8];
                    Vector3 vector5 = a_DecalsMesh.Vertices[num9];
                    Vector3 vector6 = a_DecalsMesh.Vertices[num10];
                    bool flag = this.Intersect(location2, location3);
                    bool flag2 = this.Intersect(location2, location4);
                    bool flag3 = this.Intersect(location3, location4);
                    int item = 0;
                    int num12 = 0;
                    int num13 = 0;
                    if (flag)
                    {
                        DecalRay ray = new DecalRay(vector4, vector5 - vector4);
                        float num14 = this.IntersectionFactor(ray, a_Plane);
                        item = this.m_GetCutEdgeDelegate(num3, num8, num9, vector4, vector5, location2 == RelativeVertexLocation.Inside, num14);
                    }
                    if (flag2)
                    {
                        DecalRay ray2 = new DecalRay(vector4, vector6 - vector4);
                        float num15 = this.IntersectionFactor(ray2, a_Plane);
                        num12 = this.m_GetCutEdgeDelegate(num3, num8, num10, vector4, vector6, location2 == RelativeVertexLocation.Inside, num15);
                    }
                    if (flag3)
                    {
                        DecalRay ray3 = new DecalRay(vector5, vector6 - vector5);
                        float num16 = this.IntersectionFactor(ray3, a_Plane);
                        num13 = this.m_GetCutEdgeDelegate(num3, num9, num10, vector5, vector6, location3 == RelativeVertexLocation.Inside, num16);
                    }
                    if (flag && flag2)
                    {
                        if (location2 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j + 1] = item;
                            a_DecalsMesh.Triangles[j + 2] = num12;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j] = item;
                            a_DecalsMesh.Triangles.Add(num10);
                            a_DecalsMesh.Triangles.Add(num12);
                            a_DecalsMesh.Triangles.Add(item);
                        }
                    }
                    else if (flag && flag3)
                    {
                        if (location3 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j] = item;
                            a_DecalsMesh.Triangles[j + 2] = num13;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j + 1] = item;
                            a_DecalsMesh.Triangles.Add(num10);
                            a_DecalsMesh.Triangles.Add(item);
                            a_DecalsMesh.Triangles.Add(num13);
                        }
                    }
                    else if (flag2 && flag3)
                    {
                        if (location4 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j] = num12;
                            a_DecalsMesh.Triangles[j + 1] = num13;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j + 2] = num12;
                            a_DecalsMesh.Triangles.Add(num9);
                            a_DecalsMesh.Triangles.Add(num13);
                            a_DecalsMesh.Triangles.Add(num12);
                        }
                    }
                    else if (flag && (location4 == RelativeVertexLocation.OnPlane))
                    {
                        if (location2 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j + 1] = item;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j] = item;
                        }
                    }
                    else if (flag2 && (location3 == RelativeVertexLocation.OnPlane))
                    {
                        if (location2 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j + 2] = num12;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j] = num12;
                        }
                    }
                    else if (flag3 && (location2 == RelativeVertexLocation.OnPlane))
                    {
                        if (location3 == RelativeVertexLocation.Inside)
                        {
                            a_DecalsMesh.Triangles[j + 2] = num13;
                        }
                        else
                        {
                            a_DecalsMesh.Triangles[j + 1] = num13;
                        }
                    }
                }
            }
            int num17 = this.m_ObsoleteTriangleIndices.Count - 1;
            while (num17 >= 0)
            {
                int num18 = this.m_ObsoleteTriangleIndices[num17];
                int num19 = 1;
                while (true)
                {
                    if ((num17 <= 0) || ((num18 - 3) != this.m_ObsoleteTriangleIndices[num17 - 1]))
                    {
                        a_DecalsMesh.RemoveTrianglesAt(num18, num19);
                        num17--;
                        break;
                    }
                    num18 -= 3;
                    num19++;
                    num17--;
                }
            }
            for (int k = 0; k < this.m_RelativeVertexLocations.Count; k++)
            {
                if (((RelativeVertexLocation) this.m_RelativeVertexLocations[k]) == RelativeVertexLocation.Outside)
                {
                    this.m_RemovedIndices.AddRemovedIndex(k + num3);
                }
            }
            a_DecalsMesh.RemoveAndAdjustIndices(decalsMeshLowerTriangleIndex, this.m_RemovedIndices);
            this.m_ActiveProjector.IsUV1ProjectionCalculated = false;
            this.m_ActiveProjector.IsUV2ProjectionCalculated = false;
            this.m_ActiveProjector.IsTangentProjectionCalculated = false;
        }

        private Vector3 PlaneOrigin(Plane a_Plane) => 
            (Vector3) (a_Plane.distance * a_Plane.normal);

        private RelativeVertexLocation VertexLocationRelativeToPlane(Vector3 a_PlaneOrigin, Vector3 a_PlaneNormal, Vector3 a_Vertex)
        {
            float a = Vector3.Dot(a_Vertex - a_PlaneOrigin, a_PlaneNormal);
            return (!Mathf.Approximately(a, 0f) ? ((a >= 0f) ? RelativeVertexLocation.Outside : RelativeVertexLocation.Inside) : RelativeVertexLocation.OnPlane);
        }
    }
}

