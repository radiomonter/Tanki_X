namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class GrassInstancePrototypes
    {
        private List<Mesh> grassMeshes = new List<Mesh>();
        private List<GrassPrefabData> grassPrefabDataList;

        private Mesh CreateMesh(Mesh sourceMesh) => 
            new Mesh { 
                vertices = sourceMesh.vertices,
                triangles = sourceMesh.triangles,
                bounds = sourceMesh.bounds,
                normals = sourceMesh.normals,
                uv = sourceMesh.uv
            };

        public void CreatePrototypes(List<GrassPrefabData> grassPrefabDataList)
        {
            if (grassPrefabDataList.Count == 0)
            {
                throw new ArgumentException("GrassPrefabDataList can't be empty");
            }
            this.grassPrefabDataList = grassPrefabDataList;
            for (int i = 0; i < grassPrefabDataList.Count; i++)
            {
                GameObject grassPrefab = grassPrefabDataList[i].grassPrefab;
                Mesh sharedMesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
                Mesh item = this.CreateMesh(sharedMesh);
                this.grassMeshes.Add(item);
            }
            for (int j = 0; j < grassPrefabDataList.Count; j++)
            {
                GameObject grassPrefab = grassPrefabDataList[j].grassPrefab;
                Mesh sharedMesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
                this.grassMeshes.Add(sharedMesh);
            }
        }

        public void DestroyPrototypes()
        {
            for (int i = (this.grassMeshes.Count - this.grassPrefabDataList.Count) - 1; i >= 0; i--)
            {
                Mesh mesh = this.grassMeshes[i];
                Object.DestroyImmediate(mesh);
            }
            this.grassMeshes = null;
        }

        public void GetPrototype(int index, out Mesh mesh, out GrassPrefabData grassPrefabData)
        {
            if ((index < 0) || (index >= this.grassMeshes.Count))
            {
                throw new GrassPrototypeIndexOutOfRange($"Index: {index}, prototypes count: {this.grassMeshes.Count}");
            }
            mesh = this.grassMeshes[index];
            int num = index - (this.grassPrefabDataList.Count * (index / this.grassPrefabDataList.Count));
            grassPrefabData = this.grassPrefabDataList[num];
        }

        public void GetRandomPrototype(out Mesh mesh, out GrassPrefabData grassPrefabData)
        {
            int index = Random.Range(0, this.grassMeshes.Count);
            this.GetPrototype(index, out mesh, out grassPrefabData);
        }

        public int PrototypesCount =>
            this.grassMeshes.Count;
    }
}

