namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class DecalProjector : DecalProjectorBase
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public float cullingAngle;
        public float meshOffset;
        public int uv1RectangleIndex;
        public int uv2RectangleIndex;
        public Color vertexColor;
        private float m_VertexColorBlending;

        public DecalProjector(Vector3 a_Position, Quaternion a_Rotation, Vector3 a_Scale, float a_CullingAngle, float a_meshOffset, int a_UV1RectangleIndex, int a_UV2RectangleIndex)
        {
            this.position = a_Position;
            this.rotation = a_Rotation;
            this.scale = a_Scale;
            this.cullingAngle = a_CullingAngle;
            this.meshOffset = a_meshOffset;
            this.uv1RectangleIndex = a_UV1RectangleIndex;
            this.uv2RectangleIndex = a_UV2RectangleIndex;
            this.vertexColor = Color.white;
            this.SetVertexColorBlending(0f);
        }

        public DecalProjector(Vector3 a_Position, Quaternion a_Rotation, Vector3 a_Scale, float a_CullingAngle, float a_MeshOffset, int a_UV1RectangleIndex, int a_UV2RectangleIndex, Color a_VertexColor, float a_VertexColorBlending)
        {
            if ((a_VertexColorBlending < 0f) || (a_VertexColorBlending > 1f))
            {
                throw new ArgumentOutOfRangeException("The blend value has to be in [0.0f, 1.0f].");
            }
            this.position = a_Position;
            this.rotation = a_Rotation;
            this.scale = a_Scale;
            this.cullingAngle = a_CullingAngle;
            this.meshOffset = a_MeshOffset;
            this.uv1RectangleIndex = a_UV1RectangleIndex;
            this.uv2RectangleIndex = a_UV2RectangleIndex;
            this.vertexColor = a_VertexColor;
            this.SetVertexColorBlending(a_VertexColorBlending);
        }

        public void SetVertexColorBlending(float a_VertexColorBlending)
        {
            if ((a_VertexColorBlending < 0f) || (a_VertexColorBlending > 1f))
            {
                throw new ArgumentOutOfRangeException("The blend value has to be in [0.0f, 1.0f].");
            }
            this.m_VertexColorBlending = a_VertexColorBlending;
        }

        public override Vector3 Position =>
            this.position;

        public override Quaternion Rotation =>
            this.rotation;

        public override Vector3 Scale =>
            this.scale;

        public override float CullingAngle =>
            this.cullingAngle;

        public override float MeshOffset =>
            this.meshOffset;

        public override int UV1RectangleIndex =>
            this.uv1RectangleIndex;

        public override int UV2RectangleIndex =>
            this.uv2RectangleIndex;

        public override Color VertexColor =>
            this.vertexColor;

        public override float VertexColorBlending =>
            this.m_VertexColorBlending;
    }
}

