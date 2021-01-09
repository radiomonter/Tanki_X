namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    internal class Quads
    {
        private static Mesh[] meshes;
        private static int currentQuads;

        public static void Cleanup()
        {
            if (meshes != null)
            {
                for (int i = 0; i < meshes.Length; i++)
                {
                    if (null != meshes[i])
                    {
                        Object.DestroyImmediate(meshes[i]);
                        meshes[i] = null;
                    }
                }
                meshes = null;
            }
        }

        private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
        {
            Mesh mesh = new Mesh {
                hideFlags = HideFlags.DontSave
            };
            Vector3[] vectorArray = new Vector3[triCount * 4];
            Vector2[] vectorArray2 = new Vector2[triCount * 4];
            Vector2[] vectorArray3 = new Vector2[triCount * 4];
            int[] numArray = new int[triCount * 6];
            for (int i = 0; i < triCount; i++)
            {
                int index = i * 4;
                int num3 = i * 6;
                int num4 = triOffset + i;
                float x = Mathf.Floor((float) (num4 % totalWidth)) / ((float) totalWidth);
                float y = Mathf.Floor((float) (num4 / totalWidth)) / ((float) totalHeight);
                Vector3 vector = new Vector3((x * 2f) - 1f, (y * 2f) - 1f, 1f);
                vectorArray[index] = vector;
                vectorArray[index + 1] = vector;
                vectorArray[index + 2] = vector;
                vectorArray[index + 3] = vector;
                vectorArray2[index] = new Vector2(0f, 0f);
                vectorArray2[index + 1] = new Vector2(1f, 0f);
                vectorArray2[index + 2] = new Vector2(0f, 1f);
                vectorArray2[index + 3] = new Vector2(1f, 1f);
                vectorArray3[index] = new Vector2(x, y);
                vectorArray3[index + 1] = new Vector2(x, y);
                vectorArray3[index + 2] = new Vector2(x, y);
                vectorArray3[index + 3] = new Vector2(x, y);
                numArray[num3] = index;
                numArray[num3 + 1] = index + 1;
                numArray[num3 + 2] = index + 2;
                numArray[num3 + 3] = index + 1;
                numArray[num3 + 4] = index + 2;
                numArray[num3 + 5] = index + 3;
            }
            mesh.vertices = vectorArray;
            mesh.triangles = numArray;
            mesh.uv = vectorArray2;
            mesh.uv2 = vectorArray3;
            return mesh;
        }

        public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
        {
            if (!HasMeshes() || (currentQuads != (totalWidth * totalHeight)))
            {
                int max = 0x2a51;
                int num2 = totalWidth * totalHeight;
                currentQuads = num2;
                meshes = new Mesh[Mathf.CeilToInt((1f * num2) / (1f * max))];
                int triOffset = 0;
                int index = 0;
                for (triOffset = 0; triOffset < num2; triOffset += max)
                {
                    int triCount = Mathf.FloorToInt((float) Mathf.Clamp(num2 - triOffset, 0, max));
                    meshes[index] = GetMesh(triCount, triOffset, totalWidth, totalHeight);
                    index++;
                }
            }
            return meshes;
        }

        private static bool HasMeshes()
        {
            if (meshes == null)
            {
                return false;
            }
            foreach (Mesh mesh in meshes)
            {
                if (null == mesh)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

