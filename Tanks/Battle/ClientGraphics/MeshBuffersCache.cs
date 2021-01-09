namespace Tanks.Battle.ClientGraphics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class MeshBuffersCache
    {
        private const string SURFACE_TYPE_PROPERTY = "_SurfaceType";
        private Dictionary<Mesh, int[]> trianglesCache = new Dictionary<Mesh, int[]>();
        private Dictionary<Mesh, Vector3[]> verticesCache = new Dictionary<Mesh, Vector3[]>();
        private Dictionary<Mesh, Vector3[]> normalsCache = new Dictionary<Mesh, Vector3[]>();
        private Dictionary<Mesh, Vector4[]> tangentsCache = new Dictionary<Mesh, Vector4[]>();
        private Dictionary<Mesh, Vector2[]> uvsCache = new Dictionary<Mesh, Vector2[]>();
        private Dictionary<Mesh, SurfaceType[]> surfaceTypesCache = new Dictionary<Mesh, SurfaceType[]>();
        private Dictionary<Mesh, float[]> triangleRadiusCache = new Dictionary<Mesh, float[]>();
        private Dictionary<Mesh, Vector3[]> triangleMidlesCache = new Dictionary<Mesh, Vector3[]>();

        private float CalculateTriangleSphere(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 vector = ((v1 + v2) + v3) / 3f;
            return Mathf.Max(Mathf.Max((vector - v1).sqrMagnitude, (vector - v2).sqrMagnitude), (vector - v3).sqrMagnitude);
        }

        private SurfaceType[] CollectSurfaceTypes(Mesh mesh, Renderer renderer)
        {
            SurfaceType[] typeArray = new SurfaceType[this.GetVertices(mesh).Length];
            int num = Math.Min(renderer.sharedMaterials.Length, mesh.subMeshCount);
            int submesh = 0;
            while (submesh < num)
            {
                int[] triangles = mesh.GetTriangles(submesh);
                int index = 0;
                while (true)
                {
                    if (index >= triangles.Length)
                    {
                        submesh++;
                        break;
                    }
                    Material material = renderer.sharedMaterials[submesh];
                    SurfaceType uNDEFINED = SurfaceType.UNDEFINED;
                    if (material && material.HasProperty("_SurfaceType"))
                    {
                        uNDEFINED = (SurfaceType) material.GetInt("_SurfaceType");
                    }
                    typeArray[triangles[index]] = uNDEFINED;
                    index++;
                }
            }
            return typeArray;
        }

        public Vector3[] GetNormals(Mesh mesh)
        {
            Vector3[] normals;
            if (!this.normalsCache.TryGetValue(mesh, out normals))
            {
                normals = mesh.normals;
                this.normalsCache.Add(mesh, normals);
            }
            return normals;
        }

        public SurfaceType[] GetSurfaceTypes(Mesh mesh, Renderer renderer)
        {
            SurfaceType[] typeArray;
            if (!this.surfaceTypesCache.TryGetValue(mesh, out typeArray))
            {
                typeArray = this.CollectSurfaceTypes(mesh, renderer);
                this.surfaceTypesCache.Add(mesh, typeArray);
            }
            return typeArray;
        }

        public Vector4[] GetTangents(Mesh mesh)
        {
            Vector4[] tangents;
            if (!this.tangentsCache.TryGetValue(mesh, out tangents))
            {
                tangents = mesh.tangents;
                this.tangentsCache.Add(mesh, tangents);
            }
            return tangents;
        }

        public void GetTriangleRadius(Mesh mesh, out float[] triangleRadius, out Vector3[] triangleMidles)
        {
            if (this.triangleRadiusCache.TryGetValue(mesh, out triangleRadius))
            {
                triangleMidles = this.triangleMidlesCache[mesh];
            }
            else
            {
                int[] triangles = this.GetTriangles(mesh);
                Vector3[] vertices = this.GetVertices(mesh);
                triangleRadius = new float[triangles.Length / 3];
                triangleMidles = new Vector3[triangles.Length / 3];
                int index = 0;
                while (true)
                {
                    if (index >= triangleRadius.Length)
                    {
                        this.triangleMidlesCache.Add(mesh, triangleMidles);
                        this.triangleRadiusCache.Add(mesh, triangleRadius);
                        break;
                    }
                    int num2 = triangles[index * 3];
                    int num3 = triangles[(index * 3) + 1];
                    int num4 = triangles[(index * 3) + 2];
                    Vector3 vector = vertices[num2];
                    Vector3 vector2 = vertices[num3];
                    Vector3 vector3 = vertices[num4];
                    Vector3 vector4 = ((vector + vector2) + vector3) / 3f;
                    Vector3 vector5 = vector4 - vector;
                    Vector3 vector6 = vector4 - vector2;
                    Vector3 vector7 = vector4 - vector3;
                    triangleRadius[index] = Mathf.Max(Mathf.Max(vector5.magnitude, vector6.magnitude), vector7.magnitude);
                    triangleMidles[index] = vector4;
                    index++;
                }
            }
        }

        public int[] GetTriangles(Mesh mesh)
        {
            int[] triangles;
            if (!this.trianglesCache.TryGetValue(mesh, out triangles))
            {
                triangles = mesh.triangles;
                this.trianglesCache.Add(mesh, triangles);
            }
            return triangles;
        }

        public Vector2[] GetUVs(Mesh mesh)
        {
            Vector2[] uv;
            if (!this.uvsCache.TryGetValue(mesh, out uv))
            {
                uv = mesh.uv;
                this.uvsCache.Add(mesh, uv);
            }
            return uv;
        }

        public Vector3[] GetVertices(Mesh mesh)
        {
            Vector3[] vertices;
            if (!this.verticesCache.TryGetValue(mesh, out vertices))
            {
                vertices = mesh.vertices;
                this.verticesCache.Add(mesh, vertices);
            }
            return vertices;
        }
    }
}

