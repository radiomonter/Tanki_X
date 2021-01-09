namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class ObsoleteVertexRemover
    {
        private Vector3 m_ReferenceTriangleNormal;
        private List<int> m_NeighboringTriangles = new List<int>();
        private List<int> m_UnusedNeighboringTriangles = new List<int>();
        private bool m_SortedNeighboringVerticesInitialized;
        private List<int> m_SortedNeighboringVertices = new List<int>();

        private void AddNewTriangles(DecalsMesh a_DecalsMesh)
        {
            while (true)
            {
                bool flag;
                while (true)
                {
                    if (this.m_SortedNeighboringVertices.Count >= 3)
                    {
                        flag = false;
                        for (int i = 0; i < this.m_SortedNeighboringVertices.Count; i++)
                        {
                            int num2 = i;
                            int index = i + 1;
                            int num4 = i + 2;
                            if (index >= this.m_SortedNeighboringVertices.Count)
                            {
                                index -= this.m_SortedNeighboringVertices.Count;
                                num4 -= this.m_SortedNeighboringVertices.Count;
                            }
                            else if (num4 >= this.m_SortedNeighboringVertices.Count)
                            {
                                num4 -= this.m_SortedNeighboringVertices.Count;
                            }
                            int item = this.m_SortedNeighboringVertices[num2];
                            int num6 = this.m_SortedNeighboringVertices[index];
                            int num7 = this.m_SortedNeighboringVertices[num4];
                            Vector3 rhs = GeometryUtilities.TriangleNormal(a_DecalsMesh.Vertices[item], a_DecalsMesh.Vertices[num6], a_DecalsMesh.Vertices[num7]);
                            if (Vector3.Dot(this.m_ReferenceTriangleNormal, rhs) >= 0f)
                            {
                                a_DecalsMesh.Triangles.Add(item);
                                a_DecalsMesh.Triangles.Add(num6);
                                a_DecalsMesh.Triangles.Add(num7);
                                this.m_SortedNeighboringVertices.RemoveAt(index);
                                flag = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                }
                if (!flag)
                {
                    return;
                }
            }
        }

        private void Clear()
        {
            this.m_NeighboringTriangles.Clear();
            this.m_SortedNeighboringVertices.Clear();
            this.m_UnusedNeighboringTriangles.Clear();
        }

        private void InitializeNeighboringTriangles(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            bool flag = false;
            for (int i = a_DecalsMesh.ActiveDecalProjector.DecalsMeshLowerTriangleIndex; i < a_DecalsMesh.Triangles.Count; i += 3)
            {
                int num2 = a_DecalsMesh.Triangles[i];
                int num3 = a_DecalsMesh.Triangles[i + 1];
                int num4 = a_DecalsMesh.Triangles[i + 2];
                if ((num2 == a_VertexIndex) || ((num3 == a_VertexIndex) || (num4 == a_VertexIndex)))
                {
                    this.m_NeighboringTriangles.Add(i);
                    if (!flag)
                    {
                        Vector3 vector = a_DecalsMesh.Vertices[num2];
                        this.m_ReferenceTriangleNormal = GeometryUtilities.TriangleNormal(vector, a_DecalsMesh.Vertices[num3], a_DecalsMesh.Vertices[num4]);
                        if (!Vector3Extension.Approximately(this.m_ReferenceTriangleNormal, Vector3.zero, DecalsMeshMinimizer.s_CurrentMaximumAbsoluteError, DecalsMeshMinimizer.s_CurrentMaximumRelativeError))
                        {
                            flag = true;
                        }
                    }
                }
            }
        }

        private void InitializeSortedNeighboringExternalVertices(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            this.m_SortedNeighboringVerticesInitialized = true;
            this.m_UnusedNeighboringTriangles.AddRange(this.m_NeighboringTriangles);
            if (this.m_UnusedNeighboringTriangles.Count > 0)
            {
                int num = this.m_UnusedNeighboringTriangles[this.m_UnusedNeighboringTriangles.Count - 1];
                int num2 = this.InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num, a_VertexIndex);
                num2 = this.SuccessorInnerTriangleVertexIndex(num2);
                this.m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + num2]);
                this.m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + this.SuccessorInnerTriangleVertexIndex(num2)]);
                this.m_UnusedNeighboringTriangles.RemoveAt(this.m_UnusedNeighboringTriangles.Count - 1);
            }
            else
            {
                this.m_SortedNeighboringVerticesInitialized = false;
                return;
            }
            while (true)
            {
                while (true)
                {
                    if (this.m_UnusedNeighboringTriangles.Count != 0)
                    {
                        bool flag = false;
                        int num3 = this.m_SortedNeighboringVertices[0];
                        int num4 = this.m_SortedNeighboringVertices[this.m_SortedNeighboringVertices.Count - 1];
                        int index = 0;
                        while (true)
                        {
                            if (index < this.m_UnusedNeighboringTriangles.Count)
                            {
                                int num6 = this.m_UnusedNeighboringTriangles[index];
                                int num7 = this.InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num6, a_VertexIndex);
                                num7 = this.SuccessorInnerTriangleVertexIndex(num7);
                                if (a_DecalsMesh.Triangles[num6 + num7] == num4)
                                {
                                    int item = a_DecalsMesh.Triangles[num6 + this.SuccessorInnerTriangleVertexIndex(num7)];
                                    this.m_SortedNeighboringVertices.Add(item);
                                    this.m_UnusedNeighboringTriangles.RemoveAt(index);
                                    flag = true;
                                }
                                else
                                {
                                    int num10 = num7;
                                    int num8 = a_DecalsMesh.Triangles[num6 + this.SuccessorInnerTriangleVertexIndex(num7)];
                                    if (num8 != num3)
                                    {
                                        index++;
                                        continue;
                                    }
                                    int item = a_DecalsMesh.Triangles[num6 + num10];
                                    this.m_SortedNeighboringVertices.Insert(0, item);
                                    this.m_UnusedNeighboringTriangles.RemoveAt(index);
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                continue;
                            }
                            else
                            {
                                this.m_SortedNeighboringVerticesInitialized = false;
                            }
                            break;
                        }
                    }
                    return;
                }
            }
        }

        private void InitializeSortedNeighboringInternalVertices(DecalsMesh a_DecalsMesh, int a_VertexIndex)
        {
            this.m_SortedNeighboringVerticesInitialized = true;
            this.m_UnusedNeighboringTriangles.AddRange(this.m_NeighboringTriangles);
            int num = this.m_UnusedNeighboringTriangles[this.m_UnusedNeighboringTriangles.Count - 1];
            int num2 = this.InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num, a_VertexIndex);
            num2 = this.SuccessorInnerTriangleVertexIndex(num2);
            this.m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + num2]);
            this.m_SortedNeighboringVertices.Add(a_DecalsMesh.Triangles[num + this.SuccessorInnerTriangleVertexIndex(num2)]);
            this.m_UnusedNeighboringTriangles.RemoveAt(this.m_UnusedNeighboringTriangles.Count - 1);
            while (true)
            {
                if (this.m_UnusedNeighboringTriangles.Count <= 1)
                {
                    break;
                }
                bool flag = false;
                int num3 = this.m_SortedNeighboringVertices[this.m_SortedNeighboringVertices.Count - 1];
                int index = 0;
                while (true)
                {
                    if (index < this.m_UnusedNeighboringTriangles.Count)
                    {
                        int num5 = this.m_UnusedNeighboringTriangles[index];
                        int num6 = this.InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num5, a_VertexIndex);
                        num6 = this.SuccessorInnerTriangleVertexIndex(num6);
                        int num7 = a_DecalsMesh.Triangles[num5 + num6];
                        if (num7 != num3)
                        {
                            index++;
                            continue;
                        }
                        int item = a_DecalsMesh.Triangles[num5 + this.SuccessorInnerTriangleVertexIndex(num6)];
                        this.m_SortedNeighboringVertices.Add(item);
                        this.m_UnusedNeighboringTriangles.RemoveAt(index);
                        flag = true;
                    }
                    if (!flag)
                    {
                        this.m_SortedNeighboringVerticesInitialized = false;
                    }
                    break;
                }
            }
            if (this.m_SortedNeighboringVerticesInitialized)
            {
                int num10 = this.m_UnusedNeighboringTriangles[0];
                int num11 = this.InnerTriangleIndexOfVertexIndex(a_DecalsMesh, num10, a_VertexIndex);
                num11 = this.SuccessorInnerTriangleVertexIndex(num11);
                if (this.m_SortedNeighboringVertices[this.m_SortedNeighboringVertices.Count - 1] != a_DecalsMesh.Triangles[num10 + num11])
                {
                    throw new InvalidOperationException("The last triangle doesn't match the previous ones.");
                }
                if (this.m_SortedNeighboringVertices[0] != a_DecalsMesh.Triangles[num10 + this.SuccessorInnerTriangleVertexIndex(num11)])
                {
                    throw new InvalidOperationException("The last triangle doesn't match to the first one.");
                }
            }
        }

        private int InnerTriangleIndexOfVertexIndex(DecalsMesh a_DecalsMesh, int a_TriangleIndex, int a_VertexIndex)
        {
            int num;
            if (a_DecalsMesh.Triangles[a_TriangleIndex] == a_VertexIndex)
            {
                num = 0;
            }
            else if (a_DecalsMesh.Triangles[a_TriangleIndex + 1] == a_VertexIndex)
            {
                num = 1;
            }
            else
            {
                if (a_DecalsMesh.Triangles[a_TriangleIndex + 2] != a_VertexIndex)
                {
                    throw new InvalidOperationException("The vertex index argument is not in the provided triangle.");
                }
                num = 2;
            }
            return num;
        }

        internal void RemoveObsoleteExternalVertex(DecalsMesh a_DecalsMesh, int a_VertexIndex, out bool a_SuccessfulRemoval)
        {
            this.InitializeNeighboringTriangles(a_DecalsMesh, a_VertexIndex);
            this.InitializeSortedNeighboringExternalVertices(a_DecalsMesh, a_VertexIndex);
            if (!this.m_SortedNeighboringVerticesInitialized)
            {
                a_SuccessfulRemoval = false;
            }
            else
            {
                this.RemoveUnneededTriangles(a_DecalsMesh);
                this.AddNewTriangles(a_DecalsMesh);
                a_DecalsMesh.ActiveDecalProjector.DecalsMeshUpperTriangleIndex = a_DecalsMesh.Triangles.Count - 1;
                a_SuccessfulRemoval = true;
            }
            this.Clear();
        }

        internal void RemoveObsoleteInternalVertex(DecalsMesh a_DecalsMesh, int a_VertexIndex, List<OptimizeEdge> a_RedundantInternalEdges, out bool a_SuccessfulRemoval)
        {
            this.InitializeNeighboringTriangles(a_DecalsMesh, a_VertexIndex);
            this.InitializeSortedNeighboringInternalVertices(a_DecalsMesh, a_VertexIndex);
            if (!this.m_SortedNeighboringVerticesInitialized)
            {
                a_SuccessfulRemoval = false;
            }
            else
            {
                this.RemoveUnneededTriangles(a_DecalsMesh);
                this.AddNewTriangles(a_DecalsMesh);
                a_DecalsMesh.ActiveDecalProjector.DecalsMeshUpperTriangleIndex = a_DecalsMesh.Triangles.Count - 1;
                a_SuccessfulRemoval = true;
            }
            this.Clear();
        }

        private void RemoveUnneededTriangles(DecalsMesh a_DecalsMesh)
        {
            this.m_NeighboringTriangles.Sort();
            int num = this.m_NeighboringTriangles.Count - 1;
            while (num >= 0)
            {
                int num2 = this.m_NeighboringTriangles[num];
                int num3 = 1;
                while (true)
                {
                    if ((num <= 0) || ((num2 - 3) != this.m_NeighboringTriangles[num - 1]))
                    {
                        a_DecalsMesh.RemoveTrianglesAt(num2, num3);
                        num--;
                        break;
                    }
                    num2 -= 3;
                    num3++;
                    num--;
                }
            }
        }

        private int SuccessorInnerTriangleVertexIndex(int a_InnerTriangleVertexIndex)
        {
            int num = a_InnerTriangleVertexIndex + 1;
            if (num == 3)
            {
                num = 0;
            }
            return num;
        }
    }
}

