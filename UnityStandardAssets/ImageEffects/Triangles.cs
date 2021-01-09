namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    internal class Triangles
    {
        private static Mesh[] meshes;
        private static int currentTris;

        private static void Cleanup()
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
            Vector3[] vectorArray = new Vector3[triCount * 3];
            Vector2[] vectorArray2 = new Vector2[triCount * 3];
            Vector2[] vectorArray3 = new Vector2[triCount * 3];
            int[] numArray = new int[triCount * 3];
            for (int i = 0; i < triCount; i++)
            {
                int index = i * 3;
                int num3 = triOffset + i;
                float x = Mathf.Floor((float) (num3 % totalWidth)) / ((float) totalWidth);
                float y = Mathf.Floor((float) (num3 / totalWidth)) / ((float) totalHeight);
                Vector3 vector = new Vector3((x * 2f) - 1f, (y * 2f) - 1f, 1f);
                vectorArray[index] = vector;
                vectorArray[index + 1] = vector;
                vectorArray[index + 2] = vector;
                vectorArray2[index] = new Vector2(0f, 0f);
                vectorArray2[index + 1] = new Vector2(1f, 0f);
                vectorArray2[index + 2] = new Vector2(0f, 1f);
                vectorArray3[index] = new Vector2(x, y);
                vectorArray3[index + 1] = new Vector2(x, y);
                vectorArray3[index + 2] = new Vector2(x, y);
                numArray[index] = index;
                numArray[index + 1] = index + 1;
                numArray[index + 2] = index + 2;
            }
            mesh.vertices = vectorArray;
            mesh.triangles = numArray;
            mesh.uv = vectorArray2;
            mesh.uv2 = vectorArray3;
            return mesh;
        }

        private static Mesh[] GetMeshes(int totalWidth, int totalHeight)
        {
            if (!HasMeshes() || (currentTris != (totalWidth * totalHeight)))
            {
                int max = 0x54a2;
                int num2 = totalWidth * totalHeight;
                currentTris = num2;
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
            for (int i = 0; i < meshes.Length; i++)
            {
                if (null == meshes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

