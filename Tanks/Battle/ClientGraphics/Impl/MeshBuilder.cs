namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MeshBuilder
    {
        private const int INITIAL_BUFFER_SIZE = 100;
        private Dictionary<long, int> hashToIndex = new Dictionary<long, int>();
        private List<int> triangles = new List<int>(300);
        private List<Vector3> vertices = new List<Vector3>(100);
        private List<Vector3> normals = new List<Vector3>(100);
        private List<Vector2> uvs = new List<Vector2>(100);
        private List<Vector2> uvs2 = new List<Vector2>(100);
        private List<SurfaceType> surfaceTypes = new List<SurfaceType>(100);
        private List<Color> colors = new List<Color>(100);
        private Color color;

        public void AddTriangle(int id0, int id1, int id2)
        {
            this.triangles.Add(id0);
            this.triangles.Add(id1);
            this.triangles.Add(id2);
        }

        public void AddUV(Vector2 uv)
        {
            this.uvs.Add(uv);
        }

        public int AddVertex(VertexData vertexData)
        {
            int count = this.vertices.Count;
            this.vertices.Add(vertexData.vertex);
            this.normals.Add(vertexData.normal);
            this.surfaceTypes.Add(vertexData.surfaceType);
            return count;
        }

        public int AddVertexWeld(long weldHash, VertexData vertexData, Vector2 uv, Color color)
        {
            int num;
            if (!this.hashToIndex.TryGetValue(weldHash, out num))
            {
                num = this.AddVertex(vertexData);
                this.hashToIndex.Add(weldHash, num);
                this.uvs.Add(uv);
                this.colors.Add(color);
            }
            return num;
        }

        public int AddVertexWeld(long weldHash, VertexData vertexData, Vector2 uv, Vector2 uv2, Color color)
        {
            int num;
            if (!this.hashToIndex.TryGetValue(weldHash, out num))
            {
                num = this.AddVertex(vertexData);
                this.hashToIndex.Add(weldHash, num);
                this.uvs.Add(uv);
                this.uvs2.Add(uv2);
                this.colors.Add(color);
            }
            return num;
        }

        public int AddVertexWeldAndTransform(long weldHash, VertexData vertexData, Transform transform)
        {
            int count;
            if (!this.hashToIndex.TryGetValue(weldHash, out count))
            {
                count = this.vertices.Count;
                this.vertices.Add(transform.TransformPoint(vertexData.vertex));
                this.normals.Add(transform.TransformDirection(vertexData.normal));
                this.surfaceTypes.Add(vertexData.surfaceType);
                this.hashToIndex.Add(weldHash, count);
            }
            return count;
        }

        public bool BuildToMesh(Mesh mesh, bool tangents)
        {
            if (this.Vertices.Count == 0)
            {
                return false;
            }
            mesh.Clear();
            mesh.vertices = this.Vertices.ToArray();
            mesh.normals = this.Normals.ToArray();
            mesh.triangles = this.Triangles.ToArray();
            mesh.uv = this.UVs.ToArray();
            if (this.uvs2.Count > 0)
            {
                mesh.uv2 = this.uvs2.ToArray();
            }
            mesh.colors = this.colors.ToArray();
            if (tangents)
            {
                mesh.tangents = this.CalculateTangents();
            }
            mesh.RecalculateBounds();
            return true;
        }

        public unsafe Vector4[] CalculateTangents()
        {
            int count = this.vertices.Count;
            int num2 = this.triangles.Count / 3;
            Vector4[] vectorArray = new Vector4[count];
            Vector3[] vectorArray2 = new Vector3[count];
            Vector3[] vectorArray3 = new Vector3[count];
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int index = this.triangles[num3];
                int num6 = this.triangles[num3 + 1];
                int num7 = this.triangles[num3 + 2];
                Vector3 vector = this.vertices[index];
                Vector3 vector2 = this.vertices[num6];
                Vector3 vector3 = this.vertices[num7];
                Vector2 vector4 = this.uvs[index];
                Vector2 vector5 = this.uvs[num6];
                Vector2 vector6 = this.uvs[num7];
                float num8 = vector2.x - vector.x;
                float num9 = vector3.x - vector.x;
                float num10 = vector2.y - vector.y;
                float num11 = vector3.y - vector.y;
                float num12 = vector2.z - vector.z;
                float num13 = vector3.z - vector.z;
                float num14 = vector5.x - vector4.x;
                float num15 = vector6.x - vector4.x;
                float num16 = vector5.y - vector4.y;
                float num17 = vector6.y - vector4.y;
                float num18 = (num14 * num17) - (num15 * num16);
                float num19 = 0f;
                if (num18 != 0f)
                {
                    num19 = 1f / num18;
                }
                Vector3 vector7 = new Vector3(((num17 * num8) - (num16 * num9)) * num19, ((num17 * num10) - (num16 * num11)) * num19, ((num17 * num12) - (num16 * num13)) * num19);
                Vector3 vector8 = new Vector3(((num14 * num9) - (num15 * num8)) * num19, ((num14 * num11) - (num15 * num10)) * num19, ((num14 * num13) - (num15 * num12)) * num19);
                Vector3* vectorPtr1 = &(vectorArray2[index]);
                vectorPtr1[0] += vector7;
                Vector3* vectorPtr2 = &(vectorArray2[num6]);
                vectorPtr2[0] += vector7;
                Vector3* vectorPtr3 = &(vectorArray2[num7]);
                vectorPtr3[0] += vector7;
                Vector3* vectorPtr4 = &(vectorArray3[index]);
                vectorPtr4[0] += vector8;
                Vector3* vectorPtr5 = &(vectorArray3[num6]);
                vectorPtr5[0] += vector8;
                Vector3* vectorPtr6 = &(vectorArray3[num7]);
                vectorPtr6[0] += vector8;
                num3 += 3;
            }
            for (int j = 0; j < count; j++)
            {
                Vector3 normal = this.normals[j];
                Vector3 tangent = vectorArray2[j];
                Vector3.OrthoNormalize(ref normal, ref tangent);
                vectorArray[j].x = tangent.x;
                vectorArray[j].y = tangent.y;
                vectorArray[j].z = tangent.z;
                vectorArray[j].w = (Vector3.Dot(Vector3.Cross(normal, tangent), vectorArray3[j]) >= 0f) ? 1f : -1f;
            }
            return vectorArray;
        }

        public void Clear()
        {
            this.hashToIndex.Clear();
            this.triangles.Clear();
            this.vertices.Clear();
            this.normals.Clear();
            this.uvs.Clear();
            this.uvs2.Clear();
            this.colors.Clear();
            this.surfaceTypes.Clear();
        }

        public void ClearWeldHashing()
        {
            this.hashToIndex.Clear();
        }

        public List<int> Triangles =>
            this.triangles;

        public List<Vector3> Vertices =>
            this.vertices;

        public List<Vector3> Normals =>
            this.normals;

        public List<Vector2> UVs =>
            this.uvs;

        public List<SurfaceType> SurfaceTypes =>
            this.surfaceTypes;

        public List<Vector2> UVs2 =>
            this.uvs2;
    }
}

