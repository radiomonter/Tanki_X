namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class ObsoleteVertexFinder
    {
        private OptimizeTriangleNormals m_TriangleNormals = new OptimizeTriangleNormals();
        private List<int> m_NeighboringTriangles = new List<int>();
        private List<int> m_NeighboringVertexIndices = new List<int>();
        private List<float> m_NeighboringVertexDistances = new List<float>();
        private List<float> m_NeighboringVertexWeight = new List<float>();

        private void AddNeighborTriangleIfNeeded(DecalsMesh a_DecalsMesh, int a_VertexIndex, int a_TriangleIndex)
        {
            int num = a_DecalsMesh.Triangles[a_TriangleIndex];
            int num2 = a_DecalsMesh.Triangles[a_TriangleIndex + 1];
            int num3 = a_DecalsMesh.Triangles[a_TriangleIndex + 2];
            int item = 0;
            int num5 = 0;
            bool flag = true;
            if (a_VertexIndex == num)
            {
                item = num2;
                num5 = num3;
            }
            else if (a_VertexIndex == num2)
            {
                item = num;
                num5 = num3;
            }
            else if (a_VertexIndex != num3)
            {
                flag = false;
            }
            else
            {
                item = num;
                num5 = num2;
            }
            if (flag && !this.m_NeighboringTriangles.Contains(a_TriangleIndex))
            {
                this.m_NeighboringTriangles.Add(a_TriangleIndex);
                if (!this.m_NeighboringVertexIndices.Contains(item))
                {
                    float num6 = Vector3.Distance(a_DecalsMesh.Vertices[a_VertexIndex], a_DecalsMesh.Vertices[item]);
                    this.m_NeighboringVertexIndices.Add(item);
                    this.m_NeighboringVertexDistances.Add(num6);
                }
                if (!this.m_NeighboringVertexIndices.Contains(num5))
                {
                    float num7 = Vector3.Distance(a_DecalsMesh.Vertices[a_VertexIndex], a_DecalsMesh.Vertices[num5]);
                    this.m_NeighboringVertexIndices.Add(num5);
                    this.m_NeighboringVertexDistances.Add(num7);
                }
                this.AddTriangleNormal(a_DecalsMesh, a_TriangleIndex);
            }
        }

        private void AddTriangleNormal(DecalsMesh a_DecalsMesh, int a_TriangleIndex)
        {
            if (!this.m_TriangleNormals.HasTriangleNormal(a_TriangleIndex))
            {
                int num = a_DecalsMesh.Triangles[a_TriangleIndex];
                int num2 = a_DecalsMesh.Triangles[a_TriangleIndex + 1];
                int num3 = a_DecalsMesh.Triangles[a_TriangleIndex + 2];
                Vector3 vector4 = GeometryUtilities.TriangleNormal(a_DecalsMesh.Vertices[num], a_DecalsMesh.Vertices[num2], a_DecalsMesh.Vertices[num3]);
                this.m_TriangleNormals.AddTriangleNormal(a_TriangleIndex, vector4);
            }
        }

        private bool AreVertexPropertiesIdentical(DecalsMesh a_DecalsMesh, int a_VertexIndex1, int a_VertexIndex2)
        {
            Decals decals = a_DecalsMesh.Decals;
            bool flag = Vector3Extension.Approximately(a_DecalsMesh.Vertices[a_VertexIndex1], a_DecalsMesh.Vertices[a_VertexIndex2], DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            if (flag && (decals.CurrentNormalsMode == NormalsMode.Target))
            {
                flag = Vector3Extension.Approximately(a_DecalsMesh.Normals[a_VertexIndex1], a_DecalsMesh.Normals[a_VertexIndex2], DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }
            if (flag && (decals.CurrentTangentsMode == TangentsMode.Target))
            {
                Vector4 vector6 = a_DecalsMesh.Tangents[a_VertexIndex2];
                flag = Vector3Extension.Approximately(a_DecalsMesh.Tangents[a_VertexIndex1], (Vector3) vector6, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }
            if (flag && ((decals.CurrentUVMode == UVMode.TargetUV) || (decals.CurrentUVMode == UVMode.TargetUV2)))
            {
                flag = Vector2Extension.Approximately(a_DecalsMesh.UVs[a_VertexIndex1], a_DecalsMesh.UVs[a_VertexIndex2], DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }
            if (flag && ((decals.CurrentUV2Mode == UV2Mode.TargetUV) || (decals.CurrentUV2Mode == UV2Mode.TargetUV2)))
            {
                flag = Vector2Extension.Approximately(a_DecalsMesh.UV2s[a_VertexIndex1], a_DecalsMesh.UV2s[a_VertexIndex2], DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
            }
            return flag;
        }

        private bool AreWeightedVertexPropertiesApproximatelyVertexProperties(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            bool flag = true;
            Decals decals = a_DecalsMesh.Decals;
            if (flag)
            {
                Vector3 vector = a_DecalsMesh.Vertices[a_VertexIndex];
                Vector3 zero = Vector3.zero;
                int num = 0;
                while (true)
                {
                    if (num >= this.m_NeighboringVertexIndices.Count)
                    {
                        flag = flag && Vector3Extension.Approximately(vector, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
                        break;
                    }
                    int num2 = this.m_NeighboringVertexIndices[num];
                    float num3 = this.m_NeighboringVertexWeight[num];
                    Vector3 vector3 = a_DecalsMesh.Vertices[num2];
                    zero += num3 * vector3;
                    num++;
                }
            }
            if (flag && (decals.CurrentNormalsMode == NormalsMode.Target))
            {
                Vector3 vector4 = a_DecalsMesh.Normals[a_VertexIndex];
                Vector3 zero = Vector3.zero;
                int num4 = 0;
                while (true)
                {
                    if (num4 >= this.m_NeighboringVertexIndices.Count)
                    {
                        flag = flag && Vector3Extension.Approximately(vector4, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
                        break;
                    }
                    int num5 = this.m_NeighboringVertexIndices[num4];
                    float num6 = this.m_NeighboringVertexWeight[num4];
                    Vector3 vector6 = a_DecalsMesh.Normals[num5];
                    (zero + (num6 * vector6)).Normalize();
                    num4++;
                }
            }
            if (flag && (decals.CurrentTangentsMode == TangentsMode.Target))
            {
                Vector4 vector7 = a_DecalsMesh.Tangents[a_VertexIndex];
                Vector4 zero = Vector3.zero;
                int num7 = 0;
                while (true)
                {
                    if (num7 >= this.m_NeighboringVertexIndices.Count)
                    {
                        flag = flag && Vector4Extension.Approximately(vector7, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
                        break;
                    }
                    int num8 = this.m_NeighboringVertexIndices[num7];
                    float num9 = this.m_NeighboringVertexWeight[num7];
                    Vector4 vector9 = a_DecalsMesh.Tangents[num8];
                    (zero + (num9 * vector9)).Normalize();
                    num7++;
                }
            }
            if (flag && ((decals.CurrentUVMode == UVMode.TargetUV) || (decals.CurrentUVMode == UVMode.TargetUV2)))
            {
                Vector2 vector10 = a_DecalsMesh.UVs[a_VertexIndex];
                Vector2 zero = Vector3.zero;
                int num10 = 0;
                while (true)
                {
                    if (num10 >= this.m_NeighboringVertexIndices.Count)
                    {
                        flag = flag && Vector2Extension.Approximately(vector10, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
                        break;
                    }
                    int num11 = this.m_NeighboringVertexIndices[num10];
                    float num12 = this.m_NeighboringVertexWeight[num10];
                    Vector2 vector12 = a_DecalsMesh.UVs[num11];
                    zero += num12 * vector12;
                    num10++;
                }
            }
            if (flag && ((decals.CurrentUV2Mode == UV2Mode.TargetUV) || (decals.CurrentUV2Mode == UV2Mode.TargetUV2)))
            {
                Vector2 vector13 = a_DecalsMesh.UV2s[a_VertexIndex];
                Vector2 zero = Vector3.zero;
                int num13 = 0;
                while (true)
                {
                    if (num13 >= this.m_NeighboringVertexIndices.Count)
                    {
                        flag = flag && Vector2Extension.Approximately(vector13, zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError);
                        break;
                    }
                    int num14 = this.m_NeighboringVertexIndices[num13];
                    float num15 = this.m_NeighboringVertexWeight[num13];
                    Vector2 vector15 = a_DecalsMesh.UV2s[num14];
                    zero += num15 * vector15;
                    num13++;
                }
            }
            return true;
        }

        public void ClearAll()
        {
            this.m_TriangleNormals.Clear();
            this.ClearNeighbors();
        }

        private void ClearNeighbors()
        {
            this.m_NeighboringTriangles.Clear();
            this.m_NeighboringVertexIndices.Clear();
            this.m_NeighboringVertexDistances.Clear();
            this.m_NeighboringVertexWeight.Clear();
        }

        private void ComputeNeighbors(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            this.ClearNeighbors();
            for (int i = a_DecalsMesh.ActiveDecalProjector.DecalsMeshLowerTriangleIndex; i < a_DecalsMesh.Triangles.Count; i += 3)
            {
                this.AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, i);
            }
        }

        private void ComputeNeighbors(DecalsMesh a_DecalsMesh, int a_VertexIndex, List<OptimizeEdge> a_RedundantInternalEdges)
        {
            this.ClearNeighbors();
            foreach (OptimizeEdge edge in a_RedundantInternalEdges)
            {
                this.AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, edge.triangle1Index);
                this.AddNeighborTriangleIfNeeded(a_DecalsMesh, a_VertexIndex, edge.triangle2Index);
            }
        }

        public bool IsExternalVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            this.ClearAll();
            this.ComputeNeighbors(a_DecalsMesh, a_VertexIndex);
            return this.IsVertexObsolete(a_DecalsMesh, a_VertexIndex);
        }

        public bool IsInternalVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex, List<OptimizeEdge> a_RedundantInternalEdges)
        {
            this.ClearAll();
            this.ComputeNeighbors(a_DecalsMesh, a_VertexIndex, a_RedundantInternalEdges);
            return this.IsVertexObsolete(a_DecalsMesh, a_VertexIndex);
        }

        private bool IsVertexObsolete(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            bool flag = false;
            if (this.m_NeighboringTriangles.Count <= 0)
            {
                return flag;
            }
            else
            {
                flag = true;
                bool flag2 = false;
                Vector3 zero = Vector3.zero;
                foreach (int num in this.m_NeighboringTriangles)
                {
                    zero = this.m_TriangleNormals[num];
                    if (zero != Vector3.zero)
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (flag2)
                {
                    foreach (int num2 in this.m_NeighboringTriangles)
                    {
                        Vector3 rhs = this.m_TriangleNormals[num2];
                        if ((rhs != Vector3.zero) && !MathfExtension.Approximately(Vector3.Dot(zero, rhs), 1f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    return flag;
                }
                else
                {
                    float num4 = 0f;
                    int num5 = 0;
                    while (true)
                    {
                        if (num5 >= this.m_NeighboringVertexIndices.Count)
                        {
                            float num7 = 0f;
                            if (!MathfExtension.Approximately(num4, 0f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError))
                            {
                                num7 = 1f / num4;
                            }
                            for (int i = 0; i < this.m_NeighboringVertexIndices.Count; i++)
                            {
                                float num9 = this.m_NeighboringVertexDistances[i];
                                if (!MathfExtension.Approximately(num9, 0f, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError))
                                {
                                    float item = 1f - (num9 / num7);
                                    this.m_NeighboringVertexWeight.Add(item);
                                }
                                else
                                {
                                    this.m_NeighboringVertexWeight.Add(0f);
                                    int num10 = this.m_NeighboringVertexIndices[i];
                                    flag = this.AreVertexPropertiesIdentical(a_DecalsMesh, a_VertexIndex, num10);
                                    if (!flag)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                        float num6 = this.m_NeighboringVertexDistances[num5];
                        num4 += num6;
                        num5++;
                    }
                }
            }
            if (flag)
            {
                flag = this.AreWeightedVertexPropertiesApproximatelyVertexProperties(a_DecalsMesh, a_VertexIndex);
            }
            return flag;
        }
    }
}

