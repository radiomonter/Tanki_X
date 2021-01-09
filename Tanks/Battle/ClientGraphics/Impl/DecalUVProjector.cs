namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class DecalUVProjector
    {
        private float NORMAL_TO_OFFSET_TRESHOLD = 0.4f;
        private float DEPTH_TO_OFFSET_KOEF = 0.6f;

        private Vector2 GetAtlasOffset(int tilesX, int tilesY, int position)
        {
            Vector2 vector = new Vector2(1f / ((float) tilesX), 1f / ((float) tilesY));
            int num = tilesX * tilesY;
            position = position % num;
            int num2 = position % tilesX;
            int num3 = position / tilesY;
            return new Vector2((((-tilesX * 0.5f) + 0.5f) + num2) * vector.x, (((tilesY * 0.5f) - 0.5f) - num3) * vector.y);
        }

        private Vector3 GetProjectDirectionAsMiddleNormal(DecalProjection projection, List<int> triangles, List<Vector3> vertices, List<Vector3> normals)
        {
            Vector3 zero = Vector3.zero;
            for (int i = 0; i < triangles.Count; i += 3)
            {
                int num2 = triangles[i];
                int num3 = triangles[i + 1];
                int num4 = triangles[i + 2];
                Vector3 vector2 = vertices[num2];
                Vector3 vector3 = vertices[num3];
                Vector3 vector4 = vertices[num4];
                zero += normals[num2] * this.GetTriangleSquare(vector2, vector3, vector4);
            }
            return (!zero.Equals(Vector3.zero) ? -zero.normalized : projection.Ray.direction);
        }

        private float GetTriangleSquare(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            float magnitude = (v0 - v1).magnitude;
            float num2 = (v0 - v2).magnitude;
            float num3 = (v1 - v2).magnitude;
            float num4 = ((magnitude + num2) + num3) * 0.5f;
            return Mathf.Sqrt(((num4 * (num4 - magnitude)) * (num4 - num2)) * (num4 - num3));
        }

        public void Project(MeshBuilder builder, DecalProjection projection)
        {
            List<Vector3> normals = builder.Normals;
            List<Vector3> vertices = builder.Vertices;
            List<int> triangles = builder.Triangles;
            List<SurfaceType> surfaceTypes = builder.SurfaceTypes;
            float num = projection.HalfSize * 1.1f;
            float num2 = (num * 2f) * projection.AtlasHTilesCount;
            float num3 = (num * 2f) * projection.AtlasVTilesCount;
            Quaternion quaternion2 = Quaternion.Inverse(Quaternion.LookRotation(this.GetProjectDirectionAsMiddleNormal(projection, triangles, vertices, normals), projection.Up));
            for (int i = 0; i < vertices.Count; i++)
            {
                int position = projection.SurfaceAtlasPositions[surfaceTypes[i]];
                Vector2 vector2 = this.GetAtlasOffset(projection.AtlasHTilesCount, projection.AtlasVTilesCount, position);
                Vector3 vector3 = vertices[i];
                Vector3 vector4 = normals[i];
                RaycastHit projectionHit = projection.ProjectionHit;
                Vector3 vector5 = (Vector3) (quaternion2 * (vector3 - projectionHit.point));
                Vector3 vector6 = (Vector3) (quaternion2 * vector4);
                vector6.x = Mathf.Abs(vector6.x);
                vector6.y = Mathf.Abs(vector6.y);
                float num6 = Mathf.Abs(vector5.z) * this.DEPTH_TO_OFFSET_KOEF;
                float x = vector5.x;
                float y = vector5.y;
                if (vector6.x > this.NORMAL_TO_OFFSET_TRESHOLD)
                {
                    x += (Mathf.Sign(vector5.x) * vector6.x) * num6;
                }
                if (vector6.y > this.NORMAL_TO_OFFSET_TRESHOLD)
                {
                    y += (Mathf.Sign(vector5.y) * vector6.y) * num6;
                }
                builder.AddUV(new Vector2(((x / num2) + 0.5f) + vector2.x, ((y / num3) + 0.5f) + vector2.y));
            }
        }
    }
}

