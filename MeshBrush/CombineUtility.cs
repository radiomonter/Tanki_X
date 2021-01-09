namespace MeshBrush
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CombineUtility
    {
        private static int vertexCount;
        private static int triangleCount;
        private static int stripCount;
        private static int curStripCount;
        private static Vector3[] vertices;
        private static Vector3[] normals;
        private static Vector4[] tangents;
        private static Vector2[] uv;
        private static Vector2[] uv1;
        private static Color[] colors;
        private static int[] triangles;
        private static int[] strip;
        private static int offset;
        private static int triangleOffset;
        private static int stripOffset;
        private static int vertexOffset;
        private static Matrix4x4 invTranspose;
        public const string combinedMeshName = "Combined Mesh";
        private static Vector4 p4;
        private static Vector3 p;

        public static Mesh Combine(MeshInstance[] combines, bool generateStrips)
        {
            vertexCount = 0;
            triangleCount = 0;
            stripCount = 0;
            foreach (MeshInstance instance in combines)
            {
                if (instance.mesh)
                {
                    vertexCount += instance.mesh.vertexCount;
                    if (generateStrips)
                    {
                        curStripCount = instance.mesh.GetTriangles(instance.subMeshIndex).Length;
                        if (curStripCount == 0)
                        {
                            generateStrips = false;
                        }
                        else
                        {
                            if (stripCount != 0)
                            {
                                stripCount = ((stripCount & 1) != 1) ? (stripCount + 2) : (stripCount + 3);
                            }
                            stripCount += curStripCount;
                        }
                    }
                }
            }
            if (!generateStrips)
            {
                foreach (MeshInstance instance2 in combines)
                {
                    if (instance2.mesh)
                    {
                        triangleCount += instance2.mesh.GetTriangles(instance2.subMeshIndex).Length;
                    }
                }
            }
            vertices = new Vector3[vertexCount];
            normals = new Vector3[vertexCount];
            tangents = new Vector4[vertexCount];
            uv = new Vector2[vertexCount];
            uv1 = new Vector2[vertexCount];
            colors = new Color[vertexCount];
            CombineUtility.triangles = new int[triangleCount];
            strip = new int[stripCount];
            offset = 0;
            foreach (MeshInstance instance3 in combines)
            {
                if (instance3.mesh)
                {
                    Copy(instance3.mesh.vertexCount, instance3.mesh.vertices, vertices, ref offset, instance3.transform);
                }
            }
            offset = 0;
            foreach (MeshInstance instance4 in combines)
            {
                if (instance4.mesh)
                {
                    invTranspose = instance4.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyNormal(instance4.mesh.vertexCount, instance4.mesh.normals, normals, ref offset, invTranspose);
                }
            }
            offset = 0;
            foreach (MeshInstance instance5 in combines)
            {
                if (instance5.mesh)
                {
                    invTranspose = instance5.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyTangents(instance5.mesh.vertexCount, instance5.mesh.tangents, tangents, ref offset, invTranspose);
                }
            }
            offset = 0;
            foreach (MeshInstance instance6 in combines)
            {
                if (instance6.mesh)
                {
                    Copy(instance6.mesh.vertexCount, instance6.mesh.uv, uv, ref offset);
                }
            }
            offset = 0;
            foreach (MeshInstance instance7 in combines)
            {
                if (instance7.mesh)
                {
                    Copy(instance7.mesh.vertexCount, instance7.mesh.uv2, uv1, ref offset);
                }
            }
            offset = 0;
            foreach (MeshInstance instance8 in combines)
            {
                if (instance8.mesh)
                {
                    CopyColors(instance8.mesh.vertexCount, instance8.mesh.colors, colors, ref offset);
                }
            }
            triangleOffset = 0;
            stripOffset = 0;
            vertexOffset = 0;
            foreach (MeshInstance instance9 in combines)
            {
                if (instance9.mesh)
                {
                    if (!generateStrips)
                    {
                        int[] triangles = instance9.mesh.GetTriangles(instance9.subMeshIndex);
                        int index = 0;
                        while (true)
                        {
                            if (index >= triangles.Length)
                            {
                                triangleOffset += triangles.Length;
                                break;
                            }
                            CombineUtility.triangles[index + triangleOffset] = triangles[index] + vertexOffset;
                            index++;
                        }
                    }
                    else
                    {
                        int[] triangles = instance9.mesh.GetTriangles(instance9.subMeshIndex);
                        if (stripOffset != 0)
                        {
                            if ((stripOffset & 1) != 1)
                            {
                                strip[stripOffset] = strip[stripOffset - 1];
                                strip[stripOffset + 1] = triangles[0] + vertexOffset;
                                stripOffset += 2;
                            }
                            else
                            {
                                strip[stripOffset] = strip[stripOffset - 1];
                                strip[stripOffset + 1] = triangles[0] + vertexOffset;
                                strip[stripOffset + 2] = triangles[0] + vertexOffset;
                                stripOffset += 3;
                            }
                        }
                        int index = 0;
                        while (true)
                        {
                            if (index >= triangles.Length)
                            {
                                stripOffset += triangles.Length;
                                break;
                            }
                            strip[index + stripOffset] = triangles[index] + vertexOffset;
                            index++;
                        }
                    }
                    vertexOffset += instance9.mesh.vertexCount;
                }
            }
            Mesh mesh = new Mesh {
                name = "Combined Mesh",
                vertices = vertices,
                normals = normals,
                colors = colors,
                uv = uv,
                uv2 = uv1,
                tangents = tangents
            };
            if (generateStrips)
            {
                mesh.SetTriangles(strip, 0);
            }
            else
            {
                mesh.triangles = CombineUtility.triangles;
            }
            return mesh;
        }

        private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dst[i + offset] = src[i];
            }
            offset += vertexcount;
        }

        private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dst[i + offset] = transform.MultiplyPoint(src[i]);
            }
            offset += vertexcount;
        }

        private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dst[i + offset] = src[i];
            }
            offset += vertexcount;
        }

        private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                Vector3 vector = transform.MultiplyVector(src[i]);
                dst[i + offset] = vector.normalized;
            }
            offset += vertexcount;
        }

        private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                p4 = src[i];
                p = new Vector3(p4.x, p4.y, p4.z);
                Vector3 vector = transform.MultiplyVector(p);
                p = vector.normalized;
                dst[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
            }
            offset += vertexcount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MeshInstance
        {
            public Mesh mesh;
            public int subMeshIndex;
            public Matrix4x4 transform;
        }
    }
}

