namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DecalsMeshMinimizer
    {
        internal static float s_CurrentMaximumAbsoluteError = 0.0001f;
        internal static float s_CurrentMaximumRelativeError = 0.0001f;
        private OptimizeEdges m_RedundantExternalEdges = new OptimizeEdges();
        private OptimizeEdges m_RedundantInternalEdges = new OptimizeEdges();
        private OptimizeEdges m_OverusedInternalEdges = new OptimizeEdges();
        private SortedDictionary<int, int> m_NonInternalVertexIndices = new SortedDictionary<int, int>();
        private List<int> m_ObsoleteInternalVertexIndices = new List<int>();
        private List<int> m_ObsoleteExternalVertexIndices = new List<int>();
        private ObsoleteVertexFinder m_ObsoleteVertexFinder = new ObsoleteVertexFinder();
        private ObsoleteVertexRemover m_ObsoleteVertexRemover = new ObsoleteVertexRemover();
        private RemovedIndices m_RemovedIndices = new RemovedIndices();

        private void AddEdge(OptimizeEdge a_Edge)
        {
            if (!this.m_OverusedInternalEdges.HasEdge(a_Edge))
            {
                if (this.m_RedundantInternalEdges.HasEdge(a_Edge))
                {
                    this.m_RedundantInternalEdges.RemoveEdge(a_Edge);
                    this.m_OverusedInternalEdges.AddEdge(a_Edge);
                }
                else if (!this.m_RedundantExternalEdges.HasEdge(a_Edge))
                {
                    this.m_RedundantExternalEdges.AddEdge(a_Edge);
                }
                else
                {
                    OptimizeEdge edge = this.m_RedundantExternalEdges[a_Edge];
                    this.m_RedundantExternalEdges.RemoveEdge(a_Edge);
                    edge.triangle2Index = a_Edge.triangle1Index;
                    this.m_RedundantInternalEdges.AddEdge(edge);
                }
            }
        }

        private static bool AreEdgesParallelOrIsAtLeastOneAPoint(DecalsMesh a_DecalsMesh, OptimizeEdge a_Edge1, OptimizeEdge a_Edge2)
        {
            bool flag = false;
            Vector3 vector5 = a_DecalsMesh.Vertices[a_Edge1.vertex2Index] - a_DecalsMesh.Vertices[a_Edge1.vertex1Index];
            Vector3 vector6 = a_DecalsMesh.Vertices[a_Edge2.vertex2Index] - a_DecalsMesh.Vertices[a_Edge2.vertex1Index];
            vector5.Normalize();
            vector6.Normalize();
            if (Vector3Extension.Approximately(vector5, Vector3.zero, s_CurrentMaximumAbsoluteError, s_CurrentMaximumRelativeError) || Vector3Extension.Approximately(vector6, Vector3.zero, s_CurrentMaximumAbsoluteError, s_CurrentMaximumRelativeError))
            {
                flag = true;
            }
            else if (MathfExtension.Approximately(Mathf.Abs(Vector3.Dot(vector5, vector6)), 1f, s_CurrentMaximumAbsoluteError, s_CurrentMaximumRelativeError))
            {
                flag = true;
            }
            return flag;
        }

        private void ClearAll()
        {
            this.m_RedundantInternalEdges.Clear();
            this.m_ObsoleteInternalVertexIndices.Clear();
            this.m_ObsoleteExternalVertexIndices.Clear();
            this.m_RemovedIndices.Clear();
            this.ClearTemporaryCollections();
        }

        private void ClearTemporaryCollections()
        {
            this.m_RedundantExternalEdges.Clear();
            this.m_OverusedInternalEdges.Clear();
            this.m_NonInternalVertexIndices.Clear();
        }

        private void ComputeObsoleteExternalVertices(DecalsMesh a_DecalsMesh)
        {
            for (int i = this.m_ObsoleteExternalVertexIndices.Count - 1; i >= 0; i--)
            {
                int num2 = this.m_ObsoleteExternalVertexIndices[i];
                if (!this.m_ObsoleteVertexFinder.IsExternalVertexObsolete(a_DecalsMesh, num2))
                {
                    this.m_ObsoleteExternalVertexIndices.RemoveAt(i);
                }
            }
        }

        private void ComputeObsoleteInternalVertices(DecalsMesh a_DecalsMesh)
        {
            List<OptimizeEdge> list = this.m_RedundantInternalEdges.OptimizedEdgeList();
            for (int i = this.m_ObsoleteInternalVertexIndices.Count - 1; i >= 0; i--)
            {
                int num2 = this.m_ObsoleteInternalVertexIndices[i];
                if (!this.m_ObsoleteVertexFinder.IsInternalVertexObsolete(a_DecalsMesh, num2, list))
                {
                    this.m_ObsoleteInternalVertexIndices.RemoveAt(i);
                }
            }
        }

        private void ComputePotentialObsoleteVertices(DecalsMesh a_DecalsMesh)
        {
            DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
            this.ClearAll();
            for (int i = activeDecalProjector.DecalsMeshLowerTriangleIndex; i <= activeDecalProjector.DecalsMeshUpperTriangleIndex; i += 3)
            {
                int num2 = a_DecalsMesh.Triangles[i];
                int num3 = a_DecalsMesh.Triangles[i + 1];
                int num4 = a_DecalsMesh.Triangles[i + 2];
                OptimizeEdge edge = new OptimizeEdge(num2, num3, i);
                OptimizeEdge edge2 = new OptimizeEdge(num3, num4, i);
                OptimizeEdge edge3 = new OptimizeEdge(num4, num2, i);
                this.AddEdge(edge);
                this.AddEdge(edge2);
                this.AddEdge(edge3);
            }
            List<OptimizeEdge> list = this.m_RedundantExternalEdges.OptimizedEdgeList();
            foreach (OptimizeEdge edge4 in list)
            {
                if (!this.m_NonInternalVertexIndices.ContainsKey(edge4.vertex1Index))
                {
                    this.m_NonInternalVertexIndices.Add(edge4.vertex1Index, edge4.vertex1Index);
                }
                if (!this.m_NonInternalVertexIndices.ContainsKey(edge4.vertex2Index))
                {
                    this.m_NonInternalVertexIndices.Add(edge4.vertex2Index, edge4.vertex2Index);
                }
            }
            foreach (OptimizeEdge edge5 in this.m_OverusedInternalEdges.OptimizedEdgeList())
            {
                if (!this.m_NonInternalVertexIndices.ContainsKey(edge5.vertex1Index))
                {
                    this.m_NonInternalVertexIndices.Add(edge5.vertex1Index, edge5.vertex1Index);
                }
                if (!this.m_NonInternalVertexIndices.ContainsKey(edge5.vertex2Index))
                {
                    this.m_NonInternalVertexIndices.Add(edge5.vertex2Index, edge5.vertex2Index);
                }
            }
            for (int j = activeDecalProjector.DecalsMeshLowerVertexIndex; j <= activeDecalProjector.DecalsMeshUpperVertexIndex; j++)
            {
                if (!this.m_NonInternalVertexIndices.ContainsKey(j))
                {
                    this.m_ObsoleteInternalVertexIndices.Add(j);
                }
            }
            int num6 = 0;
            while (num6 < list.Count)
            {
                OptimizeEdge edge6 = list[num6];
                int num7 = num6 + 1;
                while (true)
                {
                    if (num7 >= this.m_RedundantExternalEdges.Count)
                    {
                        num6++;
                        break;
                    }
                    OptimizeEdge edge7 = list[num7];
                    if ((edge6.vertex1Index != edge7.vertex1Index) && (edge6.vertex1Index != edge7.vertex2Index))
                    {
                        if ((((edge6.vertex2Index == edge7.vertex1Index) || (edge6.vertex2Index == edge7.vertex2Index)) && !this.m_ObsoleteExternalVertexIndices.Contains(edge6.vertex2Index)) && AreEdgesParallelOrIsAtLeastOneAPoint(a_DecalsMesh, edge6, edge7))
                        {
                            this.m_ObsoleteExternalVertexIndices.Add(edge6.vertex2Index);
                        }
                    }
                    else if (!this.m_ObsoleteExternalVertexIndices.Contains(edge6.vertex1Index) && AreEdgesParallelOrIsAtLeastOneAPoint(a_DecalsMesh, edge6, edge7))
                    {
                        this.m_ObsoleteExternalVertexIndices.Add(edge6.vertex1Index);
                    }
                    num7++;
                }
            }
            this.ClearTemporaryCollections();
        }

        public void MinimizeActiveProjectorOfDecalsMesh(DecalsMesh a_DecalsMesh)
        {
            if (a_DecalsMesh == null)
            {
                throw new ArgumentNullException("Decals mesh argument is not allowed to be null.");
            }
            float meshMinimizerMaximumAbsoluteError = a_DecalsMesh.Decals.MeshMinimizerMaximumAbsoluteError;
            float meshMinimizerMaximumRelativeError = a_DecalsMesh.Decals.MeshMinimizerMaximumRelativeError;
            try
            {
                this.MinimizeActiveProjectorOfDecalsMesh(a_DecalsMesh, meshMinimizerMaximumAbsoluteError, meshMinimizerMaximumRelativeError);
            }
            catch
            {
                throw;
            }
        }

        public void MinimizeActiveProjectorOfDecalsMesh(DecalsMesh a_DecalsMesh, float a_MaximumAbsoluteError, float a_MaximumRelativeError)
        {
            if (a_DecalsMesh == null)
            {
                throw new ArgumentNullException("Decals mesh argument is not allowed to be null.");
            }
            if (a_DecalsMesh.ActiveDecalProjector == null)
            {
                throw new ArgumentNullException("Active decal projector of decals mesh is not allowed to be null.");
            }
            if (a_MaximumAbsoluteError < 0f)
            {
                throw new ArgumentOutOfRangeException("The maximum absolute error has to be greater than zero.");
            }
            if ((a_MaximumRelativeError < 0f) || (a_MaximumRelativeError > 1f))
            {
                throw new ArgumentOutOfRangeException("The maximum relative error has to be within [0.0f, 1.0f].");
            }
            this.ClearAll();
            s_CurrentMaximumAbsoluteError = a_MaximumAbsoluteError;
            s_CurrentMaximumRelativeError = a_MaximumRelativeError;
            this.ComputePotentialObsoleteVertices(a_DecalsMesh);
            this.ComputeObsoleteInternalVertices(a_DecalsMesh);
            this.ComputeObsoleteExternalVertices(a_DecalsMesh);
            this.RemoveObsoleteVertices(a_DecalsMesh);
            this.ClearAll();
        }

        private void RemoveObsoleteVertices(DecalsMesh a_DecalsMesh)
        {
            DecalProjectorBase activeDecalProjector = a_DecalsMesh.ActiveDecalProjector;
            this.m_ObsoleteInternalVertexIndices.Sort();
            List<OptimizeEdge> list = this.m_RedundantInternalEdges.OptimizedEdgeList();
            for (int i = this.m_ObsoleteInternalVertexIndices.Count - 1; i >= 0; i--)
            {
                bool flag;
                int num2 = this.m_ObsoleteInternalVertexIndices[i];
                this.m_ObsoleteVertexRemover.RemoveObsoleteInternalVertex(a_DecalsMesh, num2, list, out flag);
                if (!flag)
                {
                    this.m_ObsoleteInternalVertexIndices.RemoveAt(i);
                }
            }
            for (int j = this.m_ObsoleteExternalVertexIndices.Count - 1; j >= 0; j--)
            {
                bool flag2;
                int num4 = this.m_ObsoleteExternalVertexIndices[j];
                this.m_ObsoleteVertexRemover.RemoveObsoleteExternalVertex(a_DecalsMesh, num4, out flag2);
                if (!flag2)
                {
                    this.m_ObsoleteExternalVertexIndices.RemoveAt(j);
                }
            }
            this.m_ObsoleteInternalVertexIndices.AddRange(this.m_ObsoleteExternalVertexIndices);
            this.m_ObsoleteInternalVertexIndices.Sort();
            foreach (int num5 in this.m_ObsoleteInternalVertexIndices)
            {
                this.m_RemovedIndices.AddRemovedIndex(num5);
            }
            a_DecalsMesh.RemoveAndAdjustIndices(activeDecalProjector.DecalsMeshLowerTriangleIndex, this.m_RemovedIndices);
            activeDecalProjector.IsUV1ProjectionCalculated = false;
            activeDecalProjector.IsUV2ProjectionCalculated = false;
            activeDecalProjector.IsTangentProjectionCalculated = false;
        }
    }
}

