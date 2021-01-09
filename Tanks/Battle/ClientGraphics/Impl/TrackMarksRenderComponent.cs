namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    internal class TrackMarksRenderComponent : Component
    {
        public Mesh mesh;
        public bool dirty;
        public TrackRenderData[] trackRenderDatas;
        public Vector3[] meshPositions;
        public Vector2[] meshUVs;
        public Vector3[] meshNormals;
        public Color[] meshColors;
        public int[] meshTris;

        public void Clear()
        {
            this.mesh.Clear();
            for (int i = 0; i < this.trackRenderDatas.Length; i++)
            {
                this.trackRenderDatas[i].Reset();
            }
            for (int j = 0; j < this.meshPositions.Length; j++)
            {
                this.meshPositions[j] = Vector3.zero;
                this.meshUVs[j] = Vector2.zero;
                this.meshNormals[j] = Vector3.zero;
                this.meshColors[j] = Color.white;
            }
            for (int k = 0; k < this.meshTris.Length; k++)
            {
                this.meshTris[k] = 0;
            }
        }
    }
}

